using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DronesController : IUpdatableController, IInitableController, IDisposable
{
    private IChangableDronesHolder _dronesHolder;
    private IDroneSpawner _droneSpawner;
    private IFreeResourceFinder _finder;
    private IMainUINotifier _uiNotifier;
    private GameConfig _gameConfig;

    private bool _isActive;
    private float _sqrLockDistance;
    private float _droneSpeed;
    private int _droneCount;

    public DronesController(IChangableDronesHolder dronesHolder, GameConfig gameConfig, IFreeResourceFinder finder, IMainUINotifier uiNotifier, IDroneSpawner droneSpawner)
    {
        _dronesHolder = dronesHolder;
        _gameConfig = gameConfig;
        _finder = finder;
        _uiNotifier = uiNotifier;
        _droneSpawner = droneSpawner;

        _droneSpeed = gameConfig.StartDronesSpeed;
        _droneCount = gameConfig.StartDronesCount;

        _uiNotifier.OnDroneSpeedChange += ChangeDronesSpeed;
        _uiNotifier.OnDronesCountChange += ChangeDronesCount;
    }

    private void ChangeDronesCount(int newCont)
    {
        if(newCont == _droneCount) return;

        if(_droneCount < newCont)
        {
            SpawnNewDrones(newCont - _droneCount);
        }
        else
        {
            RemoveDrones(_droneCount - newCont);
        }

        _droneCount = newCont;
    }

    private void RemoveDrones(int count)
    {
        foreach (var drones in _dronesHolder.Drones.Values)
        {
            RemoveDronesInFraction(drones, count);
        }
    }

    private void RemoveDronesInFraction(List<DroneModel> drones, int count)
    {
        var remainigCount = count;

        while (remainigCount > 0)
        {
            for(int i = drones.Count - 1; i >= 0; i--)
            {
                if (drones[i].State != DroneStateType.HandOver && remainigCount > 0)
                {
                    var view = drones[i].View;
                    var model = drones[i];

                    _dronesHolder.RemoveDrone(model);
                    model.Clear();
                    _droneSpawner.DespawnDrone(view);
                    remainigCount--;

                    if (remainigCount == 0)
                        break;
                }
            }
        }
    }

    private void SpawnNewDrones(int count)
    {
        foreach(var fraction in _dronesHolder.Drones.Keys)
        {
            var priority = _droneCount;

            for (int i = 0; i < count; i++)
            {
                _droneSpawner.SpawnDrones(fraction, priority, _droneSpeed);
                priority++;
            }
        }
    }

    private void ChangeDronesSpeed(float speed)
    {
        _droneSpeed = speed;

        foreach(var drones in _dronesHolder.Drones.Values)
        {
            for(int i = 0; i < drones.Count; i++)
            {
                drones[i].View.Agent.speed = speed;
            }
        }
    }

    public void Init()
    {
        _isActive = true;

        _sqrLockDistance = _gameConfig.LockResourceDistance * _gameConfig.LockResourceDistance;
    }

    public void Update()
    {
        if (!_isActive)
            return;

        foreach (var drones in _dronesHolder.Drones.Values)
        {
            for(int i = 0; i < drones.Count; i++)
            {
                ControllDrone(drones[i]);
            }
        }
    }

    private void ControllDrone(DroneModel droneModel)
    {
        switch (droneModel.State)
        {
            case DroneStateType.AwaitTarget:
                SetTargetResource(droneModel);
                break;
            case DroneStateType.GoToTarget:
                TryLockResource(droneModel);

                if (IsResetingLockedResource(droneModel))
                    break;

                TryStartCollecting(droneModel);
                break;
            case DroneStateType.CollectTarget:
                CollectResource(droneModel);
                break;
            case DroneStateType.Return:
                TryHandOver(droneModel);
                break;
            case DroneStateType.HandOver:
            case DroneStateType.None:
            default:
                break;
        }
    }

    private void SetTargetResource(DroneModel droneModel)
    {
        if (!_finder.IsHaveFreeResources())
            return;

        var tuple = _finder.GetNearestFreeResource(droneModel.View.Transform.position, droneModel.Fraction);

        if (tuple.resource == null)
            return;

        if (tuple.resetingDrone != null)
        {
            tuple.resetingDrone.ResetTarget();
            tuple.resetingDrone.SetAwaitState();
        }

        droneModel.View.Agent.SetDestination(tuple.resource.Cell.Position);
        droneModel.SetGoToTargetState(tuple.resource);
    }

    private void TryLockResource(DroneModel droneModel)
    {
        var sqrDistance = (droneModel.TargetResource.Transform.position - droneModel.View.Transform.position).sqrMagnitude;

        if (sqrDistance < _sqrLockDistance)
        {
            if (!droneModel.TargetResource.IsCollecting)
            {
                droneModel.TargetResource.Lock(droneModel.View.GetInstanceID());
            }
        }
    }

    private bool IsResetingLockedResource(DroneModel droneModel)
    {
        if (droneModel.TargetResource.IsCollecting &&
            droneModel.TargetResource.LockingDroneID != droneModel.View.GetInstanceID())
        {
            droneModel.ResetTarget();
            droneModel.SetAwaitState();
            return true;
        }

        return false;
    }

    private void TryStartCollecting(DroneModel droneModel)
    {
        var sqrDistance = (droneModel.TargetResource.Transform.position - droneModel.View.Transform.position).sqrMagnitude;
        var sqrStopingDistance = droneModel.View.Agent.stoppingDistance * droneModel.View.Agent.stoppingDistance;

        if (sqrDistance <= sqrStopingDistance)
        {
            droneModel.SetCollectingState();
        }
    }

    private void CollectResource(DroneModel droneModel)
    {
        droneModel.CollectingTime += Time.deltaTime;

        if(droneModel.CollectingTime >= _gameConfig.CollectResourceTime)
        {
            droneModel.SetReturnState();
            droneModel.View.Agent.SetDestination(droneModel.Fraction.FractonBase.ResourceDeliveryTransform.position);
        }
    }

    private void TryHandOver(DroneModel droneModel)
    {
        var distance = droneModel.View.Agent.remainingDistance;

        if(distance <= droneModel.View.Agent.stoppingDistance)
        {
            droneModel.SetHandOverState();

            var currentScale = droneModel.View.Transform.localScale;
            var sequence = DOTween.Sequence().
                Append(droneModel.View.Transform.DOScale(currentScale * 1.4f, 0.1f)).
                Append(droneModel.View.Transform.DOScale(currentScale, 0.1f)).
                OnComplete(droneModel.SetAwaitState);
        }
    }

    public void Dispose()
    {
        _uiNotifier.OnDroneSpeedChange -= ChangeDronesSpeed;
        _uiNotifier.OnDronesCountChange -= ChangeDronesCount;
    }
}

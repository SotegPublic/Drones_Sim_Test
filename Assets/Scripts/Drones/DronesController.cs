﻿using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

public class DronesController : IUpdatableController, IInitableController, IDisposable
{
    private IChangableDronesHolder _dronesHolder;
    private IDroneSpawner _droneSpawner;
    private IFreeResourceFinder _finder;
    private IMainUINotifier _uiNotifier;
    private GameConfig _gameConfig;
    private DronesMoveConfig _moveConfig;

    private bool _isActive;
    private float _sqrLockDistance;
    private float _droneSpeed;
    private int _dronesCount;
    private float _sqrCollectingDistance;

    public DronesController(IChangableDronesHolder dronesHolder, GameConfig gameConfig, IFreeResourceFinder finder, IMainUINotifier uiNotifier,
        IDroneSpawner droneSpawner, DronesMoveConfig moveConfig)
    {
        _dronesHolder = dronesHolder;
        _gameConfig = gameConfig;
        _finder = finder;
        _uiNotifier = uiNotifier;
        _droneSpawner = droneSpawner;
        _moveConfig = moveConfig;

        _droneSpeed = gameConfig.StartDronesSpeed;
        _dronesCount = gameConfig.StartDronesCount;

        _uiNotifier.OnDroneSpeedChange += ChangeDronesSpeed;
        _uiNotifier.OnDronesCountChange += ChangeDronesCount;
    }

    private void ChangeDronesCount(int newCont)
    {
        if(newCont == _dronesCount) return;

        _dronesCount = newCont;
    }

    public void Init()
    {
        _isActive = true;
        _sqrLockDistance = _moveConfig.LockResourceDistance * _moveConfig.LockResourceDistance;
        _sqrCollectingDistance = _moveConfig.CollectingDistance * _moveConfig.CollectingDistance;
    }

    public void Update()
    {
        if (!_isActive)
            return;

        foreach (var drones in _dronesHolder.Drones.Values)
        {
            if(drones.Count < _dronesCount)
            {
                SpawnNewDrones(_dronesCount - drones.Count);
            }
            
            for(int i = drones.Count - 1; i >= 0; i--)
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
                if (IsNeedToRemove(droneModel.Fraction))
                {
                    RemoveDrone(droneModel);
                    break;
                }

                SetTargetResource(droneModel);
                break;
            case DroneStateType.GoToTarget:
                CheckLastSpawnedResource(droneModel);

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

    private bool IsNeedToRemove(Fraction fraction)
    {
        var fractionDronesCount = _dronesHolder.Drones[fraction].Count;

        return _dronesCount < fractionDronesCount;
    }

    private void RemoveDrone(DroneModel droneModel)
    {
        var view = droneModel.View;

        _dronesHolder.RemoveDrone(droneModel);
        droneModel.Clear();

        _droneSpawner.DespawnDrone(view);
    }

    private void SpawnNewDrones(int count)
    {
        foreach (var fraction in _dronesHolder.Drones.Keys)
        {
            for (int i = 0; i < count; i++)
            {
                _droneSpawner.SpawnDrone(fraction, _droneSpeed).Forget();
            }
        }
    }

    private void ChangeDronesSpeed(float speed)
    {
        _droneSpeed = speed;

        foreach (var drones in _dronesHolder.Drones.Values)
        {
            for (int i = 0; i < drones.Count; i++)
            {
                drones[i].View.Agent.speed = speed;
            }
        }
    }

    private void SetTargetResource(DroneModel droneModel)
    {
        if (!_finder.IsHaveFreeResources())
            return;

        var tuple = _finder.GetNearestFreeResource(droneModel, droneModel.Fraction);

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

    private void CheckLastSpawnedResource(DroneModel droneModel)
    {
        var tuple = _finder.ChekLastSpawnedResource(droneModel, droneModel.TargetResource, droneModel.Fraction);

        if (!tuple.isNeedChangeTarget)
            return;

        if (tuple.resetingDrone != null)
        {
            tuple.resetingDrone.ResetTarget();
            tuple.resetingDrone.SetAwaitState();
        }

        droneModel.View.Agent.velocity = Vector3.zero;
        droneModel.View.Agent.SetDestination(tuple.resource.Cell.Position);
        droneModel.ChangeTarget(tuple.resource);
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

        if (sqrDistance <= _sqrCollectingDistance)
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

        if(distance <= _moveConfig.HandOverDistance)
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

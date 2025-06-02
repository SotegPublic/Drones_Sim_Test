using DG.Tweening;
using System;
using UnityEngine;

public class DroneModel
{
    private Fraction _fraction;
    private ResourceView _targetRecource;
    private DroneView _view;
    private DroneStateType _currentState;

    public DroneModel(DroneView droneView)
    {
        _view = droneView;
        _currentState = DroneStateType.AwaitTarget;
    }

    public float CollectingTime;

    public Fraction Fraction => _fraction;
    public ResourceView TargetResource => _targetRecource;
    public DroneView View => _view;
    public DroneStateType State => _currentState;

    public void SetAwaitState()
    {
        _currentState = DroneStateType.AwaitTarget;
        _view.Agent.SetDestination(_fraction.FractonBase.SpawnTransform.position);
    }

    public void SetGoToTargetState(ResourceView targetResource)
    {
        _currentState = DroneStateType.GoToTarget;
        SetTarget(targetResource);
    }

    public void SetCollectingState()
    {
        _currentState = DroneStateType.CollectTarget;
        CollectingTime = 0f;
    }

    public void SetReturnState()
    {
        _currentState = DroneStateType.Return;
        _targetRecource.Collected();
        _targetRecource = null;
    }

    public void SetFraction(Fraction fraction)
    {
        _fraction = fraction;
        _view.SetMaterial(fraction.FractionMaterial);
    }

    public void HandOverResource()
    {
        _fraction.GetResource();
        _currentState = DroneStateType.None;

        var currentScale = _view.Transform.localScale;
        var sequence = DOTween.Sequence().
            Append(_view.Transform.DOScale(currentScale * 1.4f, 0.1f)).
            Append(_view.Transform.DOScale(currentScale, 0.1f)).
            OnComplete(GoToSpawnPoint);
    }

    private void GoToSpawnPoint()
    {
        SetAwaitState();
    }

    public void Clear()
    {
        _view.ResetMaterial();
        _view.Agent.ResetPath();
        _view.Agent.isStopped = true;
        _view.Agent.velocity = Vector3.zero;
        _view.Agent.enabled = false;
        _fraction = null;
        _view = null;
        _currentState = DroneStateType.None;
    }

    public void ResetTarget()
    {
        _targetRecource = null;
        _view.Agent.ResetPath();
        _view.Agent.velocity = Vector3.zero;
    }

    private void SetTarget(ResourceView targetResource)
    {
        _targetRecource = targetResource;
    }
}

public enum DroneStateType
{
    None = 0,
    AwaitTarget = 1,
    GoToTarget = 2,
    CollectTarget = 3,
    Return = 4
}

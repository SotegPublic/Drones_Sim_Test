using DG.Tweening;
using System;
using UnityEngine;

public class DroneModel
{
    private Fraction _fraction;
    private ResourceView _targetRecource;
    private DroneView _view;
    private DroneStateType _currentState;

    public DroneModel(DroneView droneView, Fraction fraction)
    {
        _view = droneView;
        _currentState = DroneStateType.AwaitTarget;
        _fraction = fraction;
        _view.SetMaterial(fraction.FractionMaterial, fraction.FractionPathMaterial);
    }

    public float CollectingTime;

    public Fraction Fraction => _fraction;
    public ResourceView TargetResource => _targetRecource;
    public DroneView View => _view;
    public DroneStateType State => _currentState;
    public int InstanceID => _view.GetInstanceID();

    private void SetTarget(ResourceView targetResource)
    {
        _targetRecource = targetResource;
    }

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

    public void SetHandOverState()
    {
        _currentState = DroneStateType.HandOver;
        _fraction.GetResource();
    }

    public void ChangeTarget(ResourceView targetResource)
    {
        _targetRecource?.Unlock(_view.GetInstanceID());
        _targetRecource = targetResource;
    }

    public void ResetTarget()
    {
        _targetRecource?.Unlock(_view.GetInstanceID());
        _targetRecource = null;
        _view.Agent.ResetPath();
        _view.Agent.velocity = Vector3.zero;
    }

    public void Clear()
    {
        _targetRecource?.Unlock(_view.GetInstanceID());
        _targetRecource = null;
        _fraction = null;
        _view.Clear();
        _view = null;
        _currentState = DroneStateType.None;
    }
}

public enum DroneStateType
{
    None = 0,
    AwaitTarget = 1,
    GoToTarget = 2,
    CollectTarget = 3,
    Return = 4,
    HandOver = 5
}

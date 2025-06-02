using UnityEngine;

public class DroneModel
{
    private Fraction _fraction;
    private ResourceView _targetRecource;
    private DroneView _view;

    public DroneView View => _view;

    public DroneModel(DroneView droneView)
    {
        _view = droneView;
    }

    public Fraction Fraction => _fraction;
    public ResourceView TargetResource => _targetRecource;

    public void SetFraction(Fraction fraction)
    {
        _fraction = fraction;
        _view.SetMaterial(fraction.FractionMaterial);
    }

    public void ResetTarget()
    {
        _targetRecource = null;
        _view.Agent.ResetPath();
        _view.Agent.isStopped = true;
        _view.Agent.velocity = Vector3.zero;
    }

    public void SetTarget(ResourceView targetResource)
    {
        _targetRecource = targetResource;
        _view.Agent.isStopped = false;
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
    }
}

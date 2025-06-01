using System.Buffers;
using System.Collections.Generic;

public class DronesController : IUpdatableController, IInitableController
{
    private IDronesHolder _dronesHolder;
    private IFreeResourceFinder _finder;

    private bool _isActive;
    private int _maxDronesCount;

    public DronesController(IDronesHolder dronesHolder, GameConfig gameConfig, IFreeResourceFinder finder)
    {
        _dronesHolder = dronesHolder;
        _maxDronesCount = gameConfig.MaxDronesCount;
        _finder = finder;
    }

    public void Init()
    {
        _isActive = true;
    }

    public void Update()
    {
        if (!_isActive)
            return;

        foreach (var drones in _dronesHolder.Drones)
        {
            SetTargetResources(drones.Value);
            LockResources(drones.Key, drones.Value);
            ResetLockedResources(drones.Key, drones.Value);
        }
    }

    private void SetTargetResources(List<DroneView> dronesList)
    {
        if (!_finder.IsHaveFreeResources())
            return;

        for (int i = 0; i < dronesList.Count; i++)
        {
            if (dronesList[i].TargetResource != null)
                continue;

            var tuple = _finder.GetNearestFreeResource(dronesList[i].Transform.position, dronesList[i].Fraction);

            if (tuple.resource == null)
                continue;

            if (tuple.resetingDrone != null)
                tuple.resetingDrone.ResetTarget();


            dronesList[i].Agent.SetDestination(tuple.resource.Cell.Position);
            dronesList[i].SetTarget(tuple.resource);
        }
    }

    private void LockResources(Fraction fraction, List<DroneView> dronesList)
    {
        var dronesWithTarget = ArrayPool<DroneView>.Shared.Rent(dronesList.Count);
        var count = _dronesHolder.GetDronesWithTarget(fraction, ref dronesWithTarget);

        for (int i = 0; i < count; i++)
        {
            var distance = (dronesWithTarget[i].TargetResource.Transform.position - dronesWithTarget[i].Transform.position).magnitude;

            if (distance < 1f)
            {
                if (!dronesWithTarget[i].TargetResource.IsCollectig)
                {
                    dronesWithTarget[i].TargetResource.Lock(dronesWithTarget[i].GetInstanceID());
                }
            }
        }

        ArrayPool<DroneView>.Shared.Return(dronesWithTarget);
    }

    private void ResetLockedResources(Fraction fraction, List<DroneView> dronesList)
    {
        var dronesWithTarget = ArrayPool<DroneView>.Shared.Rent(dronesList.Count);
        var count = _dronesHolder.GetDronesWithTarget(fraction, ref dronesWithTarget);

        for (int i = 0; i < count; i++)
        {
            if (dronesWithTarget[i].TargetResource.IsCollectig &&
                dronesWithTarget[i].TargetResource.LockingDroneID != dronesWithTarget[i].GetInstanceID())
            {
                dronesWithTarget[i].ResetTarget();
            }
        }

        ArrayPool<DroneView>.Shared.Return(dronesWithTarget);
    }
}

using System.Collections.Generic;

public class DronesController : IUpdatableController, IInitableController
{
    private IDronesHolder _dronesHolder;
    private IFreeResourceFinder _finder;
    private GameConfig _gameConfig;

    private bool _isActive;
    private float _sqrLockDistance;

    public DronesController(IDronesHolder dronesHolder, GameConfig gameConfig, IFreeResourceFinder finder)
    {
        _dronesHolder = dronesHolder;
        _gameConfig = gameConfig;
        _finder = finder;
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
            SetTargetResources(drones);
            LockResources(drones);
            ResetLockedResources(drones);
        }
    }

    private void SetTargetResources(List<DroneModel> dronesList)
    {
        if (!_finder.IsHaveFreeResources())
            return;

        for (int i = 0; i < dronesList.Count; i++)
        {
            if (dronesList[i].TargetResource != null)
                continue;

            var tuple = _finder.GetNearestFreeResource(dronesList[i].View.Transform.position, dronesList[i].Fraction);

            if (tuple.resource == null)
                continue;

            if (tuple.resetingDrone != null)
                tuple.resetingDrone.ResetTarget();

            dronesList[i].View.Agent.SetDestination(tuple.resource.Cell.Position);
            dronesList[i].SetTarget(tuple.resource);
        }
    }

    private void LockResources(List<DroneModel> dronesList)
    {
        for (int i = 0; i < dronesList.Count; i++)
        {
            if (dronesList[i].TargetResource == null)
                continue;

            var sqrDistance = (dronesList[i].TargetResource.Transform.position - dronesList[i].View.Transform.position).sqrMagnitude;

            if (sqrDistance < _sqrLockDistance)
            {
                if (!dronesList[i].TargetResource.IsCollectig)
                {
                    dronesList[i].TargetResource.Lock(dronesList[i].View.GetInstanceID());
                }
            }
        }
    }

    private void ResetLockedResources(List<DroneModel> dronesList)
    {
        for (int i = 0; i < dronesList.Count; i++)
        {
            if (dronesList[i].TargetResource == null)
                continue;

            if (dronesList[i].TargetResource.IsCollectig &&
                dronesList[i].TargetResource.LockingDroneID != dronesList[i].View.GetInstanceID())
            {
                dronesList[i].ResetTarget();
            }
        }
    }
}

using System.Collections.Generic;

public class FreeResourceFinder : IFreeResourceFinder
{
    private IDronesHolder _dronesHolder;
    private IResourcesHolder _resourcesHolder;

    public FreeResourceFinder(IDronesHolder dronesHolder, IResourcesHolder resourcesHolder)
    {
        _dronesHolder = dronesHolder;
        _resourcesHolder = resourcesHolder;
    }

    public bool IsHaveFreeResources()
    {
        for(int i = 0; i < _resourcesHolder.Resources.Count; i++)
        {
            if (!_resourcesHolder.Resources[i].IsCollecting)
                return true;
        }

        return false;
    }

    public (ResourceView resource, DroneModel resetingDrone) GetNearestFreeResource(DroneModel currentDrone, Fraction fraction)
    {
        var fractionDrones = _dronesHolder.Drones[fraction];

        var minDistance = float.MaxValue;
        DroneModel resetingDrone = null;
        ResourceView resource = null;

        for (int i = 0; i < _resourcesHolder.Resources.Count; i++)
        {
            if (_resourcesHolder.Resources[i].IsCollecting)
                continue;

            var freeResource = _resourcesHolder.Resources[i];
            var distance = (freeResource.Transform.position - currentDrone.View.Transform.position).sqrMagnitude;

            if (distance >= minDistance)
                continue;

            if (TryGetDroneWithSameTarget(freeResource, fractionDrones, currentDrone, out var drone))
            {
                var comparedDistance = (freeResource.Transform.position - drone.View.Transform.position).sqrMagnitude;

                if (distance < comparedDistance)
                {
                    resetingDrone = drone;
                    minDistance = distance;
                    resource = freeResource;
                }
            }
            else
            {
                minDistance = distance;
                resource = freeResource;
                resetingDrone = null;
            }
        }

        return (resource, resetingDrone);
    }

    public (bool isNeedChangeTarget, ResourceView resource, DroneModel resetingDrone) ChekLastSpawnedResource(DroneModel currentDrone, ResourceView currentResource, Fraction fraction)
    {
        var newResource = _resourcesHolder.LastSpawnedResource;

        if(newResource.IsCollecting || newResource == currentResource)
            return (false, null, null);

        var currentSqrMagnitude = (currentResource.Transform.position - currentDrone.View.Transform.position).sqrMagnitude;
        var newSqrMagnitude = (newResource.Transform.position - currentDrone.View.Transform.position).sqrMagnitude;

        if(newSqrMagnitude >= currentSqrMagnitude)
        {
            return (false, null, null);
        }

        var fractionDrones = _dronesHolder.Drones[fraction];
        if (TryGetDroneWithSameTarget(newResource, fractionDrones, currentDrone, out var drone))
        {
            var comparedDistance = (newResource.Transform.position - drone.View.Transform.position).sqrMagnitude;

            if(newSqrMagnitude < comparedDistance)
            {
                return (true, newResource, drone);
            }

            return (false, null, null);
        }

        return (true, newResource, null);
    }

    private bool TryGetDroneWithSameTarget(ResourceView resource, List<DroneModel> drones, DroneModel currentDrone, out DroneModel drone)
    {
        for(int i = 0; i < drones.Count; i++)
        {
            if (drones[i].InstanceID == currentDrone.InstanceID)
                continue;

            if (drones[i].State != DroneStateType.GoToTarget)
                continue;

            if (drones[i].TargetResource == resource)
            {
                drone = drones[i];
                return true;
            }
        }

        drone = null;
        return false;
    }
}

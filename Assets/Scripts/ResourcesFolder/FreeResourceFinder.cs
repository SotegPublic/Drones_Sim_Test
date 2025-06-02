using System.Collections.Generic;
using UnityEngine;

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

    public (ResourceView resource, DroneModel resetingDrone) GetNearestFreeResource(Vector3 startPosition, Fraction fraction)
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
            var distance = (freeResource.Transform.position - startPosition).sqrMagnitude;

            if (distance >= minDistance)
                continue;

            resetingDrone = null;

            if (TryGetDroneWithSameTarget(freeResource, fractionDrones, out var drone))
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
            }
        }

        return (resource, resetingDrone);
    }

    private bool TryGetDroneWithSameTarget(ResourceView resource, List<DroneModel> drones, out DroneModel drone)
    {
        for(int i = 0; i < drones.Count; i++)
        {
            if (drones[i].TargetResource == null)
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

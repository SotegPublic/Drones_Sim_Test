using System.Buffers;
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
            if (!_resourcesHolder.Resources[i].IsCollectig)
                return true;
        }

        return false;
    }

    public (ResourceView resource, DroneView resetingDrone) GetNearestFreeResource(Vector3 startPosition, Fraction fraction)
    {
        var freeResources = ArrayPool<ResourceView>.Shared.Rent(_resourcesHolder.Resources.Count);

        var count = GetFreeResources(ref freeResources);

        var fractionDrones = _dronesHolder.Drones[fraction];

        var minDistance = float.MaxValue;
        DroneView resetingDrone = null;
        ResourceView resource = null;

        for (int i = 0; i < count; i++)
        {
            var freeResource = freeResources[i];
            var distance = (freeResource.Transform.position - startPosition).sqrMagnitude;

            if (distance >= minDistance)
                continue;

            resetingDrone = null;

            if (TryGetDroneWithSameTarget(freeResource, fractionDrones, out var drone))
            {
                var comparedDistance = (freeResource.Transform.position - drone.Transform.position).sqrMagnitude;

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

        ArrayPool<ResourceView>.Shared.Return(freeResources);
        return (resource, resetingDrone);
    }

    private bool TryGetDroneWithSameTarget(ResourceView resource, List<DroneView> drones, out DroneView drone)
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

    private int GetFreeResources(ref ResourceView[] freeResources)
    {
        var count = 0;

        for (int i = 0; i < _resourcesHolder.Resources.Count; i++)
        {
            if (!_resourcesHolder.Resources[i].IsCollectig)
            {
                freeResources[count] = _resourcesHolder.Resources[i];
                count++;
            }
        }

        return count;
    }
}

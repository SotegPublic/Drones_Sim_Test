using UnityEngine;

public interface IFreeResourceFinder
{
    public (ResourceView resource, DroneModel resetingDrone) GetNearestFreeResource(DroneModel currentDrone, Fraction fraction);
    public (bool isNeedChangeTarget, ResourceView resource, DroneModel resetingDrone) ChekLastSpawnedResource(DroneModel currentDrone, ResourceView currentResource, Fraction fraction);
    public bool IsHaveFreeResources();
}

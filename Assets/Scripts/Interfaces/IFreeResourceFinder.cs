using UnityEngine;

public interface IFreeResourceFinder
{
    public (ResourceView resource, DroneModel resetingDrone) GetNearestFreeResource(Vector3 startPosition, Fraction fraction);
    public bool IsHaveFreeResources();
}

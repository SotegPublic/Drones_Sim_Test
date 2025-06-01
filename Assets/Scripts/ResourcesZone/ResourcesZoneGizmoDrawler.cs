using UnityEngine;
using Zenject;

public class ResourcesZoneGizmoDrawler : MonoBehaviour
{
    [SerializeField] private Collider _resourcesZoneCollider;
    [SerializeField] private ResourcesZoneConfig _resourcesZoneConfig;

    private IResourcesGridHolder _resourcesGridHolder;

    [Inject]
    public void Construct(IResourcesGridHolder resourcesGridHolder)
    {
        _resourcesGridHolder = resourcesGridHolder;
    }

    void OnDrawGizmos()
    {
        if (_resourcesZoneCollider == null) return;
        if (_resourcesGridHolder == null) return;
        if (_resourcesGridHolder.GridCells == null) return;

        Gizmos.color = Color.green;
        foreach (var grid in _resourcesGridHolder.GridCells)
        {
            Gizmos.DrawWireCube(grid.Position, Vector3.one * _resourcesZoneConfig.ResourcesGridCellSize * 0.9f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_resourcesZoneCollider.bounds.center, _resourcesZoneCollider.bounds.size);
    }
}

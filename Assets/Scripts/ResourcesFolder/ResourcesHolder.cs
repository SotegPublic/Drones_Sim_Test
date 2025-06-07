using System.Collections.Generic;

public class ResourcesHolder: IResourcesHolder, IChangableResourcesHolder
{
    private List<ResourceView> _resources;
    private ResourceView _lastSpawnedResource;

    public ResourceView LastSpawnedResource => _lastSpawnedResource;
    public IReadOnlyList<ResourceView> Resources => _resources;

    void IChangableResourcesHolder.AddResource(ResourceView view)
    {
        _resources.Add(view);
    }

    void IChangableResourcesHolder.UpdateLastSpawnedResource(ResourceView view)
    {
        _lastSpawnedResource = view;
    }

    void IChangableResourcesHolder.RemoveResource(ResourceView view)
    {
        _resources.Remove(view);
    }

    void IChangableResourcesHolder.Clear()
    {
        _resources.Clear();
    }

    void IChangableResourcesHolder.CreateResourcesList(int maxResourcesCount)
    {
        _resources = new List<ResourceView>(maxResourcesCount);
    }
}

using System.Collections.Generic;

public class ResourcesHolder: IResourcesHolder, IChangableResourcesHolder
{
    private List<ResourceView> _resources;

    public IReadOnlyList<ResourceView> Resources => _resources;

    void IChangableResourcesHolder.AddResource(ResourceView view)
    {
        _resources.Add(view);
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

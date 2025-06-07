using System.Collections.Generic;

public interface IResourcesHolder
{
    public IReadOnlyList<ResourceView> Resources { get; }
    public ResourceView LastSpawnedResource { get; }
}

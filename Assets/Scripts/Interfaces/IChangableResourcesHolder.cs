﻿public interface IChangableResourcesHolder
{
    void AddResource(ResourceView view);
    void UpdateLastSpawnedResource(ResourceView view);
    void RemoveResource(ResourceView view);
    void CreateResourcesList(int maxResourcesCount);
    void Clear();
}
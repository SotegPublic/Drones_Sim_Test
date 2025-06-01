using UnityEngine;

public interface IChangableResourcesGridHolder : IResourcesGridHolder
{
    void CreateCellsList(int maxCellsCount);
    void AddCell(Vector3 position);
}

using UnityEngine;

public interface IChangableResourcesGridHolder
{
    void CreateCellsList(int maxCellsCount);
    void AddCell(Vector3 position);
}

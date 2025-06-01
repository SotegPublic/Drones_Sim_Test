using System.Collections.Generic;
using UnityEngine;

public class ResourcesGridHolder : IResourcesGridHolder, IChangableResourcesGridHolder
{
    private List<GridCell> _gridCells;

    public IReadOnlyList<GridCell> GridCells => _gridCells;

    void IChangableResourcesGridHolder.AddCell(Vector3 position)
    {
        _gridCells.Add(new GridCell(position));
    }

    void IChangableResourcesGridHolder.CreateCellsList(int maxCellsCount)
    {
        _gridCells = new List<GridCell>(maxCellsCount);
    }
}

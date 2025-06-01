using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesGridHolder : IResourcesGridHolder, IChangableResourcesGridHolder
{
    private List<GreedCell> _gridCells;

    public IReadOnlyList<GreedCell> GridCells => _gridCells;

    void IChangableResourcesGridHolder.AddCell(Vector3 position)
    {
        _gridCells.Add(new GreedCell(position));
    }

    void IChangableResourcesGridHolder.CreateCellsList(int maxCellsCount)
    {
        _gridCells = new List<GreedCell>(maxCellsCount);
    }
}

public interface IResourcesGridHolder
{
    public IReadOnlyList<GreedCell> GridCells { get; }
}

public interface IChangableResourcesGridHolder : IResourcesGridHolder
{
    void CreateCellsList(int maxCellsCount);
    void AddCell(Vector3 position);
}

public class GreedCell
{
    public bool IsBusy;
    public readonly Vector3 Position;

    public GreedCell(Vector3 position)
    {
        Position = position;
    }
}
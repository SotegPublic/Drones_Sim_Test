using System.Buffers;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesGridHolder : IResourcesGridHolder, IChangableResourcesGridHolder
{
    private List<GridCell> _gridCells;

    public IReadOnlyList<GridCell> GridCells => _gridCells;

    public bool TryGetFreeRandomCell(out GridCell cell)
    {
        var indexesArray = ArrayPool<int>.Shared.Rent(_gridCells.Count);
        var maxIndex = 0;

        for(int i = 0; i < _gridCells.Count; i++)
        {
            if (!_gridCells[i].IsBusy)
            {
                indexesArray[maxIndex] = i;
                maxIndex++;
            }
        }

        if(maxIndex == 0)
        {
            cell = null;
            return false;
        }

        var cellIndex = indexesArray[Random.Range(0, maxIndex)];
        ArrayPool<int>.Shared.Return(indexesArray);

        cell = _gridCells[cellIndex];
        return true;
    }

    public int GetBusyCellsCount()
    {
        var count = 0;
        for(int i = 0; i < _gridCells.Count; i++)
        {
            if (_gridCells[i].IsBusy)
            {
                count++;
            } 
        }

        return count;
    }


    void IChangableResourcesGridHolder.AddCell(Vector3 position)
    {
        _gridCells.Add(new GridCell(position));
    }

    void IChangableResourcesGridHolder.CreateCellsList(int maxCellsCount)
    {
        _gridCells = new List<GridCell>(maxCellsCount);
    }
}

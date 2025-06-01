using System.Collections.Generic;

public interface IResourcesGridHolder
{
    public IReadOnlyList<GridCell> GridCells { get; }
    public bool TryGetFreeRandomCell(out GridCell cell);
    public int GetBusyCellsCount();
}

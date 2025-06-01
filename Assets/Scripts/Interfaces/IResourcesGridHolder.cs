using System.Collections.Generic;

public interface IResourcesGridHolder
{
    public IReadOnlyList<GridCell> GridCells { get; }
}

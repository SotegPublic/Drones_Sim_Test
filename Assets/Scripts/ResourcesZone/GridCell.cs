using UnityEngine;

public class GridCell
{
    public bool IsBusy;
    public readonly Vector3 Position;

    public GridCell(Vector3 position)
    {
        Position = position;
    }
}

using System;
using UnityEngine;

public class GridCell
{
    public bool IsBusy;
    public ResourceView ResourceView;
    public readonly Vector3 Position;

    public GridCell(Vector3 position)
    {
        Position = position;
    }

    public void Clear()
    {
        IsBusy = false;
        ResourceView = null;
    }
}

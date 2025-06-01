using System;
using UnityEngine;

public class GridCell
{
    public Action<GridCell> OnCollected;

    public bool IsBusy;
    public ResourceView ResourceView;
    public bool IsCollectig;
    public readonly Vector3 Position;

    public GridCell(Vector3 position)
    {
        Position = position;
    }

    public void Clear()
    {
        IsBusy = false;
        ResourceView = null;
        IsCollectig = false;
    }

    public void Collected()
    {
        OnCollected?.Invoke(this);
    }
}

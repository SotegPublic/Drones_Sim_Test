using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourceView : MonoBehaviour, IPoolableObject
{
    public Action<ResourceView> OnCollected;

    [SerializeField] private AssetReferenceGameObject _assetRef;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Transform _transform;

    private GridCell _cell;

    private bool _isCollectig;
    private int _lockingDroneID;

    public AssetReferenceGameObject AssetRef => _assetRef;
    public GameObject GameObject => _gameObject;
    public Transform Transform => _transform;
    public GridCell Cell => _cell;
    public bool IsCollecting => _isCollectig;
    public int LockingDroneID => _lockingDroneID;

    public void SetPlacementCell(GridCell cell) => _cell = cell;
    public void Clear()
    {
        _cell = null;
        _isCollectig = false;
        _lockingDroneID = -1;
    }

    public void Lock(int lockingDroneID)
    {
        _lockingDroneID = lockingDroneID;
        _isCollectig = true;
    }

    public void Unlock(int unlockingDroneID)
    {
        if(unlockingDroneID == _lockingDroneID)
            _isCollectig = false;
    }

    public void Collected()
    {
        OnCollected?.Invoke(this);
    }
}

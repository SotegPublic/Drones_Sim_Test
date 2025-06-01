using UnityEngine;
using UnityEngine.AddressableAssets;

public interface IPoolableObject
{
    public AssetReferenceGameObject AssetRef { get; }
    public GameObject GameObject { get; }
    public Transform Transform { get; }
}

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourceView : MonoBehaviour, IPoolableObject
{
    [SerializeField] private AssetReferenceGameObject _assetRef;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Transform _transform;

    public AssetReferenceGameObject AssetRef => _assetRef;

    public GameObject GameObject => _gameObject;

    public Transform Transform => _transform;
}

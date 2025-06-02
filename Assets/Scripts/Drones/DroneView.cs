using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

public class DroneView : MonoBehaviour, IPoolableObject
{
    [SerializeField] private AssetReferenceGameObject _assetRef;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Transform _transform;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private PathDrawler _pathDrawler;

    public AssetReferenceGameObject AssetRef => _assetRef;
    public GameObject GameObject => _gameObject;
    public Transform Transform => _transform;
    public NavMeshAgent Agent => _agent;

    public void ShowPath(bool isShow)
    {
    }

    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }

    public void ResetMaterial()
    {
        _meshRenderer.material = null;
    }
}

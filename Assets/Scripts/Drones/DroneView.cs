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
    [SerializeField] private LineRenderer _lineRenderer;

    public AssetReferenceGameObject AssetRef => _assetRef;
    public GameObject GameObject => _gameObject;
    public Transform Transform => _transform;
    public NavMeshAgent Agent => _agent;
    public LineRenderer LineRenderer => _lineRenderer;

    public void SetMaterial(Material droneMat, Material pathMat)
    {
        _meshRenderer.material = droneMat;
        _lineRenderer.material = pathMat;
    }

    public void ResetMaterial()
    {
        _meshRenderer.material = null;
        _lineRenderer.material = null;
    }

    public void Clear()
    {
        ResetMaterial();
        _agent.ResetPath();
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _agent.enabled = false;
        _lineRenderer.positionCount = 0;
    }
}

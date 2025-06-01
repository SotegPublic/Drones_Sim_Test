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

    private Fraction _fraction;
    private ResourceView _targetRecource;

    public AssetReferenceGameObject AssetRef => _assetRef;
    public GameObject GameObject => _gameObject;
    public Transform Transform => _transform;
    public NavMeshAgent Agent => _agent;
    public Fraction Fraction => _fraction;
    public ResourceView TargetResource => _targetRecource;

    public void SetFraction(Fraction fraction)
    {
        _fraction = fraction;
        _meshRenderer.material = _fraction.FractionMaterial;
    }

    public void ResetTarget()
    {
        _targetRecource = null;
        _agent.ResetPath();
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
    }

    public void Clear()
    {
        _meshRenderer.material = null;
        _agent.ResetPath();
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _agent.enabled = false;
        _fraction = null;
    }

    public void SetTarget(ResourceView targetResource)
    {
        _targetRecource = targetResource;
        _agent.isStopped = false;
    }
    public void ClearTarget() => _targetRecource = null;
}

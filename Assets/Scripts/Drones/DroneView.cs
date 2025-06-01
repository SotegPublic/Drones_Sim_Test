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

    public AssetReferenceGameObject AssetRef => _assetRef;
    public GameObject GameObject => _gameObject;
    public Transform Transform => _transform;
    public MeshRenderer MeshRenderer => _meshRenderer;
    public NavMeshAgent Agent => _agent;
    public Fraction Fraction => _fraction;

    public void SetFraction(Fraction fraction)
    {
        _fraction = fraction;
        _meshRenderer.material = _fraction.FractionMaterial;
    }

    public void Clear()
    {
        _meshRenderer.material = null;
        _agent.path = null;
        _agent.isStopped = true;
        _agent.enabled = false;
        _fraction = null;
    }
}

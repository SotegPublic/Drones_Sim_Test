using UnityEngine;
using UnityEngine.AddressableAssets;

public class DroneView : MonoBehaviour, IPoolableObject
{
    [SerializeField] private AssetReferenceGameObject _assetRef;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Transform _transform;
    [SerializeField] private MeshRenderer _meshRenderer;

    public AssetReferenceGameObject AssetRef => _assetRef;

    public GameObject GameObject => _gameObject;

    public Transform Transform => _transform;
}

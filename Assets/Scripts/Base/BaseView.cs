using UnityEngine;

public class BaseView : MonoBehaviour
{
    [SerializeField] private Transform _baseTransform;
    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private Transform _resourceDeliveryTransform;

    public Transform BaseTransform => _baseTransform;
    public Transform SpawnTransform => _spawnTransform;
    public Transform ResourceDeliveryTransform => _resourceDeliveryTransform;
}

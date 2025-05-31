using UnityEngine;

public class BaseView : MonoBehaviour
{
    [SerializeField] private Transform _baseTransform;
    [SerializeField] private Transform _spawnTransform;

    public Transform BaseTransform => _baseTransform;
    public Transform SpawnTransform => _spawnTransform;
}

using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameConfig), menuName = "CustomSO/" + nameof(GameConfig), order = 1)]
public class GameConfig : ScriptableObject
{
    [Header("Drones")]
    [SerializeField] private int _startDronesCount;
    [SerializeField] private int _maxDronesCount;
    [SerializeField] private float _startDronesSpeed;
    [SerializeField] private float _maxDronesSpeed;
    [SerializeField] private bool _isDrawPath;

    [Header("Resources")]
    [SerializeField] private float _collectResourceTime;
    [SerializeField] private float _lockResourceDistance;
    [SerializeField] private int _maxResourcesCount;
    [SerializeField] private float _spawnResourcesSpeed;

    public int StartDronesCount => _startDronesCount;
    public int MaxDronesCount => _maxDronesCount;
    public float StartDronesSpeed => _startDronesSpeed;
    public float MaxDronesSpeed => _maxDronesSpeed;
    public float CollectResourceTime => _collectResourceTime;
    public int MaxResourcesCount => _maxResourcesCount;
    public float SpawnResourcesSpeed => _spawnResourcesSpeed;
    public float LockResourceDistance => _lockResourceDistance;
    public bool IsDrawPath => _isDrawPath;
}
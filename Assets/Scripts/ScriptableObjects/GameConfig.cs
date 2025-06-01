using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameConfig), menuName = "CustomSO/" + nameof(GameConfig), order = 1)]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int _startDronesCount;
    [SerializeField] private int _maxDronesCount;
    [SerializeField] private float _startDronesSpeed;
    [SerializeField] private float _collectResourceTime;
    [SerializeField] private int _maxResourcesCount;
    [SerializeField] private float _spawnResourcesFrequency;

    public int StartDronesCount => _startDronesCount;
    public int MaxDronesCount => _maxDronesCount;
    public float StartDronesSpeed => _startDronesSpeed;
    public float CollectResourceTime => _collectResourceTime;
    public int MaxResourcesCount => _maxResourcesCount;
    public float SpawnResourcesFrequency => _spawnResourcesFrequency;
}


using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameConfig), menuName = "CustomSO/" + nameof(GameConfig), order = 1)]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int _startDronesCount;
    [SerializeField] private int _maxDronesCount;
    [SerializeField] private float _startDronesSpeed;
    [SerializeField] private float _collectResourceTime;
    [SerializeField] private float _startResourcesFrequency;

    public int StartDronesCount => _startDronesCount;
    public int MaxDronesCount => _maxDronesCount;
    public float StartDronesSpeed => _startDronesSpeed;
    public float CollectResourceTime => _collectResourceTime;
    public float StartResourcesFrequency => _startResourcesFrequency;
}


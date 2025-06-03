using UnityEngine;

[CreateAssetMenu(fileName = nameof(DronesMoveConfig), menuName = "CustomSO/" + nameof(DronesMoveConfig), order = 5)]
public class DronesMoveConfig : ScriptableObject
{
    [SerializeField] private float _reactForce = 2f;
    [SerializeField] private float _reactDistance = 1f;
    [SerializeField] private float _reactSpeedModifier = 1.2f;
    [SerializeField] private float _handOverDistance = 0.8f;
    [SerializeField] private float _collectingDistance = 0.8f;
    [SerializeField] private float _lockResourceDistance = 1f;

    public float ReactForce => _reactForce;
    public float ReactDistance => _reactDistance;
    public float ReactSpeedModifier => _reactSpeedModifier;
    public float HandOverDistance => _handOverDistance;
    public float CollectingDistance => _collectingDistance;
    public float LockResourceDistance => _lockResourceDistance;
}
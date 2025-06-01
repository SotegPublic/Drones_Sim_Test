using UnityEngine;

[CreateAssetMenu(fileName = nameof(ResourcesZoneConfig), menuName = "CustomSO/" + nameof(ResourcesZoneConfig), order = 2)]
public class ResourcesZoneConfig : ScriptableObject
{
    [SerializeField] private float _resourcesGridCellSize;
    [SerializeField] private int _droneAgentIndex;

    public float ResourcesGridCellSize => _resourcesGridCellSize;
    public int DroneAgentIndex => _droneAgentIndex;
}

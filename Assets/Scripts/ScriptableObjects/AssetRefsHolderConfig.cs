using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = nameof(AssetRefsHolderConfig), menuName = "CustomSO/" + nameof(AssetRefsHolderConfig), order = 3)]
public class AssetRefsHolderConfig : ScriptableObject
{
    [SerializeField] private AssetReferenceGameObject _droneRef;
    [SerializeField] private AssetReferenceGameObject _resourceRef;

    public AssetReferenceGameObject DroneRef => _droneRef;
    public AssetReferenceGameObject ResourceRef => _resourceRef;
}


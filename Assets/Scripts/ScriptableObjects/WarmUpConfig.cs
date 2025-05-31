using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = nameof(WarmUpConfig), menuName = "CustomSO/" + nameof(WarmUpConfig), order = 0)]
public class WarmUpConfig : ScriptableObject
{
    [SerializeField] private List<WarmUpedObjectConfig> objects;

    public List<WarmUpedObjectConfig> Objects => objects;
}

[Serializable]
public class WarmUpedObjectConfig
{
    [SerializeField] private AssetReferenceGameObject _objectRef;
    [SerializeField] private int _warmUpCount;

    public AssetReferenceGameObject ObjectRef => _objectRef;
    public int WarmUpCount => _warmUpCount;
}
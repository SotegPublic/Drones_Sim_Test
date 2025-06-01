using System;
using UnityEngine;
using Zenject;

public class PoolableObjectFactory : IPoolableObjectFactory
{
    private IInstantiator _instantiator;

    public PoolableObjectFactory(IInstantiator instantiator)
    {
        _instantiator = instantiator;
    }

    public IPoolableObject Create(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (!prefab.TryGetComponent<IPoolableObject>(out var component))
            throw new Exception($"game object {prefab.name} dos't have IPoolableObject component");

        return _instantiator.InstantiatePrefab(prefab, position, rotation, parent).GetComponent<IPoolableObject>();
    }
}

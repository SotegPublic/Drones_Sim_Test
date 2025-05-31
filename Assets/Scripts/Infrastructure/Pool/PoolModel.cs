using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PoolModel
{
    private AsyncOperationHandle<GameObject> _handler;
    private List<IPoolableObject> _poolableObjects;

    public AsyncOperationHandle<GameObject> Handler => _handler;
    public List<IPoolableObject> PoolableObjects => _poolableObjects;

    public PoolModel(AsyncOperationHandle<GameObject> handler)
    {
        _handler = handler;
        _poolableObjects = new List<IPoolableObject>(32);
    }
}

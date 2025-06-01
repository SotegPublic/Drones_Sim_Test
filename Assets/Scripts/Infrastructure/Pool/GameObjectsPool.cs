using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class GameObjectsPool : IGameObjectsPool
{
    private IPoolableObjectFactory _poolableObjectFactory;
    private Transform _poolTransform;

    private Dictionary<string, PoolModel> _pools = new Dictionary<string, PoolModel>(4);

    [Inject]
    public GameObjectsPool(IPoolableObjectFactory poolableObjectFactory, Transform transform)
    {
        _poolTransform = transform;
        _poolableObjectFactory = poolableObjectFactory;
    }

    public async UniTask<GameObject> GetObjectFromPool(AssetReferenceGameObject objectReference)
    {
        if (!_pools.ContainsKey(objectReference.AssetGUID))
            CreatePool(objectReference);

        if (_pools[objectReference.AssetGUID].PoolableObjects.Count == 0)
        {
            var poolableObject = await GetPoolableObject(_pools[objectReference.AssetGUID].Handler, _poolTransform.position);
            return poolableObject.GameObject;
        }
        else
        {
            var poolableObject = _pools[objectReference.AssetGUID].PoolableObjects[0];
            _pools[objectReference.AssetGUID].PoolableObjects.RemoveAt(0);

            return poolableObject.GameObject;
        }
    }

    private void CreatePool(AssetReferenceGameObject objectReference)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(objectReference);
        _pools.Add(objectReference.AssetGUID, new PoolModel(handle));
    }

    private async UniTask<IPoolableObject> GetPoolableObject(AsyncOperationHandle<GameObject> handler, Vector3 position)
    {
        if (handler.Status == AsyncOperationStatus.None)
            await handler.Task;

        if (handler.Status != AsyncOperationStatus.Succeeded)
            throw new Exception($"Error on handler load");

        return _poolableObjectFactory.Create(handler.Result, position, Quaternion.identity, null);
    }

    public async UniTask WarmUpObjects(AssetReferenceGameObject objectReference, int count)
    {
        if (count == 0)
            return;

        if (!_pools.ContainsKey(objectReference.AssetGUID))
        {
            CreatePool(objectReference);
        }

        var tasks = System.Buffers.ArrayPool<UniTask<IPoolableObject>>.Shared.Rent(count);

        for (int i = 0; i < count; i++)
        {
            var handler = _pools[objectReference.AssetGUID].Handler;
            tasks[i] = GetPoolableObject(handler, _poolTransform.position);
        }

        var poolableObjects = await UniTask.WhenAll(tasks);

        for (int i = 0; i < count; i++)
        {
            _pools[objectReference.AssetGUID].PoolableObjects.Add(poolableObjects[i]);
        }

        System.Buffers.ArrayPool<UniTask<IPoolableObject>>.Shared.Return(tasks, clearArray: true);
    }

    public void ReturnViewToPool(IPoolableObject poolableObject)
    {
        poolableObject.Transform.rotation = Quaternion.identity;
        poolableObject.Transform.localScale = Vector3.one;
        poolableObject.Transform.position = _poolTransform.position;

        _pools[poolableObject.AssetRef.AssetGUID].PoolableObjects.Add(poolableObject);
    }
}

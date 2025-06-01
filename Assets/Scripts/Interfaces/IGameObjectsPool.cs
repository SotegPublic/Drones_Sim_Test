using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public interface IGameObjectsPool
{
    public UniTask<GameObject> GetObjectFromPool(AssetReferenceGameObject objectReference);
    public UniTask WarmUpObjects(AssetReferenceGameObject objectReference, int count);
    public void ReturnViewToPool(IPoolableObject poolableObject);
}

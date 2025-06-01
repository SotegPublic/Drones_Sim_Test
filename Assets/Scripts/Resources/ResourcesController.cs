using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class ResourcesController : IDisposable, IUpdatableController, IInitableController
{
    private IGameObjectsPool _pool;
    private GameConfig _gameConfig;
    private AssetRefsHolderConfig _assetRefsHolder;
    private IResourcesGridHolder _gridHolder;

    private float _currentTime;

    public ResourcesController(IGameObjectsPool pool, GameConfig gameConfig, IResourcesGridHolder gridHolder, AssetRefsHolderConfig assetRefsHolder)
    {
        _pool = pool;
        _gameConfig = gameConfig;
        _gridHolder = gridHolder;
        _assetRefsHolder = assetRefsHolder;
    }

    public void Init()
    {
        for (int i = 0; i < _gridHolder.GridCells.Count; i++)
        {
            _gridHolder.GridCells[i].OnCollected += ReturnResource;
        }

        TrySpawnResource();
    }

    public void Update()
    {
        if (_gridHolder.GetBusyCellsCount() == _gameConfig.MaxResourcesCount)
            return;

        _currentTime += Time.deltaTime;

        if (_currentTime >= _gameConfig.SpawnResourcesFrequency)
        {
            TrySpawnResource();

            _currentTime = 0;
        }
    }

    private void TrySpawnResource()
    {
        if (_gridHolder.TryGetFreeRandomCell(out var cell))
        {
            SpawnResource(cell).Forget();
        }
    }

    private async UniTask SpawnResource(GridCell cell)
    {
        cell.IsBusy = true;

        var resourceObject = await _pool.GetObjectFromPool(_assetRefsHolder.ResourceRef);
        var resourceView = resourceObject.GetComponent<ResourceView>();

        resourceView.Transform.position = cell.Position;
        cell.ResourceView = resourceView;
    }

    private void ReturnResource(GridCell cell)
    {
        var resourcesView = cell.ResourceView;
        _pool.ReturnViewToPool(resourcesView);
        cell.Clear();
    }

    public void Dispose()
    {
        for (int i = 0; i < _gridHolder.GridCells.Count; i++)
        {
            _gridHolder.GridCells[i].OnCollected -= ReturnResource;
        }
    }
}

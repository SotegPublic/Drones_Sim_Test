using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class ResourcesController : IDisposable, IUpdatableController, IInitableController
{
    private IGameObjectsPool _pool;
    private AssetRefsHolderConfig _assetRefsHolder;
    private IResourcesGridHolder _gridHolder;
    private IChangableResourcesHolder _resourcesHolder;
    private IMainUINotifier _uiNotifier;

    private float _currentTime;
    private int _maxResourcesCount;
    private float _spawnResourcesSpeed;

    public ResourcesController(IGameObjectsPool pool, GameConfig gameConfig, IResourcesGridHolder gridHolder, AssetRefsHolderConfig assetRefsHolder,
        IChangableResourcesHolder resourcesHolder, IMainUINotifier uiNotifier)
    {
        _pool = pool;
        _gridHolder = gridHolder;
        _assetRefsHolder = assetRefsHolder;
        _resourcesHolder = resourcesHolder;
        _uiNotifier = uiNotifier;

        _maxResourcesCount = gameConfig.MaxResourcesCount;
        _spawnResourcesSpeed = gameConfig.SpawnResourcesSpeed;

        _uiNotifier.OnGenerationSpeedChange += ChangeSpawnResourcesFrequency;
    }

    public void Init()
    {
        _resourcesHolder.CreateResourcesList(_maxResourcesCount);
        TrySpawnResource();
    }

    public void ChangeSpawnResourcesFrequency(float spawnResourcesFrequency)
    {
        _spawnResourcesSpeed = spawnResourcesFrequency;
    }

    public void Update()
    {
        if (_gridHolder.GetBusyCellsCount() == _maxResourcesCount)
            return;

        _currentTime += Time.deltaTime;

        if (_currentTime >= (1/_spawnResourcesSpeed))
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
        resourceView.SetPlacementCell(cell);
        cell.ResourceView = resourceView;
        _resourcesHolder.AddResource(resourceView);
        _resourcesHolder.UpdateLastSpawnedResource(resourceView);
        resourceView.OnCollected += ReturnResource;
    }

    private void ReturnResource(ResourceView view)
    {
        view.Cell.Clear();
        view.OnCollected -= ReturnResource;
        view.Clear();
        _pool.ReturnViewToPool(view);
        _resourcesHolder.RemoveResource(view);
    }

    public void Dispose()
    {
        _resourcesHolder.Clear();
        _uiNotifier.OnGenerationSpeedChange -= ChangeSpawnResourcesFrequency;
    }
}

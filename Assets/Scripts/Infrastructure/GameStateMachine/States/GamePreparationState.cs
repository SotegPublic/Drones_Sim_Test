using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class GamePreparationState : BaseState, IInitializable
{
    private WarmUpConfig _warmUpConfig;
    private IGameObjectsPool _gameObjectsPool;

    private ResourcesZoneConfig _resourcesZoneConfig;
    private IChangableResourcesGridHolder _resourcesGridHolder;
    private Collider _resourcesZoneCollider;
    private GameObject[] _obstacles;

    private List<UniTask> _tasks;
    private float _agentDefaultRadius;

    [Inject]
    public GamePreparationState(WarmUpConfig warmUpConfig, IGameObjectsPool gameObjectsPool, IChangableResourcesGridHolder gridHolder,
        ResourcesZoneConfig resourcesZoneConfig, GameObject[] obstacles, Collider resourcesZoneCollider)
    {
        _warmUpConfig = warmUpConfig;
        _gameObjectsPool = gameObjectsPool;
        _resourcesGridHolder = gridHolder;
        _resourcesZoneConfig = resourcesZoneConfig;
        _obstacles = obstacles;
        _resourcesZoneCollider = resourcesZoneCollider;
    }

    public void Initialize()
    {
        CreateTasksList();
    }

    public override void EnterState()
    {
        GenerateResourcesZoneGrid();
        WarmUpAsync().Forget();
    }

    private void GenerateResourcesZoneGrid()
    {
        _agentDefaultRadius = NavMesh.GetSettingsByID(_resourcesZoneConfig.DroneAgentIndex).agentRadius;
        var bounds = _resourcesZoneCollider.bounds;
        var center = bounds.center;

        var cellsHalfX = Mathf.FloorToInt(bounds.extents.x / _resourcesZoneConfig.ResourcesGridCellSize);
        var cellsHalfZ = Mathf.FloorToInt(bounds.extents.z / _resourcesZoneConfig.ResourcesGridCellSize);

        var totalCellsX = cellsHalfX * 2;
        var totalCellsZ = cellsHalfZ * 2;
        var maxCellsCount = totalCellsX * totalCellsZ;
        _resourcesGridHolder.CreateCellsList(maxCellsCount);

        for (int x = cellsHalfX; x >= -cellsHalfX; x--)
        {
            for (int z = cellsHalfZ; z >= -cellsHalfZ; z--)
            {
                Vector3 point = center + new Vector3(
                    x * _resourcesZoneConfig.ResourcesGridCellSize,
                    0,
                    z * _resourcesZoneConfig.ResourcesGridCellSize
                );

                if (IsPointValidSimplified(point))
                {
                    _resourcesGridHolder.AddCell(point);
                }
            }
        }
    }

    bool IsPointValidSimplified(Vector3 greedCenterPoint)
    {
        if (_resourcesZoneCollider.ClosestPoint(greedCenterPoint) != greedCenterPoint)
            return false;

        for (int i = 0; i < _obstacles.Length; i++)
        {
            if(_obstacles[i].TryGetComponent<Collider>(out var collider))
            {
                Vector3 closestPointOnObstacle = collider.ClosestPoint(greedCenterPoint);
                float distance = Vector3.Distance(greedCenterPoint, closestPointOnObstacle);

                if (distance < collider.bounds.size.x + _agentDefaultRadius)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void CreateTasksList()
    {
        var tasksCount = 0;

        for (int i = 0; i < _warmUpConfig.Objects.Count; i++)
        {
            tasksCount += _warmUpConfig.Objects[i].WarmUpCount;
        }
        _tasks = new List<UniTask>(tasksCount);
    }

    private async UniTask WarmUpAsync()
    {
        for (int i = 0; i < _warmUpConfig.Objects.Count; i++)
        {
            var objRef = _warmUpConfig.Objects[i].ObjectRef;
            var count = _warmUpConfig.Objects[i].WarmUpCount;

            _tasks.Add(_gameObjectsPool.WarmUpObjects(objRef, count));
        }

        await UniTask.WhenAll(_tasks);

        EndState();
    }

    public override void ExitState()
    {
        _tasks.Clear();
    }
}

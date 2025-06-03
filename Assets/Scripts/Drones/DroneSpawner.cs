using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class DroneSpawner : IDroneSpawner
{
    private IGameObjectsPool _pool;
    private AssetRefsHolderConfig _refsHolder;
    private IChangableDronesHolder _dronesHolder;

    public DroneSpawner(IGameObjectsPool gameObjectsPool, AssetRefsHolderConfig assetRefsHolderConfig, IChangableDronesHolder dronesHolder)
    {
        _pool = gameObjectsPool;
        _refsHolder = assetRefsHolderConfig;
        _dronesHolder = dronesHolder;
    }

    public async UniTask SpawnDrone(Fraction fraction, float speed)
    {
        var drone = await _pool.GetObjectFromPool(_refsHolder.DroneRef);
        var droneView = drone.GetComponent<DroneView>();

        var droneModel = new DroneModel(droneView, fraction);

        droneModel.View.Agent.speed = speed;

        var position = fraction.FractonBase.SpawnTransform.position;
        droneModel.View.Transform.position = position;

        droneModel.View.Agent.enabled = true;
        droneModel.View.Agent.SetDestination(position + fraction.FractonBase.SpawnTransform.forward); //todo - костыль для срабатывания avoidancePriority, лучше в систему спавна по гриду вокруг базы

        _dronesHolder.AddDrone(droneModel);
    }
    public void DespawnDrone(DroneView drone)
    {
        _pool.ReturnViewToPool(drone);
    }
}

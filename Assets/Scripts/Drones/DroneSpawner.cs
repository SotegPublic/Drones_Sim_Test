using Cysharp.Threading.Tasks;
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

    public async UniTask SpawnDrones(Fraction fraction, int avoidancePriority, float speed)
    {
        var drone = await _pool.GetObjectFromPool(_refsHolder.DroneRef);
        var droneView = drone.GetComponent<DroneView>();

        droneView.SetFraction(fraction);

        droneView.Agent.avoidancePriority = avoidancePriority + 1;
        droneView.Agent.speed = speed;

        var position = fraction.FractonBase.SpawnTransform.position;
        droneView.Transform.position = position;

        droneView.Agent.enabled = true;
        droneView.Agent.SetDestination(position + Vector3.forward); //todo - костыль для срабатывания avoidancePriority, лучше в систему спавна по гриду вокруг базы

        _dronesHolder.AddDrone(droneView);
    }
}

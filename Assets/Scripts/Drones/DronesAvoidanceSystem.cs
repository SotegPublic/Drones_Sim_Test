using UnityEngine;

public class DronesAvoidanceSystem : IUpdatableController
{
    private IDronesHolder _dronesHolder;
    private DronesMoveConfig _moveConfig;

    private float _sqrReactDistance;

    public DronesAvoidanceSystem(IDronesHolder dronesHolder, DronesMoveConfig moveConfig)
    {
        _dronesHolder = dronesHolder;
        _moveConfig = moveConfig;

        _sqrReactDistance = _moveConfig.ReactDistance * _moveConfig.ReactDistance;
    }

    public void Update()
    {
        foreach(var drones in _dronesHolder.Drones.Values)
        {
            for(int i = 0; i < drones.Count; i++)
            {
                if (drones[i].State != DroneStateType.GoToTarget && drones[i].State != DroneStateType.Return)
                    continue;

                CheckNearbyDrones(drones[i]);
            }
        }
    }

    private void CheckNearbyDrones(DroneModel droneModel)
    {
        foreach (var drones in _dronesHolder.Drones.Values)
        {
            for (int i = 0; i < drones.Count; i++)
            {
                if (drones[i].View.GetInstanceID() == droneModel.View.GetInstanceID())
                    continue;

                var dir = drones[i].View.Transform.position - droneModel.View.Transform.position;

                if (dir.sqrMagnitude < _sqrReactDistance)
                {
                    droneModel.View.Agent.velocity = Vector3.Lerp(
                        a: droneModel.View.Agent.desiredVelocity,
                        b: -dir.normalized * droneModel.View.Agent.speed * _moveConfig.ReactSpeedModifier,
                        t: Mathf.Clamp01((_moveConfig.ReactForce - dir.magnitude) / _moveConfig.ReactDistance)
                        );
                }
            }
        }
    }
}

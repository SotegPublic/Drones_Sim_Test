using System;
using System.Collections.Generic;
using Zenject;

public class DronesHolder: IDronesHolder, IChangableDronesHolder
{
    private GameConfig _gameConfig;

    private Dictionary<Fraction, List<DroneView>> _drones = new Dictionary<Fraction, List<DroneView>>(4);

    public IReadOnlyDictionary<Fraction, List<DroneView>> Drones => _drones;

    [Inject]
    public DronesHolder(GameConfig gameConfig)
    {
        _gameConfig = gameConfig;
    }

    public int GetDronesWithTarget(Fraction fraction, ref DroneView[] drones)
    {
        var count = 0;
        var fractionDrones = _drones[fraction];

        for(int i = 0; i < fractionDrones.Count; i++)
        {
            if (fractionDrones[i].TargetResource != null)
            {
                drones[count] = fractionDrones[i];
                count++;
            }
        }

        return count;
    }

    public void AddDrone(DroneView drone)
    {
        var fraction = drone.Fraction;

        if(!_drones.ContainsKey(fraction))
        {
            _drones.Add(fraction, new List<DroneView>(_gameConfig.MaxDronesCount));
        }

        _drones[fraction].Add(drone);
    }

    public void RemoveDrone(DroneView drone)
    {
        var fraction = drone.Fraction;

        if (!_drones.ContainsKey(fraction))
            throw new Exception($"Unknown fraction {fraction.FractonBase.name}");

        _drones[fraction].Remove(drone);

        if (_drones[fraction].Count == 0)
            _drones.Remove(fraction);
    }
}

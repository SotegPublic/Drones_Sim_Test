using System;
using System.Collections.Generic;
using Zenject;

public class DronesHolder: IDronesHolder, IChangableDronesHolder
{
    private GameConfig _gameConfig;

    private Dictionary<Fraction, List<DroneModel>> _drones = new Dictionary<Fraction, List<DroneModel>>(4);

    public IReadOnlyDictionary<Fraction, List<DroneModel>> Drones => _drones;

    [Inject]
    public DronesHolder(GameConfig gameConfig)
    {
        _gameConfig = gameConfig;
    }

    public void AddDrone(DroneModel droneModel)
    {        
        var fraction = droneModel.Fraction;

        if(!_drones.ContainsKey(fraction))
        {
            _drones.Add(fraction, new List<DroneModel>(_gameConfig.MaxDronesCount));
        }

        _drones[fraction].Add(droneModel);
    }

    public void RemoveDrone(DroneModel droneModel)
    {
        var fraction = droneModel.Fraction;

        if (!_drones.ContainsKey(fraction))
            throw new Exception($"Unknown fraction {fraction.FractonBase.name}");

        _drones[fraction].Remove(droneModel);

        if (_drones[fraction].Count == 0)
            _drones.Remove(fraction);
    }
}

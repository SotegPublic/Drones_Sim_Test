using System.Collections.Generic;

public interface IDronesHolder
{
    public IReadOnlyDictionary<Fraction, List<DroneModel>> Drones { get; }
}

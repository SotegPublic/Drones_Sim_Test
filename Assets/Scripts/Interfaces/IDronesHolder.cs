using System.Collections.Generic;

public interface IDronesHolder
{
    public IReadOnlyDictionary<Fraction, List<DroneView>> Drones { get; }
    public int GetDronesWithTarget(Fraction fraction, ref DroneView[] drones);
}

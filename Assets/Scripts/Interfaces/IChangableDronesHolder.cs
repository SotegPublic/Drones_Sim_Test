public interface IChangableDronesHolder : IDronesHolder
{
    public void AddDrone(DroneView drone);
    public void RemoveDrone(DroneView drone);
}

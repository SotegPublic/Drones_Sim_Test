public interface IChangableDronesHolder : IDronesHolder
{
    public void AddDrone(DroneModel drone);
    public void RemoveDrone(DroneModel drone);
}

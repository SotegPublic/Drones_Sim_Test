using Cysharp.Threading.Tasks;

public interface IDroneSpawner
{
    public UniTask SpawnDrone(Fraction fraction, int avoidancePriority, float speed);
    public void DespawnDrone(DroneView drone);
}

using Cysharp.Threading.Tasks;

public interface IDroneSpawner
{
    public UniTask SpawnDrones(Fraction fraction, int avoidancePriority, float speed);
}

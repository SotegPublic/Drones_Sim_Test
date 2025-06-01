using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Zenject;

public class SpawnDronesState : BaseState, IInitializable
{
    private IDroneSpawner _droneSpawner;
    private GameConfig _gameConfig;
    private Fraction[] _fractions;

    private List<UniTask> _tasks;

    [Inject]
    public SpawnDronesState(GameConfig gameConfig, Fraction[] fractions, IDroneSpawner droneSpawner)
    {
        _gameConfig = gameConfig;
        _fractions = fractions;
        _droneSpawner = droneSpawner;
    }

    public void Initialize()
    {
        var tasksCount = _gameConfig.StartDronesCount * _fractions.Length;
        _tasks = new List<UniTask>(tasksCount);
    }

    public override void EnterState()
    {
        SpawnDonsAsync().Forget();
    }

    private async UniTask SpawnDonsAsync()
    {
        for (int i = 0; i < _gameConfig.StartDronesCount; i++)
        {
            for(int j = 0; j < _fractions.Length; j++)
            {
                _tasks.Add(_droneSpawner.SpawnDrones(_fractions[j], i, _gameConfig.StartDronesSpeed));
            }

            await UniTask.Delay(200);
        }


        await UniTask.WhenAll(_tasks);
        EndState();
    }

    public override void ExitState()
    {
        _tasks.Clear();
    }
}

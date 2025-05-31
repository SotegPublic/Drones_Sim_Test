public class SpawnDronesState : BaseState
{
    private GameConfig _gameConfig;
    private Fraction[] _fractions;
    private IGameObjectsPool _pool;

    public SpawnDronesState(GameConfig gameConfig, Fraction[] fractions, IGameObjectsPool pool)
    {
        _gameConfig = gameConfig;
        _fractions = fractions;
        _pool = pool;
    }

    public override void EnterState()
    {
        for(int i = 0; i < _fractions.Length; i++)
        {
            SpawnDrones(_fractions[i]);
        }

        EndState();
    }

    private void SpawnDrones(Fraction fraction)
    {
        
    }

    public override void ExitState()
    {
        
    }
}

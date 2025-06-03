using System;
using System.Collections.Generic;
using Zenject;

public class GameStateMachine : IGameStateMachine, IDisposable
{
    private IChangableGameStateHolder _gameStateHolder;
    private IStateFactory _statesFactory;

    private Dictionary<Type, IGameState> _gameStates = new Dictionary<Type, IGameState>(8);

    [Inject]
    public GameStateMachine(IChangableGameStateHolder stateHolder, IStateFactory gamseStatesFactory)
    {
        _gameStateHolder = stateHolder;
        _statesFactory = gamseStatesFactory;

        _gameStates.Add(typeof(GamePreparationState), _statesFactory.CreateState<GamePreparationState>());
        _gameStates.Add(typeof(SpawnDronesState), _statesFactory.CreateState<SpawnDronesState>());
        _gameStates.Add(typeof(GameInProgressState), _statesFactory.CreateState<GameInProgressState>());
    }

    [Inject]
    public void Init()
    {
        foreach(var state in _gameStates.Values)
        {
            state.OnStateEnd += EndCurrentState;
        }
    }

    public void EndCurrentState(Type currentStateType)
    {
        if (_gameStateHolder.GetCurrentGameState() != currentStateType)
            return;

        _gameStates[currentStateType].ExitState();

        switch (currentStateType)
        {
            case var _ when currentStateType == typeof(GamePreparationState):
                _gameStateHolder.ChangeCurrentGameState(typeof(SpawnDronesState));
                _gameStates[typeof(SpawnDronesState)].EnterState();
                break;
            case var _ when currentStateType == typeof(SpawnDronesState):
                _gameStateHolder.ChangeCurrentGameState(typeof(GameInProgressState));
                _gameStates[typeof(GameInProgressState)].EnterState();
                break;
            default:
                break;
        }
    }

    public void StartGame()
    {
        _gameStateHolder.ChangeCurrentGameState(typeof(GamePreparationState));
        _gameStates[typeof(GamePreparationState)].EnterState();
    }

    public void Update()
    {
        _gameStates[_gameStateHolder.GetCurrentGameState()].Update();
    }

    public void LateUpdate()
    {
        _gameStates[_gameStateHolder.GetCurrentGameState()].LateUpdate();
    }

    public void Dispose()
    {
        foreach (var state in _gameStates.Values)
        {
            state.OnStateEnd -= EndCurrentState;
        }

        _gameStates.Clear();
    }
}

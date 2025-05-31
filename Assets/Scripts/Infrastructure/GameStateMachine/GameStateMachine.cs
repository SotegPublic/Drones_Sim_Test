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

        _gameStates.Add(typeof(WarmUpState), _statesFactory.CreateState<WarmUpState>());
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
            //case var _ when currentStateType == typeof(WarmUpState):
            //    _gameStateHolder.ChangeCurrentGameState(typeof(SpawnFiguresOnFieldState));
            //    _gameStates[typeof(SpawnFiguresOnFieldState)].EnterState();
            //    break;
            default:
                break;
        }
    }

    public void StartGame()
    {
        _gameStateHolder.ChangeCurrentGameState(typeof(WarmUpState));
        _gameStates[typeof(WarmUpState)].EnterState();
    }

    public void Update()
    {
        foreach(var state in _gameStates.Values)
        {
            state.Update();
        }
    }

    public void LateUpdate()
    {
        foreach (var state in _gameStates.Values)
        {
            state.LateUpdate();
        }
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

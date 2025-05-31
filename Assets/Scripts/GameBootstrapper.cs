using UnityEngine;
using Zenject;

public class GameBootstrapper : MonoBehaviour
{
    private IGameStateMachine _gameStateMachine;

    [Inject]
    public void Construct(IGameStateMachine stateMachine)
    {
        _gameStateMachine = stateMachine;
    }

    private void Awake()
    {
        //if (Screen.orientation != ScreenOrientation.LandscapeLeft &&
        //    Screen.orientation != ScreenOrientation.LandscapeRight)
        //{
        //    Screen.orientation = ScreenOrientation.LandscapeLeft;
        //}
    }

    private void Start()
    {
        _gameStateMachine.StartGame();
    }

    private void Update()
    {
        _gameStateMachine?.Update();
    }

    private void LateUpdate()
    {
        _gameStateMachine?.LateUpdate();
    }

    private void OnDestroy()
    {
        _gameStateMachine?.Dispose();
    }
}

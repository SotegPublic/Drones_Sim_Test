using System;
using Zenject;

public class InputHandler : IPlayerInputHandler
{
    private PlayerInput _playerInput;

    public PlayerInput PlayerInput => _playerInput;

    [Inject]
    public void Construct()
    {
        _playerInput = new PlayerInput();
    }
}

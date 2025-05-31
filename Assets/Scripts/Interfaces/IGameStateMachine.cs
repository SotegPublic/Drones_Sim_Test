using System;

public interface IGameStateMachine
{
    public void EndCurrentState(Type currentStateType);
    public void StartGame();

    public void Update();
    public void LateUpdate();
    public void Dispose();
}

using System;

public abstract class BaseState: IGameState
{
    public Action<Type> OnStateEnd { get; set; }

    public abstract void EnterState();

    public abstract void ExitState();

    public virtual void LateUpdate()
    {
    }

    public virtual void Update()
    {
    }

    protected void EndState()
    {
        OnStateEnd?.Invoke(this.GetType());
    }
}

using System.Collections.Generic;

public class GameInProgressState : BaseState
{
    private List<IUpdatableController> _updatableControllers;
    private List<IInitableController> _initableControllers;

    private bool isActive;

    public GameInProgressState(List<IUpdatableController> updatableControllers, List<IInitableController> initableControllers)
    {
        _updatableControllers = updatableControllers;
        _initableControllers = initableControllers;
    }

    public override void EnterState()
    {
        for(int i = 0; i < _initableControllers.Count; i++)
        {
            _initableControllers[i].Init();
        }

        isActive = true;
    }

    public override void ExitState()
    {
    }

    public override void Update()
    {
        base.Update();

        if (!isActive)
            return;

        for(int i = 0; i < _updatableControllers.Count; i++)
        {
            _updatableControllers[i].Update();
        }
    }
}

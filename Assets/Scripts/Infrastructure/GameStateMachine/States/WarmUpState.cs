using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public class WarmUpState : BaseState
{
    private WarmUpConfig _warmUpConfig;
    private IGameObjectsPool _gameObjectsPool;

    private List<UniTask> _tasks;
    
    public WarmUpState(WarmUpConfig warmUpConfig, IGameObjectsPool gameObjectsPool)
    {
        _warmUpConfig = warmUpConfig;
        _gameObjectsPool = gameObjectsPool;
    }

    public override void EnterState()
    {
        CreateTasksList();

        EnterStateAsync().Forget();
    }

    private void CreateTasksList()
    {
        var tasksCount = 0;

        for (int i = 0; i < _warmUpConfig.Objects.Count; i++)
        {
            tasksCount += _warmUpConfig.Objects[i].WarmUpCount;
        }
        _tasks = new List<UniTask>(tasksCount);
    }

    private async UniTask EnterStateAsync()
    {
        for (int i = 0; i < _warmUpConfig.Objects.Count; i++)
        {
            var objRef = _warmUpConfig.Objects[i].ObjectRef;
            var count = _warmUpConfig.Objects[i].WarmUpCount;

            _tasks.Add(_gameObjectsPool.WarmUpObjects(objRef, count));
        }

        await UniTask.WhenAll(_tasks);

        EndState();
    }

    public override void ExitState()
    {
        _tasks.Clear();
    }
}

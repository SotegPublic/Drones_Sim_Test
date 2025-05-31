using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class MainGameInstaller : MonoInstaller
{
    [SerializeField] private GameBootstrapper _bootstrapper;
    [SerializeField] private Transform _poolTransform;

    public override void InstallBindings()
    {
        Container.Bind<GameBootstrapper>().FromInstance(_bootstrapper).AsSingle();

        Container.Bind<IPoolableObjectFactory>().To<PoolableObjectFactory>().AsSingle();
        Container.Bind<IGameObjectsPool>().To<GameObjectsPool>().AsSingle().WithArguments(_poolTransform);

        Container.Bind<IPlayerInputHandler>().To<InputHandler>().AsSingle();

    }

}

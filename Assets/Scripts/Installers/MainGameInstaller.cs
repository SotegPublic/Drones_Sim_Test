using UnityEngine;
using Zenject;

public class MainGameInstaller : MonoInstaller
{
    [SerializeField] private GameBootstrapper _bootstrapper;
    [SerializeField] private Transform _poolTransform;
    [SerializeField] private ResourcesZoneGizmoDrawler _gizmoDrawler;
    [SerializeField] private AssetRefsHolderConfig _assetRefsHolderConfig;
    [SerializeField] private GameConfig _gameConfig;

    public override void InstallBindings()
    {
        Container.Bind<GameBootstrapper>().FromInstance(_bootstrapper).AsSingle();

        Container.Bind<IPoolableObjectFactory>().To<PoolableObjectFactory>().AsSingle();
        Container.Bind<IGameObjectsPool>().To<GameObjectsPool>().AsSingle().WithArguments(_poolTransform);


        Container.Bind<IDroneSpawner>().To<DroneSpawner>().AsSingle().WithArguments(_assetRefsHolderConfig);
        Container.BindInterfacesTo<DronesHolder>().AsSingle().WithArguments(_gameConfig);

        Container.Bind<IPlayerInputHandler>().To<InputHandler>().AsSingle();

        Container.BindInterfacesTo<ResourcesGridHolder>().AsSingle();
        Container.BindInterfacesTo<ResourcesHolder>().AsSingle();
        Container.Bind<IFreeResourceFinder>().To<FreeResourceFinder>().AsSingle();
        Container.Bind<ResourcesZoneGizmoDrawler>().FromInstance(_gizmoDrawler).AsSingle();

        Container.BindInterfacesTo<ResourcesController>().AsSingle().WithArguments(_assetRefsHolderConfig, _gameConfig);
        Container.BindInterfacesTo<DronesController>().AsSingle().WithArguments(_gameConfig);
    }

}

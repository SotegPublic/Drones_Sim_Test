using UnityEngine;
using Zenject;

public class MainGameInstaller : MonoInstaller
{
    [SerializeField] private GameBootstrapper _bootstrapper;
    [SerializeField] private Transform _poolTransform;
    [SerializeField] private ResourcesZoneGizmoDrawler _gizmoDrawler;
    [SerializeField] private AssetRefsHolderConfig _assetRefsHolderConfig;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private DronesMoveConfig _dronesMoveConfig;
    [SerializeField] private Fraction[] _fractions;

    public override void InstallBindings()
    {
        Container.Bind<GameBootstrapper>().FromInstance(_bootstrapper).AsSingle();
        Container.BindInstance(_gameConfig).AsSingle();
        Container.BindInstance(_assetRefsHolderConfig).AsSingle();
        Container.BindInstance(_dronesMoveConfig).AsSingle();

        Container.Bind<IPoolableObjectFactory>().To<PoolableObjectFactory>().AsSingle();
        Container.Bind<IGameObjectsPool>().To<GameObjectsPool>().AsSingle().WithArguments(_poolTransform);

        Container.Bind<IFractionsHolder>().To<FractionsHolder>().AsSingle().WithArguments(_fractions);

        Container.Bind<IDroneSpawner>().To<DroneSpawner>().AsSingle();
        Container.BindInterfacesTo<DronesHolder>().AsSingle();
        Container.BindInterfacesTo<DrawDronePathController>().AsSingle();
        Container.BindInterfacesTo<DronesAvoidanceSystem>().AsSingle();

        Container.Bind<IPlayerInputHandler>().To<InputHandler>().AsSingle();

        Container.BindInterfacesTo<ResourcesGridHolder>().AsSingle();
        Container.BindInterfacesTo<ResourcesHolder>().AsSingle();
        Container.Bind<IFreeResourceFinder>().To<FreeResourceFinder>().AsSingle();
        Container.Bind<ResourcesZoneGizmoDrawler>().FromInstance(_gizmoDrawler).AsSingle();

        Container.BindInterfacesTo<ResourcesController>().AsSingle();
        Container.BindInterfacesTo<DronesController>().AsSingle();
    }

}

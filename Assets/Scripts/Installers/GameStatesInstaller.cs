using UnityEngine;
using Zenject;

public class GameStatesInstaller : MonoInstaller
{
    [SerializeField] private WarmUpConfig _warmUpConfig;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private Collider _resourcesZoneCollider;
    [SerializeField] private GameObject[] _obstacles;
    [SerializeField] private ResourcesZoneConfig _resourcesZoneConfig;
    [SerializeField] private Fraction[] _fractions;
    public override void InstallBindings()
    {
        Container.Bind<IResolver>().To<DIResolver>().AsSingle().NonLazy();
        Container.Bind<IStateFactory>().To<StateFactory>().AsSingle().NonLazy();

        Container.BindInterfacesTo<CurrentGameStateHolder>().AsSingle().NonLazy();
        Container.BindInterfacesTo<GameStateMachine>().AsSingle().NonLazy();

        //bind states
        Container.BindInterfacesAndSelfTo<GamePreparationState>().AsSingle().
            WithArguments(_warmUpConfig, _resourcesZoneConfig, _resourcesZoneCollider, _obstacles).NonLazy();
        Container.BindInterfacesAndSelfTo<SpawnDronesState>().AsSingle().WithArguments(_fractions, _gameConfig).NonLazy();
    }
}

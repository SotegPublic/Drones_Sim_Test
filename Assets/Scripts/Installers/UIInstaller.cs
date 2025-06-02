using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIInstaller: MonoInstaller
{
    [SerializeField] private MainUIView _mainUIView;

    public override void InstallBindings()
    {
        Container.Bind<MainUIView>().FromInstance(_mainUIView).AsSingle().NonLazy();
        Container.BindInterfacesTo<MainUIController>().AsSingle();
    }
}
using Core.Controllers;
using Core.Systems;
using Core.Systems.DataPersistenceSystem;
using Core.Views;
using UnityEngine;
using Zenject;

namespace Start
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private RootController rootController;
        [SerializeField] private RootCanvas rootCanvas;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private AudioSystem audioSystem;
        [SerializeField] private DataPersistenceManager dataPersistenceManager;

        public override void InstallBindings()
        {
            Container.Bind<ControllerMapper>().AsSingle().NonLazy();
            Container.Bind<IRootController>().To<RootController>().FromInstance(rootController).AsSingle().NonLazy();
            Container.Bind<RootCanvas>().FromInstance(rootCanvas).AsSingle();
            Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();
            Container.Bind<AudioSystem>().FromInstance(audioSystem).AsSingle();
            Container.Bind<DataPersistenceManager>().FromInstance(dataPersistenceManager).AsSingle();
        }
    }
}
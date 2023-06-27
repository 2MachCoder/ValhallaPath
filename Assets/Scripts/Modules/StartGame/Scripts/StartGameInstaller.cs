using UnityEngine;
using Zenject;

namespace Modules.StartGame.Scripts
{
    public class StartGameInstaller : MonoInstaller<StartGameInstaller>
    {
        [SerializeField] private StartGameUIView startGameUIView;

        public override void InstallBindings()
        {
            Container.Bind<StartGameUIView>().FromInstance(startGameUIView).AsSingle();
            Container.Bind<StartGameController>().AsTransient();
        }
    }
}
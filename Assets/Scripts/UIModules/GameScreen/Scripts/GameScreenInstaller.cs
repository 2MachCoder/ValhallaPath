using Core;
using Core.Views;
using UnityEngine;
using Zenject;

namespace UIModules.GameScreen.Scripts
{
    public class GameScreenInstaller: MonoInstaller<GameScreenInstaller>
    {   
        [SerializeField] private GameScreenUIView gameScreenUIViewPrefab;
        [SerializeField] private GameplayModule gameplayModule;
        
        public override void InstallBindings()
        { 
            Container.Bind<GameScreenUIView>().FromComponentInNewPrefab(gameScreenUIViewPrefab)
                .UnderTransform(c => c.Container.Resolve<RootCanvas>().transform).AsTransient();
            Container.Bind<PlayModeController>().AsTransient();
            Container.Bind<GameplayModule>().FromInstance(gameplayModule).AsSingle();
        }
    }
}
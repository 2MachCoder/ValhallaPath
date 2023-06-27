using Core;
using Core.Views;
using UnityEngine;
using Zenject;

namespace Modules.Game.Scripts
{
    public class GameModuleInstaller: MonoInstaller<GameModuleInstaller>
    {   
        [SerializeField] private PlayModeUIView playModeUIViewPrefab;
        [SerializeField] private PlayModeManager playModeManager;
        [SerializeField] private Ring ringPrefab;
        
        public override void InstallBindings()
        { 
            Container.Bind<PlayModeUIView>().FromComponentInNewPrefab(playModeUIViewPrefab)
                .UnderTransform(c => c.Container.Resolve<RootCanvas>().transform).AsTransient();
            Container.Bind<PlayModeController>().AsTransient();
            Container.Bind<PlayModeManager>().FromInstance(playModeManager).AsSingle();
            Container.BindMemoryPool<Ring, Ring.RingPool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(ringPrefab)
                .UnderTransform(playModeManager.ringsSpawn);
        }
    }
}
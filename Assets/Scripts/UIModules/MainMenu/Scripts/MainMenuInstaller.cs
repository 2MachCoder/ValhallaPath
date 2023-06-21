using Core.Views;
using UnityEngine;
using Zenject;

namespace UIModules.MainMenu.Scripts
{
    public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
    {
        [SerializeField] private MainMenuUIView mainMenuUIViewPrefab;
        
        public override void InstallBindings()
        { 
            Container.Bind<MainMenuUIView>().FromComponentInNewPrefab(mainMenuUIViewPrefab)
                .UnderTransform(c => c.Container.Resolve<RootCanvas>().transform).AsTransient();
            Container.Bind<MainMenuController>().AsTransient();
        }
    }
}
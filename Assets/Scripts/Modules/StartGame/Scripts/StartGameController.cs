using Core.Controllers;
using Cysharp.Threading.Tasks;

namespace Modules.StartGame.Scripts
{
    public class StartGameController : IController
    {
        private readonly StartGameUIView _startGameUIView;
        private readonly IRootController _rootController;

        public StartGameController(StartGameUIView startGameUIView, IRootController rootController)
        {
            _startGameUIView = startGameUIView;
            _rootController = rootController;
        }
        
        public async UniTask Run(object param)
        {
            await _startGameUIView.Show();
            await _startGameUIView.progressBarView.Animate(4);
            
            _rootController.RunController(ControllerMap.MainMenu);
        }

        public async UniTask Stop() => await _startGameUIView.Hide();

        public void Dispose() => _startGameUIView.Dispose();
    }
}
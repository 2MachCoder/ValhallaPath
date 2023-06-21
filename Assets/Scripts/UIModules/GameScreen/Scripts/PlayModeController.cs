using System;
using Core;
using Core.Controllers;
using Cysharp.Threading.Tasks;
using Zenject;

namespace UIModules.GameScreen.Scripts
{
    public class PlayModeController : IController
    {
        private readonly IRootController _rootController;
        private readonly GameScreenUIView _gameScreenUIView;
        private readonly GameplayModule _gameplayModule;
        private readonly PlaceholderFactory<Ring> _ringFactory;
        private readonly UniTaskCompletionSource<Action> _completionSource;
        
        public PlayModeController(IRootController rootController, GameScreenUIView gameScreenUIView, 
            GameplayModule gameplayModule, PlaceholderFactory<Ring> ringFactory)
        {
            _rootController = rootController;
            _gameScreenUIView = gameScreenUIView;
            _gameScreenUIView.gameObject.SetActive(false);
            _gameplayModule = gameplayModule;
            _ringFactory = ringFactory;
            _completionSource = new UniTaskCompletionSource<Action>();
        }
        
        public async UniTask Run(object param)
        {
            await _gameScreenUIView.Show();
            SetupEventListeners();
            _gameplayModule.gameObject.SetActive(true);
            _gameplayModule.Initialize(_gameScreenUIView, _ringFactory);
            _gameplayModule.Show();
            _gameplayModule.StartGame();
            var result = await _completionSource.Task;
            result.Invoke();
        }
        
        private void SetupEventListeners()
        {
            _gameScreenUIView.interactButton.onClick.AddListener(() => _gameScreenUIView.interactButton.gameObject.SetActive(false));
            _gameScreenUIView.dropButton.onClick.AddListener(() => _gameScreenUIView.dropButton.gameObject.SetActive(false));
        }

        public async UniTask Stop()
        {
            await _gameScreenUIView.Hide();
            _gameplayModule.Hide();
        }
        
        public void Dispose()
        {
            _gameScreenUIView.Dispose();
        }
    }
}
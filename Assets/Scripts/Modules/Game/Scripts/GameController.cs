using System;
using Core;
using Core.Controllers;
using Cysharp.Threading.Tasks;
using Modules.Game.Scripts.Popups;
using Zenject;

namespace Modules.Game.Scripts
{
    public class PlayModeController : IController
    {
        private readonly IRootController _rootController;
        private readonly PlayModeUIView _playModeUIView;
        private readonly PlayModeManager _playModeManager;
        private readonly PlaceholderFactory<Ring> _ringFactory;
        private readonly UniTaskCompletionSource<Action> _completionSource;
        private LevelSettings[] _levels;
        private LevelSettings _currentLevel;
        
        public PlayModeController(IRootController rootController, PlayModeUIView playModeUIView,
            PlayModeManager playModeManager)
        {
            _rootController = rootController;
            _playModeUIView = playModeUIView;
            _playModeUIView.gameObject.SetActive(false);
            _playModeManager = playModeManager;
            _completionSource = new UniTaskCompletionSource<Action>();
        }
        
        public async UniTask Run(object param)
        {
            _currentLevel = (LevelSettings)param;
            _levels = _playModeManager.levelScriptableObjects;
            SetupEventListeners();
            _playModeUIView.healthBar.gameObject.SetActive(false);
            _playModeUIView.startPopup.levelNumber.text = (Array.IndexOf(_levels,_currentLevel) + 1).ToString();
            _playModeUIView.Show().Forget();
            await _playModeUIView.startPopup.Show();
            var result = await _completionSource.Task;
            result.Invoke();
        }
        
        private void SetupEventListeners()
        {
            _playModeManager.OnLevelCompleted += OnWin;
            _playModeManager.OnBusterCollected += BusterCollected;
            _playModeManager.OnLevelFailed += OnLose;
            _playModeUIView.startPopup.startButton.onClick.AddListener(OnPlayButtonClicked);
            _playModeUIView.startPopup.backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked); 
            _playModeUIView.winPopup.nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
            _playModeUIView.winPopup.backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
            _playModeUIView.winPopup.restartButton.onClick.AddListener(() => OnRestartButtonClicked(_playModeUIView.winPopup));
            _playModeUIView.losePopup.restartButton.onClick.AddListener(() => OnRestartButtonClicked(_playModeUIView.losePopup));
            _playModeUIView.losePopup.backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        }

        private void BusterCollected()
        {
            //Camera shake
        }
        
        private void OnStartLevelButtonClicked()
        {
            //_playModeUIView.scoreCounter.SetActive(true);
            _playModeUIView.healthBar.gameObject.SetActive(true);
            _playModeManager.StartGame();
        }

        private void OnLose() => OnLevelComplete(_playModeUIView.losePopup);
        private void OnWin()
        {
            OnLevelComplete(_playModeUIView.winPopup);
            //_scoreSystem.Score += _score;
        }
        
        private async void OnLevelComplete(LevelResultPopup resultPopup)
        {
            //resultPopup.scoreCountText.text = _score.ToString();
            //_playModeUIView.scoreCount.text = "0";
            //_playModeUIView.scoreCounter.gameObject.SetActive(false);
            _playModeUIView.healthBar.value = 100f;
            _playModeManager.player.IsAlive = true;
            await resultPopup.Show();
        }
        
        #region Popups
        private async void OnPlayButtonClicked()    //StartPopup
        {
            await _playModeUIView.startPopup.Hide();
            _playModeManager.Show();
            _playModeManager.Initialize(_currentLevel);
        }
        
        private async void OnBackToMenuButtonClicked()  //from all popups
        {
            _playModeManager.CleanAndHide();
            await _playModeUIView.Hide();
            _completionSource.TrySetResult(() => _rootController.RunController(ControllerMap.MainMenu));
        }

        private void OnRestartButtonClicked(LevelResultPopup popup) //from both popups 
        {
            _playModeManager.Hide();
            _completionSource.TrySetResult(() => _rootController.RunController(ControllerMap.PlayMode, _currentLevel));
        }

        private void OnNextLevelButtonClicked() //from WinPopup
        {
            _playModeManager.CleanAndHide();
            _completionSource.TrySetResult(() => _rootController.RunController(ControllerMap.PlayMode,
                _levels[DetermineNextLevelIndex()]));
        }
        #endregion
        
        private int DetermineNextLevelIndex()
        {
            var levelIndex = Array.IndexOf(_levels, _currentLevel);
            if (levelIndex == _levels.Length)
                levelIndex = 0;
            return levelIndex;
        }

        public async UniTask Stop() => await _playModeUIView.Hide();
        public void Dispose()
        {
            _playModeUIView.startPopup.startButton.onClick.RemoveListener(OnPlayButtonClicked);
            _playModeUIView.startPopup.backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClicked); 
            _playModeUIView.winPopup.nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
            _playModeUIView.winPopup.backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClicked);
            _playModeUIView.winPopup.restartButton.onClick.RemoveListener(() => OnRestartButtonClicked(_playModeUIView.winPopup));
            _playModeUIView.losePopup.restartButton.onClick.RemoveListener(() => OnRestartButtonClicked(_playModeUIView.losePopup));
            _playModeUIView.losePopup.backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClicked);
            _playModeManager.OnLevelCompleted -= OnWin;
            _playModeManager.OnBusterCollected -= BusterCollected;
            _playModeManager.OnLevelFailed -= OnLose;
            _playModeUIView.Dispose();
        }
    }
}
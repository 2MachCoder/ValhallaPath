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
        private LevelSettings _currentLevelSettings;
        
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
            _levels = _playModeManager.levelSettings;
            _currentLevelSettings = _levels[_playModeManager.LevelIndex];
            SetupEventListeners();
            _playModeManager.Initialize(_currentLevelSettings);
            _playModeUIView.startPopup.levelNumber.text = (Array.IndexOf(_levels,_currentLevelSettings) + 1).ToString();
            _playModeUIView.Show().Forget();
            await _playModeUIView.startPopup.Show();
            var result = await _completionSource.Task;
            result.Invoke();
        }
        
        private void SetupEventListeners()
        {
            _playModeManager.OnPlayerDamaged += PlayerDamaged;
            _playModeManager.OnBusterCollected += BusterCollected;
            _playModeManager.OnLevelCompleted += OnWin;
            _playModeManager.OnLevelFailed += OnLose;
            _playModeUIView.startPopup.startButton.onClick.AddListener(OnStartButtonClicked);
            _playModeUIView.startPopup.backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked); 
            _playModeUIView.winPopup.nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
            _playModeUIView.winPopup.backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
            _playModeUIView.winPopup.restartButton.onClick.AddListener(() => OnRestartButtonClicked(_playModeUIView.winPopup));
            _playModeUIView.losePopup.restartButton.onClick.AddListener(() => OnRestartButtonClicked(_playModeUIView.losePopup));
            _playModeUIView.losePopup.backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        }

        private void PlayerDamaged(int health)
        {
            _playModeUIView.healthBar.value = health;
        }
        private void BusterCollected()
        {
            //Camera shake
        }

        private void OnLose()
        {
            _playModeUIView.healthBar.value = 0f;
            OnLevelCompleted(_playModeUIView.losePopup);
        }

        private void OnWin() => OnLevelCompleted(_playModeUIView.winPopup);

        private async void OnLevelCompleted(LevelResultPopup resultPopup)
        {
            _playModeUIView.healthUI.SetActive(false);
            _playModeManager.GameStarted = false;
            await resultPopup.Show();
        }
        
        #region Popups
        private async void OnStartButtonClicked()    //StartPopup
        {
            await _playModeUIView.startPopup.Hide();
            _playModeUIView.Show().Forget();
            _playModeUIView.healthUI.SetActive(true);
            _playModeUIView.healthBar.value = 100f;
            _playModeManager.GameStarted = true;
        }
        
        private async void OnBackToMenuButtonClicked()  //All popups
        {
            _playModeManager.Hide();
            await _playModeUIView.Hide();
            _completionSource.TrySetResult(() => _rootController.RunController(ControllerMap.MainMenu));
        }

        private async void OnRestartButtonClicked(LevelResultPopup popup) //from both popups 
        {
            await popup.Hide();
            _playModeUIView.healthUI.SetActive(true);
            _playModeUIView.healthBar.value = 100f;
            _playModeUIView.healthUI.SetActive(true);
            _playModeUIView.Show().Forget();
            _playModeManager.Restart();
        }

        private async void OnNextLevelButtonClicked() //from WinPopup
        {
            DetermineNextLevelIndex();
            await _playModeUIView.winPopup.Hide();
            _playModeUIView.healthUI.SetActive(true);
            _playModeUIView.healthBar.value = 100f;
            _playModeUIView.Show().Forget();
            _playModeManager.GenerateLevel(_currentLevelSettings);
            _playModeManager.GameStarted = true;
        }
        #endregion
        
        private int DetermineNextLevelIndex()
        {
            var levelIndex = Array.IndexOf(_levels, _currentLevelSettings);
            if (levelIndex == _levels.Length)
                levelIndex = 0;
            _currentLevelSettings = _levels[levelIndex];
            _playModeManager.LevelIndex = (byte)levelIndex;
            return levelIndex;
        }

        public async UniTask Stop() => await _playModeUIView.Hide();
        public void Dispose()
        {
            _playModeUIView.startPopup.startButton.onClick.RemoveListener(OnStartButtonClicked);
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
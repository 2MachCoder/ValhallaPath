using System;
using Core.Controllers;
using Core.Systems;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.MainMenu.Scripts
{
    public class MainMenuController : IController
    {
        private readonly IRootController _rootController;
        private readonly AudioSystem _audioSystem;
        private readonly MainMenuUIView _mainMenuUIView;
        private readonly UniTaskCompletionSource<Action> _completionSource;

        public MainMenuController(IRootController rootController, MainMenuUIView mainMenuUIView, AudioSystem audioSystem)
        {
            _rootController = rootController;
            _mainMenuUIView = mainMenuUIView;
            _mainMenuUIView.gameObject.SetActive(false);    
            _audioSystem = audioSystem;
            _completionSource = new UniTaskCompletionSource<Action>();
        }
        
        public async UniTask Run(object param)
        {
            SetupEventListeners();
            await _mainMenuUIView.Show();
            _audioSystem.PlayMainMenuMelody();
            var result = await _completionSource.Task;
            result.Invoke();
        }
        
        private void SetupEventListeners()
        {
            _mainMenuUIView.playButton.onClick.AddListener(PlayButtonClicked);
            _mainMenuUIView.quitButton.onClick.AddListener(ExitButtonClicked);
            _mainMenuUIView.settingsButton.onClick.AddListener(ShowSettingsPopup);
            _mainMenuUIView.settingsPopup.closeButton.onClick.AddListener(HideSettingsPopup);
            _mainMenuUIView.settingsPopup.musicSwitch.OnSwitchToggled += OnMusicSwitchToggled;
            _mainMenuUIView.settingsPopup.musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolumeSlider);
            _mainMenuUIView.settingsPopup.soundsSwitch.OnSwitchToggled += OnSoundsSwitchToggled;
            _mainMenuUIView.settingsPopup.soundsVolumeSlider.onValueChanged.AddListener(ChangeSoundsVolumeSlider);
        }
        
        private void PlayButtonClicked() =>
            _completionSource.TrySetResult(() => _rootController.RunController(ControllerMap.PlayMode));

        private void ExitButtonClicked() => Application.Quit();
        
        #region SettingsPopup

        private async void ShowSettingsPopup()
        {
            _mainMenuUIView.settingsPopup.gameObject.SetActive(true);
            _mainMenuUIView.settingsPopup.musicVolumeSlider.value = _audioSystem.MusicVolume;
            _mainMenuUIView.settingsPopup.soundsVolumeSlider.value = _audioSystem.SoundsVolume;
            await _mainMenuUIView.settingsPopup.Show();
        }
        
        private void OnMusicSwitchToggled(bool switchEnabled)
        {
            if (!switchEnabled)
                _mainMenuUIView.settingsPopup.musicVolumeSlider.value = 0;
            else if (_mainMenuUIView.settingsPopup.musicVolumeSlider.value == 0)
                _mainMenuUIView.settingsPopup.musicVolumeSlider.value = .5f;
        }
        
        private void ChangeMusicVolumeSlider(float volume)
        {
            if (volume > 0)
            {
                if (!_mainMenuUIView.settingsPopup.musicSwitch.SwitchEnabled)
                    _mainMenuUIView.settingsPopup.musicSwitch.Toggle();  //Turn switch on
                _audioSystem.SetMusicVolume(volume);
            }
            else
            {
                if (_mainMenuUIView.settingsPopup.musicSwitch.SwitchEnabled)
                    _mainMenuUIView.settingsPopup.musicSwitch.Toggle();  //Turn switch off
                _audioSystem.SetMusicVolume(0);
            }
        }

        private void OnSoundsSwitchToggled(bool switchEnabled)
        {
            if(!switchEnabled)
                _mainMenuUIView.settingsPopup.soundsVolumeSlider.value = 0;
            else if(_mainMenuUIView.settingsPopup.soundsVolumeSlider.value == 0)
                _mainMenuUIView.settingsPopup.soundsVolumeSlider.value = .5f;
        } 
        
        private void ChangeSoundsVolumeSlider(float volume)
        {
            if (volume > 0)
            {
                if (!_mainMenuUIView.settingsPopup.soundsSwitch.SwitchEnabled)
                    _mainMenuUIView.settingsPopup.soundsSwitch.Toggle();  //Turn switch on
                _audioSystem.SetSoundsVolume(volume);
            }
            else
            {
                if (_mainMenuUIView.settingsPopup.soundsSwitch.SwitchEnabled)
                    _mainMenuUIView.settingsPopup.soundsSwitch.Toggle();  //Turn switch off
                _audioSystem.SetSoundsVolume(0);
            }
        }
        
        private async void HideSettingsPopup() => await _mainMenuUIView.settingsPopup.Hide();
        
        #endregion
        
        public async UniTask Stop() => await _mainMenuUIView.Hide();

        public void Dispose()
        {
            _audioSystem.StopMainMenuMelody();
            _mainMenuUIView.playButton.onClick.RemoveListener(PlayButtonClicked);
            _mainMenuUIView.settingsButton.onClick.RemoveListener(ShowSettingsPopup);
            _mainMenuUIView.settingsPopup.musicVolumeSlider.onValueChanged.RemoveListener(ChangeMusicVolumeSlider);
            _mainMenuUIView.settingsPopup.soundsVolumeSlider.onValueChanged.RemoveListener(ChangeSoundsVolumeSlider);
            _mainMenuUIView.settingsPopup.musicSwitch.OnSwitchToggled -= OnMusicSwitchToggled;
            _mainMenuUIView.settingsPopup.soundsSwitch.OnSwitchToggled -= OnSoundsSwitchToggled;
            _mainMenuUIView.settingsPopup.closeButton.onClick.RemoveListener(HideSettingsPopup);
            _mainMenuUIView.Dispose();
        }
    }
}
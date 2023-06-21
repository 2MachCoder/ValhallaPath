using System;
using UltimateCleanGUIPack.Common.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Animator))]
    public class Switch : MonoBehaviour
    {
        [SerializeField] private bool switchEnabled;
        private ButtonSounds _buttonSounds;
        private Button _button;
        private Animator _animator;
        private Image _bgEnabledImage;
        private Image _bgDisabledImage;
        private Image _handleEnabledImage;
        private Image _handleDisabledImage;
        public event Action<bool> OnSwitchToggled;
        public bool SwitchEnabled => switchEnabled;
    
        private void Awake()
        {
            _buttonSounds = GetComponent<ButtonSounds>();
            _button = GetComponent<Button>();   
            _animator = GetComponent<Animator>();
            _bgEnabledImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _bgDisabledImage = transform.GetChild(0).GetChild(1).GetComponent<Image>();
            _handleEnabledImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();
            _handleDisabledImage = transform.GetChild(1).GetChild(1).GetComponent<Image>();
        }
        
        public void Toggle()
        {
            _buttonSounds.PlayPressedSound();
            switchEnabled = !SwitchEnabled;
            OnSwitchToggled?.Invoke(switchEnabled);
            if (SwitchEnabled)
            {
                _bgDisabledImage.gameObject.SetActive(false);
                _bgEnabledImage.gameObject.SetActive(true);
                _handleDisabledImage.gameObject.SetActive(false);
                _handleEnabledImage.gameObject.SetActive(true);
            }
            else
            {
                _bgEnabledImage.gameObject.SetActive(false);
                _bgDisabledImage.gameObject.SetActive(true);
                _handleEnabledImage.gameObject.SetActive(false);
                _handleDisabledImage.gameObject.SetActive(true);
            }
            _animator.SetTrigger(SwitchEnabled ? "Enable" : "Disable");
        }

        private void OnEnable()
        {
            _animator.SetTrigger(SwitchEnabled ? "Enable" : "Disable");
            _button.onClick.AddListener(Toggle);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Toggle);
        }
    }
}
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    [RequireComponent(typeof(ButtonSounds))]
    public class ButtonZoom : Button
    {
        private ButtonSounds _buttonSounds;
        private bool _isPointerDown;
        private Vector3 _defaultScale;

        protected override void Awake()
        {
            _buttonSounds = GetComponent<ButtonSounds>();
            _defaultScale = transform.localScale;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isPointerDown)
            {
                _buttonSounds.PlayRolloverSound();
                transform.DOScale(_defaultScale * 1.02f, 0.1f);
            }
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!_isPointerDown)
            {
                transform.DOScale(_defaultScale, 0.1f);
            }
            base.OnPointerExit(eventData);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            _buttonSounds.PlayPressedSound();
            transform.DOScale(_defaultScale * 0.98f, 0.1f);
            _isPointerDown = true;
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (_isPointerDown)
            {
                transform.DOScale(_defaultScale, 0.1f);
                _isPointerDown = false;
            }
            base.OnPointerUp(eventData);
        }
    }
}
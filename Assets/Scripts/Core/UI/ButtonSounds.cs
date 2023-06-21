using Core.Systems;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class ButtonSounds : MonoBehaviour
    {
        [Inject] private AudioSystem _audioSystem;
        public AudioClip pressedSound;
        public AudioClip rolloverSound;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayPressedSound()
        {
            if (_audioSystem.SoundsVolume>0)
            {
                _audioSource.volume = _audioSystem.SoundsVolume;
                _audioSource.clip = pressedSound;
                _audioSource.Play();
            }
            else
                _audioSource.clip = null;
        }

        public void PlayRolloverSound()
        {
            if (_audioSystem.SoundsVolume>0)
            {
                _audioSource.volume = _audioSystem.SoundsVolume;
                _audioSource.clip = rolloverSound;
                _audioSource.Play();
            }
            else
                _audioSource.clip = null;
        }
    }
}
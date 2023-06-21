using Core.Systems.DataPersistenceSystem;
using UnityEngine;

namespace Core.Systems
{
    [RequireComponent(typeof(AudioListener))]
    [RequireComponent(typeof(AudioSource))]
    public class AudioSystem : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private AudioSource musicAudioSource;
        public float MusicVolume { get; private set; }
        public float SoundsVolume { get; private set; }
        
        public void PlayMainMenuMelody() => musicAudioSource.Play();
        public void StopMainMenuMelody() => musicAudioSource.Stop();

        public void SetMusicVolume(float volume)
        {
            if (volume > 0)
            {
                MusicVolume = volume;
                musicAudioSource.volume = MusicVolume;
            }
            else
            {
                MusicVolume = 0;
                musicAudioSource.volume = MusicVolume;
            }
        }
        
        public void SetSoundsVolume(float volume) => SoundsVolume = volume > 0 ? volume : 0;

        public void SaveData(ref GameData gameData)
        {
            gameData.MusicVolume = MusicVolume;
            gameData.SoundsVolume = SoundsVolume;
        }

        public void LoadData(GameData gameData)
        {
            MusicVolume = gameData.MusicVolume;
            SoundsVolume = gameData.SoundsVolume;
            musicAudioSource.volume = MusicVolume;
            musicAudioSource.Play();
        }
    }
}
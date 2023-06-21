namespace Core.Systems.DataPersistenceSystem
{
    public class GameData
    {
        public float SoundsVolume;
        public float MusicVolume;
        public float CameraSensitivity;

        public GameData()
        {
            SoundsVolume = 0.5f;
            MusicVolume = 0.5f;
            CameraSensitivity = 5;
        }
    }
}
using Core.Systems.DataPersistenceSystem;
using Interfaces;
using UIModules.GameScreen.Scripts;
using UnityEngine;
using Zenject;

namespace Core
{
    public class GameplayModule : MonoBehaviour, IDataPersistence
    {
        [Inject] private Camera _camera;
        [SerializeField] private Player player;
        private GameScreenUIView _gameScreenUIView;
        private IBreakable _currentBroken;

        public float CameraSensitivity { get; set; }

        public void Initialize(GameScreenUIView gameScreenUIView, PlaceholderFactory<Ring> levelViewFactory)
        {
            _gameScreenUIView = gameScreenUIView;
            player = Instantiate(player, transform).GetComponent<Player>();
            //player.Initialize(CameraSensitivity);
        }

        public async void StartGame()
        {
            
        }

        private void GenerateLevel()
        {
            
        }

        public void Show()
        {
            GenerateLevel();
        }

        public void Hide()
        {
            player.Hide();
        }
        
        public void LoadData(GameData gameData)
        {
            CameraSensitivity = gameData.CameraSensitivity;
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.CameraSensitivity = CameraSensitivity;
        }
    }
}

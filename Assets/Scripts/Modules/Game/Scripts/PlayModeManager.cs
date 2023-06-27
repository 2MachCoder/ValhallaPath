using System;
using System.Collections.Generic;
using Core;
using Core.Systems.DataPersistenceSystem;
using UnityEngine;
using Zenject;

namespace Modules.Game.Scripts
{
    public class PlayModeManager : MonoBehaviour, IDataPersistence
    {
        [Inject] private Ring.RingPool _ringPool;
        [SerializeField] private GameObject busterPrefab;
        public LevelSettings[] levelScriptableObjects;
        public Transform ringsSpawn;
        public Player player;
        public byte levelIndex;
        private const float ZOffset = 3.9f;
        private readonly Vector3 _playerSpawnPosition = new(0f,1f,3.5f);
        private LevelSettings _currentLevelSettings;
        private List<Ring> _rings;
        private bool _gameStarted;
        public event Action OnBusterCollected;
        public event Action OnLevelCompleted;
        public event Action OnPlayerDamaged;
        public event Action OnLevelFailed;

        private void Update()
        {
            if (Input.touchCount > 0 && _gameStarted)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                    player.StartMoving();
                else if (touch.phase is TouchPhase.Ended or TouchPhase.Canceled)
                    player.StopMoving();
            }
        }

        
        public void Initialize(LevelSettings levelSettings)
        {
            _currentLevelSettings = levelSettings;
            player.OnDead += OnPlayerLose;
            player.OnDamaged += OnPlayerDamaged;
            player.OnBusterCollected += OnBusterCollected;
            player.OnFinishLine += OnPlayerWin;
            GenerateNewLevel();
        }
        
        public void StartGame()
        {
            player.IsAlive = true;
            _gameStarted = true;
        }
        
        private void GenerateNewLevel()
        {
            player.transform.position = _playerSpawnPosition;

            if (_currentLevelSettings.Rings.Count < 4)
            {
                for (int i = 0; i < _currentLevelSettings.Rings.Count; i++)
                {
                    var currentRing = _ringPool.Spawn(transform.position, _currentLevelSettings.Rings[i]);
                    currentRing.transform.position += new Vector3(0, 0, ZOffset*i);
                }
            }
            Debug.Log("Логика для большого уровня");
        }
        
        private void RestartGenerateLevel()
        {
            GenerateNewLevel();
        }

        private void MovePlayer()
        {
            
        }

        private void OnPlayerWin() => OnLevelCompleted?.Invoke();
        
        private void OnPlayerLose() => OnLevelFailed?.Invoke();

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        public void CleanAndHide()
        {
            foreach (var ring in _rings) _ringPool.Despawn(ring);
            _ringPool.Clear();
            _rings.Clear();
            
            _currentLevelSettings = null;
            Hide();
        }

        public void LoadData(GameData gameData) => levelIndex = gameData.Level;
        public void SaveData(ref GameData gameData) => gameData.Level = levelIndex;
    }
}

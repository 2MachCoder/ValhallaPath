using System;
using System.Collections.Generic;
using Core;
using Core.Systems.DataPersistenceSystem;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Modules.Game.Scripts
{
    public class PlayModeManager : MonoBehaviour, IDataPersistence
    {
        [Inject] private Ring.RingPool _ringPool;
        [SerializeField] private GameObject busterPrefab;
        
        public LevelSettings[] levelSettings;
        public Transform ringsSpawn;
        public Player player;

        private const float ZOffset = 3.9f;
        private readonly Vector3 _playerSpawnPosition = new(0f,1f,3f);
        private float _rotationSpeed = 45f;
        private LevelSettings _currentLevelSettings;
        private List<Ring> _rings = new();
        private bool _gameStarted;
        public event Action<Int32> OnPlayerDamaged;
        public event Action OnBusterCollected;
        public event Action OnLevelCompleted;
        public event Action OnLevelFailed;

        public byte LevelIndex { get; set; }

        private void Update()
        {
            MovePlayer();
        }

        public void Initialize(LevelSettings levelSettings)
        {
            gameObject.SetActive(true);
            _currentLevelSettings = levelSettings;
            player.OnDead += OnPlayerLose;
            player.OnDamaged += OnPlayerDamaged;
            player.OnBusterCollected += OnBusterCollected;
            player.OnFinishLine += OnPlayerWin;
            GenerateLevel(levelSettings);
        }
        
        public void StartGame()
        {
            _gameStarted = true;
        }

        public void EndGame()
        {
            _gameStarted = false;
        }
        
        public void GenerateLevel(LevelSettings levelSettings)
        {
            _currentLevelSettings = levelSettings;
            player.Initialize();
            player.transform.position = _playerSpawnPosition;
            RotateRings();
            
            if (_currentLevelSettings.Rings.Count <= 5)
            {
                for (int i = 0; i < _currentLevelSettings.Rings.Count; i++)
                {
                    var currentRing = _ringPool.Spawn(transform.position, _currentLevelSettings.Rings[i]);
                    currentRing.transform.position += new Vector3(0, 0, ZOffset*i);
                    _rings.Add(currentRing);
                }
            }
            else
            {
                Debug.Log("Логика для большого уровня");
            }
        }
            
        public void Restart()
        {
            GenerateLevel(_currentLevelSettings);
        }

        private void MovePlayer()
        {
            if (Input.touchCount > 0 && _gameStarted && player.IsAlive)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                    player.StartRun();
                else if (touch.phase is TouchPhase.Ended or TouchPhase.Canceled)
                    player.StopRun();
            }
        }
        
        private void RotateRings()
        {
            ringsSpawn.transform.DORotate(new Vector3(0f, 0f, 360f), _rotationSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear); 
        }

        private void OnPlayerWin() => OnLevelCompleted?.Invoke();
        
        private void OnPlayerLose() => OnLevelFailed?.Invoke();
        
        public void CleanAndHide()
        {
            foreach (var ring in _rings) _ringPool.Despawn(ring);
            _ringPool.Clear();
            _rings.Clear();
            
            _currentLevelSettings = null;
            Hide();
        }
        
        public void Hide() => gameObject.SetActive(false);

        public void LoadData(GameData gameData) => LevelIndex = gameData.Level;
        public void SaveData(ref GameData gameData) => gameData.Level = LevelIndex;
    }
}

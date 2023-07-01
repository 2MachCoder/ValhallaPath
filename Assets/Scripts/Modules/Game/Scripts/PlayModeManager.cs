using System;
using System.Collections.Generic;
using Core;
using Core.Systems.DataPersistenceSystem;
using DG.Tweening;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Modules.Game.Scripts
{
    public class PlayModeManager : MonoBehaviour, IDataPersistence
    {
        [Inject] private Ring.RingPool _ringPool;
        [SerializeField] private FinishPlatform finishplatform;
        [SerializeField] private Booster booster;

        public LevelSettings[] levelSettings;
        public Transform ringsSpawn;
        public Player player;
        public bool GameStarted { get; set; }

        private const float ZOffset = 3.9f;
        private const int RingsFieldOfView = 4;
        private readonly Vector3 _playerSpawnPosition = new(0f, 1f, 2f);
        private float _rotationSpeed = 5f;
        private int _ringsCount;
        private LevelSettings _currentLevelSettings;
        private List<Ring> _rings = new();
        public event Action<Int32> OnPlayerDamaged;
        public event Action OnBusterCollected;
        public event Action OnLevelCompleted;
        public event Action OnLevelFailed;

        public byte LevelIndex { get; set; }

        private void Update()
        {
            MovePlayer();
        }

        public void Initialize()
        {
            gameObject.SetActive(true);
            player.OnDead += OnPlayerDied;
            player.OnDamaged += OnPlayerDamaged;
            player.OnBusterCollected += OnBusterCollected;
            player.OnRingPassed += OnPlayerPassRing;
            player.OnFinish += OnPlayerWin;
            InitGenerateLevel();
        }

        public void InitGenerateLevel()
        {
            _currentLevelSettings = levelSettings[LevelIndex];
            player.Initialize();
            player.transform.position = _playerSpawnPosition;
            
            if (!_rings.IsEmpty()) Clean();

            for (_ringsCount = 0; _ringsCount < RingsFieldOfView; _ringsCount++)
            {
                var currentOffset = ZOffset * _ringsCount;
                
                var currentRing = _ringPool.Spawn(transform.position, _currentLevelSettings.Rings[_ringsCount]);
                currentRing.transform.position += new Vector3(0, 0, currentOffset);
                _rings.Add(currentRing);
            }
            
            finishplatform.transform.position = new Vector3(0f, 0f, _currentLevelSettings.Rings.Count * ZOffset);
            ringsSpawn.transform.DORotate(Vector3.zero, 0f);
            RotateRings();
        }   

        private void OnPlayerPassRing(Ring ring)
        {
            if (ring != null)
            {
                _ringPool.Despawn(ring);
                _rings.Remove(ring);
                
                if (_ringsCount < _currentLevelSettings.Rings.Count)
                {
                    var nextRing = _ringPool.Spawn(transform.position, _currentLevelSettings.Rings[_ringsCount]);
                    var currentOffset = ZOffset * _ringsCount;
                    nextRing.transform.position += new Vector3(0f, 0f, currentOffset);
                    _rings.Add(nextRing);
                    _ringsCount++;
                }
            }
        }

        public void Restart()
        {
            GameStarted = true;
            InitGenerateLevel();
        }

        private void MovePlayer()
        {
            if (Input.touchCount <= 0 || !GameStarted || !player.IsAlive) return;
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    player.StartRun();
                    break;
                case TouchPhase.Ended or TouchPhase.Canceled:
                    player.StopRun();
                    break;
            }
        }

        private void RotateRings()
        {
            ringsSpawn.transform.DORotate(new Vector3(0f, 0f, 360f), _rotationSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }

        private void OnPlayerWin()
        {
            GameStarted = false;
            OnLevelCompleted?.Invoke();
        }

        private void OnPlayerDied()
        {
            GameStarted = false;
            OnLevelFailed?.Invoke();
        }

        private void Clean()
        {
            foreach (var ring in _rings) _ringPool.Despawn(ring);
            _rings.Clear();
        } 

        public void Hide()
        {
            player.OnDead -= OnPlayerDied;
            player.OnDamaged -= OnPlayerDamaged;
            player.OnRingPassed -= OnPlayerPassRing;
            player.OnBusterCollected -= OnBusterCollected;
            player.OnFinish -= OnPlayerWin;

            Clean();
            _currentLevelSettings = null;
            gameObject.SetActive(false);
        }
        
        public void LoadData(GameData gameData) => LevelIndex = gameData.Level;
        public void SaveData(ref GameData gameData) => gameData.Level = LevelIndex;
    }
}

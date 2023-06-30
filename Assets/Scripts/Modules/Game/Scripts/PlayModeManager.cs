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
        [SerializeField] private Buster buster;

        public LevelSettings[] levelSettings;
        public Transform ringsSpawn;
        public Player player;
        public bool GameStarted { get; set; }

        private const float ZOffset = 3.9f;
        private readonly Vector3 _playerSpawnPosition = new(0f, 1f, 2f);
        private float _rotationSpeed = 5f;
        private float _finalRingsLenght;
        private int _ringCounter;
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

        public void Initialize(LevelSettings levelSettings)
        {
            gameObject.SetActive(true);
            _currentLevelSettings = levelSettings;
            player.OnDead += OnPlayerDied;
            player.OnDamaged += OnPlayerDamaged;
            player.OnBusterCollected += OnBusterCollected;
            player.OnFinish += OnPlayerWin;
            InitGenerateLevel(levelSettings);
        }

        public void InitGenerateLevel(LevelSettings levelSettings)
        {
            _currentLevelSettings = levelSettings;
            player.Initialize();
            player.transform.position = _playerSpawnPosition;
            _finalRingsLenght = 0f;
            
            if (!_rings.IsEmpty()) Clean();
            finishplatform.gameObject.SetActive(false);

            if (_currentLevelSettings.Rings.Count <= 5)
            {
                for (int i = 0; i < _currentLevelSettings.Rings.Count; i++)
                {
                    var currentRing = _ringPool.Spawn(transform.position, _currentLevelSettings.Rings[i]);
                    var currentOffset = ZOffset * i;
                    currentRing.transform.position += new Vector3(0, 0, currentOffset);
                    _rings.Add(currentRing);
                    _finalRingsLenght += currentOffset;
                }

                finishplatform.gameObject.SetActive(true);
                finishplatform.transform.position = new Vector3(0f, 0f, _finalRingsLenght);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    var currentRing = _ringPool.Spawn(transform.position, _currentLevelSettings.Rings[i]);
                    var currentOffset = ZOffset * i;
                    currentRing.transform.position += new Vector3(0, 0, currentOffset);
                    _rings.Add(currentRing);
                    _finalRingsLenght += currentOffset;
                }
                player.OnRingPassed += OnPlayerPassRing;
            }

            ringsSpawn.transform.DORotate(Vector3.zero, 0f);
            RotateRings();
        }

        private void OnPlayerPassRing(Ring ring)
        {
            _ringPool.Despawn(ring);
        }

        public void Restart()
        {
            GameStarted = true;
            InitGenerateLevel(_currentLevelSettings);
        }

        private void MovePlayer()
        {
            if (Input.touchCount > 0 && GameStarted && player.IsAlive)
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
            _ringPool.Clear();
            _rings.Clear();
        } 

        public void Hide()
        {
            player.OnDead -= OnPlayerDied;
            player.OnDamaged -= OnPlayerDamaged;
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

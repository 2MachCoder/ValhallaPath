using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        [Inject] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private ParticleSystem immortalParticles;
        private const float ShakeIntensity = 1f;
        private const int ShakeTime = 1000; //milliSeconds
        private const int ImmortalTime = 4000; //milliSeconds
        private const float DamageBackwardDistance = 1f;
        private const int WallLayer = 3;
        private const int AnimTransitionDuration = 500; // milliSeconds, * 2

        [Header("Options:")] public float speed = 5f;
        public int health = 100;
        public int wallDamage = 25;
        public int enemyDamage = 15;

        private CinemachineBasicMultiChannelPerlin _cbmcp;
        private Animator _animator;
        private Ring _currentRing;
        private bool _isImmortal;
        private bool _isMoving;
        private bool _ableToTriggerEnter = true;
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int Alive = Animator.StringToHash("Alive");
        private static readonly int IsDamaged = Animator.StringToHash("IsDamaged");

        public event Action OnFinish;
        public event Action OnDead;
        public event Action OnBusterCollected;
        public event Action<Int32> OnDamaged;
        public event Action<Ring> OnRingPassed;

        public bool IsAlive { get; private set; }

        private int Health
        {
            get => health;
            set
            {
                if (value < 0)
                    health = 0;
                else if (value > 100)
                    health = 100;
                else
                    health = value;
            }
        }

        private void Update()
        {
            if (_isMoving && IsAlive) MoveForward();
        }

        private async void OnTriggerEnter(Collider other)
        {
            if (!_ableToTriggerEnter) return;
            _ableToTriggerEnter = false;

            if (other.gameObject.TryGetComponent(out _currentRing)) // Passed the ring
            {
                await _currentRing.Break();
                _ableToTriggerEnter = true;
                OnRingPassed?.Invoke(_currentRing);
            }

            else if (other.gameObject.layer == WallLayer)
            {
                if (!_isImmortal)
                {
                    _isMoving = false;
                    TakeDamage(wallDamage).Forget();
                }
                else
                {
                    _currentRing = other.gameObject.GetComponentInParent<Ring>();
                    _currentRing.GetComponent<BoxCollider>().enabled = false;
                    await _currentRing.Break();
                    _ableToTriggerEnter = true;
                    _currentRing.GetComponent<BoxCollider>().enabled = true;
                    OnRingPassed?.Invoke(_currentRing);
                }
            }

            else if (other.gameObject.GetComponent<Booster>())
            {
                other.GetComponent<Booster>().Collect();
                _ableToTriggerEnter = true;
                BecomeImmortal().Forget();
            }
            
            // else if (other.gameObject.GetComponent<Enemy>())
            // {
            //     var enemy = other.GetComponent<Enemy>();
            //     if (!_isImmortal)
            //     {
            //         enemy.AttackPlayer();
            //         TakeDamage(enemyDamage).Forget();
            //     }
            //     else
            //     {
            //         enemy.Die();
            //     }
            // }
            
            else if (other.gameObject.GetComponent<FinishPlatform>())
            {
                other.GetComponent<FinishPlatform>().LaunchFireworks();
                await UniTask.Delay(500);
                StopRun();
                OnFinish?.Invoke();
            }
        }

        public void Initialize()
        {
            _cbmcp = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _animator = GetComponent<Animator>();
            _animator.SetBool(Alive, true);
            _animator.SetBool(IsRunning, false);
            _animator.SetTrigger(Idle);
            IsAlive = true;
            _isMoving = false;
            health = 100;
            _ableToTriggerEnter = true;
        }

        #region Movement

        public void StartRun()
        {
            _isMoving = true;
            _animator.SetBool(IsRunning, true);
        }

        public void StopRun()
        {
            _isMoving = false;
            _animator.SetBool(IsRunning, false);
        }

        private void MoveForward()
        {
            transform.DOMove(transform.position + transform.forward * (speed * Time.deltaTime), 0);
        }

        #endregion
        
        private async UniTask TakeDamage(int damage)
        {
            Health -= damage;
            ShakeCamera();
            if (health > 0)
            {
                _animator.SetTrigger(IsDamaged);
                OnDamaged?.Invoke(health);
                await transform.DOMoveZ(transform.position.z - DamageBackwardDistance, 0.5f);
                _ableToTriggerEnter = true;
            }
            else
            {
                OnDamaged?.Invoke(health);
                PlayDeath();
            }
        }

        private async UniTask BecomeImmortal()
        {
            _isImmortal = true;
            immortalParticles.Play();
            await UniTask.Delay(ImmortalTime);
            immortalParticles.Stop();
            _isImmortal = false;
        }
        
        private async void PlayDeath()
        {
            IsAlive = false;
            _animator.SetBool(Alive, false);
            await UniTask.Delay(AnimTransitionDuration);
            var animationDelayTime = (int)(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * 1000);
            await UniTask.Delay(animationDelayTime);
            OnDead?.Invoke();
        }
        
        private async void ShakeCamera()
        {
            _cbmcp.m_AmplitudeGain = ShakeIntensity;
            await UniTask.Delay(ShakeTime);
            _cbmcp.m_AmplitudeGain = 0;
        }
    }
}
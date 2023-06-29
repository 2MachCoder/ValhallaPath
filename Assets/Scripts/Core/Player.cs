using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        private const float BackwardDistance = 1f;
        private const int WallLayer = 3;
        
        [Header("Options:")]
        public float speed = 5f;
        public int health = 100;
        public int wallDamage = 25;
        public int enemyDamage = 15;

        private Animator _animator;
        private bool _isMoving;
        private bool _ableToTriggerEnter = true;
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int Alive = Animator.StringToHash("Alive");
        private static readonly int IsDamaged = Animator.StringToHash("IsDamaged");

        public event Action OnFinishLine;
        public event Action OnDead;
        public event Action OnBusterCollected;
        public event Action<Int32> OnDamaged;

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
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(_ableToTriggerEnter);

            if (!_ableToTriggerEnter) return;
            _ableToTriggerEnter = false;

            if (other.gameObject.GetComponent<Ring>())
            {
                other.GetComponent<Ring>().Break();
                _ableToTriggerEnter = true;
            }
            
            else if (other.gameObject.layer == WallLayer) 
                TakeDamage(wallDamage);
            
            else if (other.gameObject.GetComponent<Enemy>())
            {
                other.GetComponent<Enemy>().AttackPlayer();
                TakeDamage(enemyDamage);
            }
        }

        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(Idle);
            IsAlive = true;
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
            Debug.Log(health);
            if (health > 0)
            {
                _animator.SetTrigger(IsDamaged);
                OnDamaged?.Invoke(health);
                await transform.DOMoveZ(transform.position.z - BackwardDistance, 0.5f);
                _ableToTriggerEnter = true;
                Debug.Log("true");
            }
            else
                PlayDeath();
        }

        private async void PlayDeath()
        {
            IsAlive = false;
            _animator.SetBool(Alive, false);
            var animationDelayTime = (int)(_animator.runtimeAnimatorController.animationClips[0].length * 1000);
            await UniTask.Delay(animationDelayTime);
            OnDead?.Invoke();
        }
    }
}
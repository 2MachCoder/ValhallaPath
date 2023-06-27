using System;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private LayerMask wallLayer;
        private Animator _animator;
        public float speed = 5f;
        private bool _isAlive;
        private bool _isMoving;
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        public event Action OnFinishLine;
        public event Action OnDead;
        public event Action OnBusterCollected;
        public event Action OnDamaged;

        public bool IsAlive { get; set; }

        private void Show()
        {
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(Idle);
        }

        private void OnCollisionEnter(Collision collision)
        {   
            if (collision.gameObject.layer == wallLayer)
            {
                
            }
        }
        
        private void Update()
        {
            if (_isMoving) MoveForward();
        }

        public void StartMoving()
        {
            _isMoving = true;
            _animator.SetBool(IsMoving, true); 
        }

        public void StopMoving()
        {
            _isMoving = false;
            _animator.SetBool(IsMoving, false);
        }

        private void MoveForward()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void Hide()
        {
            _isMoving = false;
        }
    }
}
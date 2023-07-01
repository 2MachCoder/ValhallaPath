using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    public class Enemy : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Attack = Animator.StringToHash("Attack");

        public void AttackPlayer() // Turn to player and kick him
        {
            //_animator.SetTrigger(Attack);
        }

        public void Die()
        {
            //_animator.SetTrigger(Dead);
        }
    }
}
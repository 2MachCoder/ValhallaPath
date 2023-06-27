using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    public class Enemy : MonoBehaviour
    {
        private Animator _animator;

        public void AttackPlayer()
        {
            //повернуться к игроку и пиздануть его
        }
    }
}
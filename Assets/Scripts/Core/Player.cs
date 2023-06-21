using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        private Animator _animator;
        private bool _isAlive;
        
        public bool IsAlive { get; private set; }

        public void Initialize(Camera camera)
        {
            
        }

        public void Hide()
        {
            
        }
    }
}
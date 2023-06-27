using UnityEngine;
using Zenject;

namespace Core
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class Ring : MonoBehaviour
    {
        [SerializeField] private GameObject[] walls = new GameObject[8];
        private Animator _animator;
        private AudioSource _audioSource;
        private static readonly int Break = Animator.StringToHash("Break");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Initialize(Vector3 position, RingSettings settings)
        {
            foreach (var wall in walls)
                wall.SetActive(true);
            
            transform.position = position;
            foreach (var index in settings.BreakableWalls)
            {
                walls[index].SetActive(false);
            } 
            _animator.SetBool(Break, false);
        }
        
        public class RingPool : MonoMemoryPool<Vector3, RingSettings, Ring>
        {
            protected override void Reinitialize(Vector3 position, RingSettings settings, Ring ring)
            {
                ring.Initialize(position, settings);
            }
        }
    }
}
using UnityEngine;
using Zenject;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SphereCollider))]
    public class Booster : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        private SphereCollider _collider;
        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
        }

        public void Collect()
        {
            _collider.enabled = false;
            particles.Stop();
        }

        private void Initialize(Transform spawn)
        {
            transform.parent = spawn;
            transform.position = spawn.position;
            _collider.enabled = true;
            particles.Play();
        }
        
        public class BoosterPool : MonoMemoryPool<Transform, Booster>
        {
            protected override void Reinitialize(Transform spawn, Booster booster)
            {
                booster.Initialize(spawn);
            }
        }
    }
}
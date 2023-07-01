using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class Ring : MonoBehaviour
    {
        [Inject] private Booster.BoosterPool _boosterPool;
        //[Inject] private Enemy.EnemyPool _enemyPool;
        [SerializeField] private GameObject[] walls = new GameObject[8];
        private const float ZBoosterOffset = 2f;
        private Animator _animator;
        private AudioSource _audioSource;
        private Booster _booster;
        private Enemy _enemy;
        private static readonly int IsBroken = Animator.StringToHash("IsBroken");

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
            _animator.SetBool(IsBroken, false);

            if (settings.WithBooster)
            {
                _booster = _boosterPool.Spawn(transform);
                _booster.transform.position = walls[settings.BoosterSpawnIndex].transform.position -
                                              new Vector3(0f, 0f, ZBoosterOffset);
            }
        }

        public async UniTask Break()
        {
            _audioSource.Play();
            _animator.SetBool(IsBroken, true);
            var animationDelayTime = (int)(_animator.runtimeAnimatorController.animationClips[0].length * 1000);
            await UniTask.Delay(animationDelayTime);
            foreach (var wall in walls)
                wall.SetActive(false);
            if (_booster != null)
            {   
                _boosterPool.Despawn(_booster);
                _booster = null;
            }
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
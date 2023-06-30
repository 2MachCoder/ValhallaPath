using UnityEngine;

namespace Core
{
    public class Buster : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        public void Collect()
        {
            particles.Play();
        }
    }
}
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Level settings", menuName = "Configs/Level settings")]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField] private Vector3[] doorsPositions;
        [SerializeField] private Vector2[] interactablePositions;
        [SerializeField] private Vector2 playerSpawnPosition;
        
        public Vector3[] DoorsPositions => doorsPositions;
        
        public Vector2[] InteractablePositions => interactablePositions;

        public Vector2 PlayerSpawnPosition => playerSpawnPosition;
    }
}

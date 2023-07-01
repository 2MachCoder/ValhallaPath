using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Ring settings", menuName = "Configs/Ring settings")]
    public class RingSettings : ScriptableObject
    {
        [SerializeField] private int[] breakableWalls;
        [SerializeField] private bool withEnemy;
        [SerializeField] private int enemySpawnIndex;
        [SerializeField] private bool withBooster;
        [SerializeField] private int boosterSpawnIndex;
        
        public int[] BreakableWalls => breakableWalls;
        public bool WithBooster => withBooster;
        public int BoosterSpawnIndex => boosterSpawnIndex;
    }
}

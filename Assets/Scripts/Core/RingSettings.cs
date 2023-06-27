using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Ring settings", menuName = "Configs/Ring settings")]
    public class RingSettings : ScriptableObject
    {
        [SerializeField] private int[] breakableWalls;
        [SerializeField] private Dictionary<int, Enemy> enemies;
        
        public int[] BreakableWalls => breakableWalls;
        
        public Dictionary<int, Enemy> Enemies => enemies;
    }
}

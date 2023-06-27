using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Level settings", menuName = "Configs/Level settings")]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField] private List<RingSettings> rings;
        
        public List<RingSettings> Rings => rings;
    }
}

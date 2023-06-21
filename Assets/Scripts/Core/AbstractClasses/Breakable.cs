using Interfaces;
using UnityEngine;

namespace Core.AbstractClasses
{
    public class Breakable : IBreakable
    {
        public bool IsBroken { get; }
        public void Break()
        {
            
        }

        public void Fix()
        {
            
        }
    }
}
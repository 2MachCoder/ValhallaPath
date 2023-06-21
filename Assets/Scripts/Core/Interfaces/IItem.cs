using UnityEngine;

namespace Core.GameInterfaces
{
    public interface IItem
    {
        void Grab(Transform owner);
        void Throw();
        public Transform Transform { get; }
    }
}
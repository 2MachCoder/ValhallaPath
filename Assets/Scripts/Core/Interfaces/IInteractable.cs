using UnityEngine;

namespace Core.GameInterfaces
{
    public interface IInteractable
    {
        public bool IsWorking { get; }
        public Transform PointToLook { get; }
        void Interact();
    }
}
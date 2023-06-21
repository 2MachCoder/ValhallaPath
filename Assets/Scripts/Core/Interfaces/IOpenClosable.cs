using UnityEngine;

namespace Core.GameInterfaces
{
    public interface IOpenClosable
    {
        bool IsOpen { get; }
        bool IsInteractable { get; }

        public Transform PointToLook { get; }
        void OpenClose();
    }
}
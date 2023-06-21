using Core.GameInterfaces;
using UnityEngine;

namespace Core.AbstractClasses
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        private Animator _animController;
        private bool _isWorking;
        private bool _isBroken;
        private static readonly int Working = Animator.StringToHash("IsWorking");
        private static readonly int Broken = Animator.StringToHash("IsBroken");
        [SerializeField] private Transform pointToLook;
        
        public Transform PointToLook => pointToLook;
        public bool IsWorking => _isWorking;
        public bool IsBroken => _isBroken;
        
        private void Awake()
        {
            _animController = GetComponentInParent<Animator>();
        }

        public void Interact()
        {
            _isWorking = true;
            _animController.SetBool(Working, IsWorking);
        }

        public void Break()
        {
            _isWorking = false;
            _isBroken = true;
            _animController.SetBool(Broken,true);
        }
    }
}
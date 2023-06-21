using Core.GameInterfaces;
using UnityEngine;

namespace Core.AbstractClasses
{
    [RequireComponent(typeof(Animator))]
    public abstract class OpenClosable : MonoBehaviour, IOpenClosable
    {
        [SerializeField] private Transform pointToLook;
        protected Animator AnimController;
        protected bool _isOpen;
        protected bool _isInteractable = true;
        private static readonly int Open = Animator.StringToHash("IsOpen");

        public Transform PointToLook => pointToLook;
        public bool IsOpen { get; protected set; }
        public bool IsInteractable => _isInteractable;

        protected virtual void Awake()
        {
            AnimController = GetComponent<Animator>();
        }

        public virtual void OpenClose()
        {
            if (_isOpen && _isInteractable)
            {
                AnimController.SetBool(Open, false);
                _isInteractable = false;
                _isOpen = false;
            }
            else if (_isInteractable)
            {
                AnimController.SetBool(Open, true);
                _isInteractable = false;
                _isOpen = true;
            }
        }
        
        public void IndicateOpenClose() //For anim controller
        {
            _isInteractable = true; 
        }
    }
}
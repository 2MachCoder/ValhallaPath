using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.AbstractClasses
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class Item : MonoBehaviour
    {
        private const int LevelsToMoveUp = 3;

        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private MeshCollider physicCollider;
        [SerializeField] private SphereCollider triggerCollider;
        
        public Transform Transform => transform;

        public virtual void Grab(Transform owner)                         
        {
            rigidBody.isKinematic = true;
            triggerCollider.enabled = false;
            physicCollider.enabled = false;
            var transform1 = transform;
            transform1.rotation = owner.rotation;
            transform1.parent = owner;
            transform1.localPosition = Vector3.zero;
        }
        
        public virtual async void Throw(Vector3 force) 
        {
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
            physicCollider.enabled = true;
            Transform parent = transform.parent;
            for (int i = 0; i < LevelsToMoveUp && parent != null; i++)
                parent = parent.parent;
            transform.SetParent(parent);
            rigidBody.AddForce(force, ForceMode.Impulse);
            await UniTask.Delay(500);
            triggerCollider.enabled = true;
        }
    }
}
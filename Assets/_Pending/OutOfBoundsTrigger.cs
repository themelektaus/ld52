using UnityEngine;
using UnityEngine.Events;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(OutOfBoundsTrigger))]
    public class OutOfBoundsTrigger : MonoBehaviour
    {
        [SerializeField] Bounds bounds;
        [SerializeField] UnityEvent onOutOfBounds;

        void Update()
        {
            if (bounds.Contains(transform.position))
                return;

            onOutOfBounds.Invoke();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
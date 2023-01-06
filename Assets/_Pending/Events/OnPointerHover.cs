using UnityEngine;
using UnityEngine.Events;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Pointer Hover")]
    public class OnPointerHover : MonoBehaviour
    {
        [SerializeField] UnityEvent onHover;
        [SerializeField] UnityEvent onElse;

        void Update()
        {
            if (Utils.IsPointerOver(gameObject))
            {
                onHover.Invoke();
                return;
            }

            onElse.Invoke();
        }
    }
}
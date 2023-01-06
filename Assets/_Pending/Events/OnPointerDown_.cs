using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Pointer Down")]
    public class OnPointerDown_ : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] UnityEvent onLeftClick;
        [SerializeField] UnityEvent onRightClick;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                onLeftClick.Invoke();
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                onRightClick.Invoke();
                return;
            }
        }
    }
}
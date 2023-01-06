using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Prototype.Pending
{
    public abstract class MouseBehaviourUI : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler
    {
        protected bool isHovering { get; private set; }
        protected HashSet<int> buttons { get; private set; } = new();

        protected virtual void OnDisable()
        {
            if (isHovering)
            {
                foreach (var button in buttons)
                    OnUp(button);

                OnLeave();
            }

            isHovering = false;
            buttons.Clear();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;
            OnEnter();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;
            OnLeave();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            var button = (int) eventData.button;
            buttons.Add(button);
            OnDown(button);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            var button = (int) eventData.button;
            buttons.Remove(button);
            OnUp(button);
        }

        protected virtual void OnEnter() { }
        protected virtual void OnLeave() { }
        protected virtual void OnDown(int button) { }
        protected virtual void OnUp(int button) { }
    }
}
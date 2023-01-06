using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Prototype.Pending
{
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(Button))]
    public class Button : MouseBehaviour, IDestroyable
    {
        [SerializeField] float destroyDelay = 1;

        public UnityEvent onClick;

        Animator animator;

        protected override void OnDisable()
        {
            animator.SetBool("Hover", false);
            base.OnDisable();
        }

        protected override void OnAwake()
        {
            animator = GetComponent<Animator>();
            animator.keepAnimatorStateOnDisable = true;

            if (animator.GetParameterNames().Contains("Visible"))
                animator.SetBool("Visible", true);
        }

        protected override void OnUpdate()
        {
            animator.SetBool("Hover", isHovering);
        }

        protected override void OnDown()
        {
            animator.SetTrigger("Click");
            onClick.Invoke();
        }

        void LateUpdate()
        {
            animator.ResetTrigger("Click");
        }

        public void PerformClick()
        {
            OnDown();
        }

        public void Destroy()
        {
            this.CreateSequence(() =>
                {
                    if (animator.GetParameterNames().Contains("Visible"))
                        animator.SetBool("Visible", false);
                })
                .Wait(destroyDelay)
                .Kill(gameObject)
                .Start();
        }
    }
}
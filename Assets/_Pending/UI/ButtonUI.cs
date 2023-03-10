using System;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

namespace Prototype.Pending
{
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/Button (UI)")]
    public class ButtonUI : MouseBehaviourUI, IDestroyable
    {
        static DateTime lastClick = DateTime.MinValue;

        [SerializeField] float destroyDelay = 1;
        [SerializeField] float maxClickInterval = .15f;

        public UnityEvent onLeftClick;

        public bool active;

        [SerializeField] SoundEffectCollection clickSound;

        Animator animator;
        bool destroyed;

        bool AnimatorHasParameter(string name)
            => animator.runtimeAnimatorController && animator.GetParameterNames().Contains(name);

        void AnimatorSetBool(string name, bool value)
        {
            if (AnimatorHasParameter(name))
                animator.SetBool(name, value);
        }

        void AnimatorSetTrigger(string name)
        {
            if (AnimatorHasParameter(name))
                animator.SetTrigger(name);
        }

        void AnimatorResetTrigger(string name)
        {
            if (AnimatorHasParameter(name))
                animator.ResetTrigger(name);
        }

        void Awake()
        {
            animator = GetComponent<Animator>();
            animator.keepAnimatorStateOnDisable = true;

            AnimatorSetBool("Visible", true);
            AnimatorSetBool("Disabled", true);
        }

        protected override void OnDisable()
        {
            AnimatorSetBool("Hover", false);
            AnimatorSetBool("Disabled", true);
            AnimatorSetBool("Active", false);
            base.OnDisable();
        }

        void OnEnable()
        {
            AnimatorSetBool("Disabled", false);
        }

        void Update()
        {
            AnimatorSetBool("Hover", !destroyed && isHovering);
            AnimatorSetBool("Active", !destroyed && active);
        }

        protected override void OnDown(int button)
        {
            if (destroyed || active)
                return;

            var now = DateTime.Now;
            if ((now - lastClick).TotalSeconds < maxClickInterval)
                return;

            lastClick = now;

            switch (button)
            {
                case 0:
                    onLeftClick.Invoke();
                    clickSound.PlayRandomClip();
                    AnimatorSetTrigger("Click");
                    break;
            }
        }

        void LateUpdate()
        {
            AnimatorResetTrigger("Click");
        }

        public void PerformLeftClick() => OnDown(0);
        public void PerformRightClick() => OnDown(1);

        public void Destroy()
        {
            destroyed = true;
            this.CreateSequence(() => AnimatorSetBool("Visible", false))
                .Wait(destroyDelay)
                .Kill(gameObject)
                .Start();
        }
    }
}
using UnityEngine;

namespace Prototype.Pending
{
    public abstract class MouseBehaviour : MonoBehaviour, IObserver<RaycastHit>
    {
        [SerializeField] ObjectQuery raycastQuery;

        Raycast raycast;

        bool hit;

        protected bool isHovering { get; private set; }
        protected bool isDown { get; private set; }

        protected void Awake()
        {
            raycast = raycastQuery.FindComponent<Raycast>();
            OnAwake();
        }

        protected virtual void OnEnable()
        {
            var collider = GetComponentInChildren<Collider>();
            this.SubscribeTo(raycast.subject, x => collider == x.collider);
        }

        protected virtual void OnDisable()
        {
            this.UnsubscribeFrom(raycast.subject);

            if (isHovering)
            {
                if (isDown)
                    OnUp();
                OnLeave();
            }

            isHovering = false;
            isDown = false;
        }

        public void ReceiveNotification(RaycastHit hit)
        {
            if (!enabled)
                return;
            
            this.hit = true;

            if (isHovering)
                return;

            isHovering = true;
            OnEnter();
        }

        protected void Update()
        {
            if (isHovering && !isDown && Input.GetMouseButtonDown(0))
            {
                isDown = true;
                OnDown();
            }

            if (isDown && Input.GetMouseButtonUp(0))
            {
                isDown = false;
                if (isHovering)
                    OnUp();
            }

            if (hit)
            {
                hit = false;
                goto Update;
            }

            if (!isDown && isHovering)
            {
                isHovering = false;
                OnLeave();
            }

        Update:
            OnUpdate();
        }

        protected virtual void OnAwake() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnEnter() { }
        protected virtual void OnDown() { }
        protected virtual void OnUp() { }
        protected virtual void OnLeave() { }
    }
}
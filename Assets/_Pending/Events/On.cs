using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Prototype.Pending
{
    public interface IEventInfo<TEvent, TAction> where TEvent : UnityEventBase
    {
        public float delay { get; set; }
        public TEvent @event { get; set; }

        public abstract void OnAddListener(TAction call);
        public abstract void OnRemoveListener(TAction call);
    }

    public abstract class On<TEvent, TAction> : MonoBehaviour where TEvent : UnityEventBase
    {
        [SerializeField] protected bool onlyInEditor;
        [SerializeField] protected bool onlyInBuild;

        protected virtual bool offloadCoroutine => false;
        protected abstract IEventInfo<TEvent, TAction> CreateEventInfo();
        protected abstract void OnAddEventInfo(IEventInfo<TEvent, TAction> eventInfo);

        protected abstract IEnumerable<IEventInfo<TEvent, TAction>> eventInfos { get; }

        protected void Invoke(Action<IEventInfo<TEvent, TAction>> action)
        {
#if UNITY_EDITOR
            if (!eventInfos.Any())
                this.LogWarning("No Event Infos");

            if (onlyInBuild)
                return;
#else
            if (onlyInEditor)
                return;
#endif
            foreach (var eventInfo in eventInfos)
            {
                var @event = eventInfo.@event;
                if (@event is null)
                    continue;

                if (eventInfo.delay > 0)
                {
                    if (offloadCoroutine)
                    {
                        var tempGameObject = new GameObject("Temp Game Object for Events");
                        tempGameObject.AddComponent<Dummy>()
                            .Wait(eventInfo.delay)
                            .Then(() => action(eventInfo))
                            .Destroy(tempGameObject)
                            .Start();

                        continue;
                    }

                    this.Wait(eventInfo.delay)
                        .Start(() => action(eventInfo));

                    continue;
                }

                action(eventInfo);
            }
        }

        public void AddListener(TAction call) => AddListener(0, call);

        public void AddListener(float delay, TAction call)
        {
            var eventInfo = CreateEventInfo();
            eventInfo.delay = delay;
            eventInfo.OnAddListener(call);
            OnAddEventInfo(eventInfo);
        }

        public void RemoveListener(TAction call)
        {
            foreach (var eventInfo in eventInfos)
                eventInfo.OnRemoveListener(call);
        }

        Transform parent;

        public void UseParent(Transform parent)
        {
            this.parent = parent;
        }

        public void Instantiate(GameObject gameObject)
        {
            gameObject.Instantiate(parent);
            parent = null;
        }

        public void DestroyGameObject()
        {
            gameObject.Destroy();
        }

        public void DestroyUnityObject(UnityEngine.Object @object)
        {
            Destroy(@object);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        public void CreateEventSystem()
        {
            if (!EventSystem.current)
            {
                var gameObject = new GameObject("Event System");
                gameObject.AddComponent<EventSystem>();
                gameObject.AddComponent<StandaloneInputModule>();
                gameObject.AddComponent<BaseInput>();
            }
        }
    }

    public abstract class On : On<UnityEvent, UnityAction>
    {
        [Serializable]
        public struct EventInfo : IEventInfo<UnityEvent, UnityAction>
        {
            [SerializeField] float _delay;
            public float delay { get => _delay; set => _delay = value; }

            [SerializeField] UnityEvent _event;
            public UnityEvent @event { get => _event; set => _event = value; }

            public void OnAddListener(UnityAction call) => _event.AddListener(call);
            public void OnRemoveListener(UnityAction call) => _event.RemoveListener(call);
        }

        protected override IEventInfo<UnityEvent, UnityAction> CreateEventInfo()
            => new EventInfo { @event = new() };

        [SerializeField] List<EventInfo> _eventInfos = new();
        protected override IEnumerable<IEventInfo<UnityEvent, UnityAction>> eventInfos
            => _eventInfos.Select(x => x as IEventInfo<UnityEvent, UnityAction>);

        protected override void OnAddEventInfo(IEventInfo<UnityEvent, UnityAction> eventInfo)
            => _eventInfos.Add((EventInfo) eventInfo);

        public void Invoke()
        {
            Invoke(x => x.@event.Invoke());
        }
    }

    public abstract class On<T0> : On<UnityEvent<T0>, UnityAction<T0>>
    {
        [Serializable]
        public struct EventInfo : IEventInfo<UnityEvent<T0>, UnityAction<T0>>
        {
            [SerializeField] float _delay;
            public float delay { get => _delay; set => _delay = value; }

            [SerializeField] UnityEvent<T0> _event;
            public UnityEvent<T0> @event { get => _event; set => _event = value; }

            public void OnAddListener(UnityAction<T0> call) => _event.AddListener(call);
            public void OnRemoveListener(UnityAction<T0> call) => _event.RemoveListener(call);
        }

        protected override IEventInfo<UnityEvent<T0>, UnityAction<T0>> CreateEventInfo()
            => new EventInfo { @event = new() };

        [SerializeField] List<EventInfo> _eventInfos = new();
        protected override IEnumerable<IEventInfo<UnityEvent<T0>, UnityAction<T0>>> eventInfos
            => _eventInfos.Select(x => x as IEventInfo<UnityEvent<T0>, UnityAction<T0>>);

        protected override void OnAddEventInfo(IEventInfo<UnityEvent<T0>, UnityAction<T0>> eventInfo)
            => _eventInfos.Add((EventInfo) eventInfo);

        public void Invoke(T0 arg0)
        {
            Invoke(x => x.@event.Invoke(arg0));
        }
    }
}
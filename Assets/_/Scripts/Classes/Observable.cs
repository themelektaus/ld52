using System.Collections.Generic;
using System.Linq;

namespace Prototype
{
    public class Observable<T> : IObservable<T>
    {
        struct ObserverInfo
        {
            public IObserver<T> observer;
            public System.Predicate<T> validation;
        }

        readonly List<ObserverInfo> observerInfos = new();

        public void Register(IObserver<T> observer, System.Predicate<T> validation = null)
        {
            observerInfos.Add(new()
            {
                observer = observer,
                validation = validation
            });
        }

        public void Unregister(IObserver<T> observer)
        {
            observerInfos.RemoveAll(x => x.observer == observer);
        }

        public void UnregisterAll()
        {
            observerInfos.Clear();
        }

        public void Notify(T message)
        {
            foreach (var observerInfo in observerInfos.ToList())
                if (observerInfo.validation is null || observerInfo.validation(message))
                    observerInfo.observer.ReceiveNotification(message);
        }
    }
}
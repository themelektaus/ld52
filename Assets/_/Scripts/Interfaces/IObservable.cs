namespace Prototype
{
    public interface IObservable<T>
    {
        public void Register(IObserver<T> observer, System.Predicate<T> validation = null);
        public void Unregister(IObserver<T> observer);
        public void UnregisterAll();
    }
}
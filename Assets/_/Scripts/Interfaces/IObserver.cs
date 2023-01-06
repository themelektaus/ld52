namespace Prototype
{
    public interface IObserver<T>
    {
        public void ReceiveNotification(T message);
    }
}
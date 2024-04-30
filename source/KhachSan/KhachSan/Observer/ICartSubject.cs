namespace KhachSan.Observer
{
    public interface ICartSubject
    {
        void AttachObserver(ICartObserver observer);
        void DetachObserver(ICartObserver observer);
        void NotifyObservers();
    }
}

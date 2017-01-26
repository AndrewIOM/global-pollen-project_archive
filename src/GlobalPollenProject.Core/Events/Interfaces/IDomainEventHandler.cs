namespace GlobalPollenProject.Core.Events
{
    public interface IDomainEventHandler<T> where T : IDomainEvent
    {
        void Handle(T args);
    }
}
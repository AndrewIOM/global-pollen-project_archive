using System.ComponentModel;
using GlobalPollenProject.Core.Events;

namespace GlobalPollenProject.Infrastructure.EventHandlers
{
    public interface IDomainEventDispatcher
    {
        void Dispatch(IDomainEvent domainEvent);
    }

    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IContainer _container;

        public DomainEventDispatcher(IContainer container)
        {
            _container = container;
        }

        public void Dispatch(IDomainEvent domainEvent)
        {
            var handlerType = typeof (IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var wrapperType = typeof (DomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = _container.GetAllInstances(handlerType);
            var wrappedHandlers = handlers
                .Cast<object>()
                .Select(handler => (DomainEventHandler) Activator.CreateInstance(wrapperType, handler));
            
            foreach (var handler in wrappedHandlers)
            {
                handler.Handle(domainEvent);
            } 
        }

        private abstract class DomainEventHandler
        {
            public abstract void Handle(IDomainEvent domainEvent);
        }

        private class DomainEventHandler<T> : DomainEventHandler
            where T : IDomainEvent
        {
            private readonly IDomainEventHandler<T> _handler;

            public DomainEventHandler(IDomainEventHandler<T> handler)
            {
                _handler = handler;
            }

            public override void Handle(IDomainEvent domainEvent)
            {
                _handler.Handle((T) domainEvent);
            }
        }
    }
}
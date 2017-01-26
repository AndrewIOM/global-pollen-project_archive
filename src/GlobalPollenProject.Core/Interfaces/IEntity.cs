using System.Collections.Generic;
using GlobalPollenProject.Core.Events;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface IEntity
    {
        object GetState();
        ICollection<IDomainEvent> Events { get; }
    }
}
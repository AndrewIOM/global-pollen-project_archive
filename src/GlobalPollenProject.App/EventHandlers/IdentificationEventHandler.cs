using System;
using GlobalPollenProject.Core.Events;

namespace GlobalPollenProject.Infrastructure.EventHandlers
{
    public class IdentificationEventHandler : IDomainEventHandler<GrainIdentified>
    {
        public void Handle(GrainIdentified args)
        {
            throw new NotImplementedException();
        }
    }
}
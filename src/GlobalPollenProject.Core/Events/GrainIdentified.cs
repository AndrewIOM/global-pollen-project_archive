namespace GlobalPollenProject.Core.Events
{
    public class GrainIdentified : IDomainEvent
    {
        public GrainIdentified(UnknownGrainAggregate grain)
        {
            Grain = grain;
        }

        public UnknownGrainAggregate Grain { get; private set; }
    }
}
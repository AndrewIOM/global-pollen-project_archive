
namespace GlobalPollenProject.Core.Interfaces
{
    public interface IEntity
    {
        bool IsDeleted { get; set; }
        //ICollection<IDomainEvent> Events { get; }
        
    }
}
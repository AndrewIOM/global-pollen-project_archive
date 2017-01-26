namespace GlobalPollenProject.Core.Interfaces
{
    public interface IState<T> where T : IEntity
    {
        T ToEntity(IState<T> state);
    }
}
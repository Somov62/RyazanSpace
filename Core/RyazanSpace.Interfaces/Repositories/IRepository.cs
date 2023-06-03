using RyazanSpace.Interfaces.Entities;

namespace RyazanSpace.Interfaces.Repositories
{
    public interface IRepository<T> : IBaseRepository<T> where T : IEntity
    {
        Task<bool> ExistId(int id, CancellationToken cancel = default);

        Task<T> GetById(int id, CancellationToken cancel = default);

        Task<T> DeleteById(int id, CancellationToken cancel = default);

    }

}

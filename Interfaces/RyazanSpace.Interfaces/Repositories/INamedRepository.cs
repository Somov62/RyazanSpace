using RyazanSpace.Interfaces.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace RyazanSpace.Interfaces.Repositories
{
    public interface INamedRepository<T> : IRepository<T> where T : INamedEntity
    {
        Task<bool> ExistName(string name, CancellationToken cancel = default);

        Task<T> GetByName(string name, CancellationToken cancel = default);

        Task<T> DeleteByName(string name, CancellationToken cancel = default);

    }
}

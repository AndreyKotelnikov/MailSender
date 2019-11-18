using System.Threading;
using System.Threading.Tasks;

namespace Repository.Abstract
{
    public interface IUnitOfWork<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<bool> Update(TEntity entity, CancellationToken cancellationToken = default);

        Task<bool> Delete(TEntity entity, CancellationToken cancellationToken = default);

        Task<bool> DiscardChanges();
    }
}
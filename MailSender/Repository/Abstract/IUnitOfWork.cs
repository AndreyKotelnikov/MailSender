using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Entities.Abstract;

namespace Repository.Abstract
{
    
    public interface IUnitOfWork<TEntity> : IReadOnlyRepository<TEntity>, IWriteOnlyRepositoty<TEntity> where TEntity : class
    {
     
    }
}
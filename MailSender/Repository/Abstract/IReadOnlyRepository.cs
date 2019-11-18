using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Abstract
{
    public interface IReadOnlyRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper,
            CancellationToken cancellationToken );

        /// <summary>
        /// Используется для возврата скалярных результатов запроса
        /// </summary>
        Task<TResult> GetAsync<TResult>(
            Func<IQueryable<TEntity>, TResult> queryShaper,
            CancellationToken cancellationToken);
    }
}

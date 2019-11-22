using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Entities.Abstract;

namespace Repository.Abstract
{
    /// <summary>
    /// Для формирования запросов только на чтение для указанного типа сущности
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public interface IReadOnlyRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> queryExpression,
            CancellationToken cancellationToken );

        /// <summary>
        /// Используется для возврата скалярных результатов запроса
        /// </summary>
        Task<TResult> GetAsync<TResult>(
            Expression<Func<IQueryable<TEntity>, TResult>> queryExpression,
            CancellationToken cancellationToken);
    }
}

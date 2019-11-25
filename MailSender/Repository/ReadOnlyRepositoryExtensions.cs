using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Repository.Abstract;

namespace Repository
{
    public static class ReadOnlyRepositoryExtensions
    {
        public static Task<IEnumerable<T>> GetAsync<T>(
            this IReadOnlyRepository<T> repository,
            Expression<Func<IQueryable<T>, IQueryable<T>>> queryExpression
            ) 
            where T : class 
            => repository.GetAsync(queryExpression, CancellationToken.None);

        public static Task<TResult> GetAsync<T, TResult>(
            this IReadOnlyRepository<T> repository,
            Expression<Func<IQueryable<T>, TResult>> queryExpression
            ) 
            where T : class 
            => repository.GetAsync(queryExpression, CancellationToken.None);

        public static Task<IEnumerable<T>> GetAllAsync<T>(
            this IReadOnlyRepository<T> repository
            ) 
            where T : class 
            => repository.GetAsync(q => q, CancellationToken.None);

        public static Task<IEnumerable<T>> GetAllAsync<T>(
            this IReadOnlyRepository<T> repository, 
            CancellationToken cancellationToken
            ) 
            where T : class 
            => repository.GetAsync(q => q, cancellationToken);

        public static Task<IEnumerable<T>> FindByAsync<T>(
            this IReadOnlyRepository<T> repository, 
            Expression<Func<T, bool>> predicate
            ) 
            where T : class 
            => repository.GetAsync(q => q.Where(predicate), CancellationToken.None);

        public static Task<int> GetMaxIdAsync<T>(this IReadOnlyRepository<T> repository, CancellationToken cancellationToken)
            where T : class
            => repository.GetMaxIdAsync(cancellationToken);

        public static Task<int> GetMaxIdAsync<T>(this IReadOnlyRepository<T> repository)
            where T : class
            => repository.GetMaxIdAsync(CancellationToken.None);
    }
}

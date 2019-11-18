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
            this IReadOnlyRepository<T> repository, Func<IQueryable<T>, 
                IQueryable<T>> queryShaper
            ) 
            where T : class 
            => repository.GetAsync(queryShaper, CancellationToken.None);

        public static Task<TResult> GetAsync<T, TResult>(
            this IReadOnlyRepository<T> repository, 
            Func<IQueryable<T>, TResult> queryShaper
            ) 
            where T : class 
            => repository.GetAsync(queryShaper, CancellationToken.None);

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
    }
}

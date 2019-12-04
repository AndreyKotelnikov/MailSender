using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CodeFirstDbContext.Abstract;
using Entities.Abstract;
using RepositoryAbstract;

namespace CodeFirstDbContext
{
    public sealed class UnitOfWorkEF<TEntity> : IUnitOfWork<TEntity> where TEntity : class, IUniqueEntity
    {
        private readonly IDbContextProvider _dbContextProvider;

        private readonly IConcurrencyExceptionsResolver _concurrencyExceptionsResolver;


        public UnitOfWorkEF(IDbContextProvider dbContextProvider, IConcurrencyExceptionsResolver concurrencyExceptionsResolver)
        {
            if (typeof(TEntity).GetInterfaces().All(t => t != typeof(IBaseEntity)))
            {
                throw new TypeAccessException($"Тип {nameof(TEntity)} не содержит реализацию интерфейса {nameof(IBaseEntity)}");
            }

            _dbContextProvider = dbContextProvider;
            _concurrencyExceptionsResolver = concurrencyExceptionsResolver;
        }

        public async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            using (DbContext context = _dbContextProvider.GetDbContext())
            {
                (entity as IBaseEntity).CreatedDate = DateTime.Now;
                context.Set<TEntity>().Attach(entity);
                context.Set<TEntity>().Add(entity);
                if (await _concurrencyExceptionsResolver.SaveChangesWithResolvesConcurrencyExceptionsAsync(context, cancellationToken) > 0)
                {
                    return entity.Id;
                }
                return 0;
            }
        }

        public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            using (DbContext context = _dbContextProvider.GetDbContext())
            {
                (entity as IBaseEntity).CreatedDate = DateTime.Now;
                context.Set<TEntity>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                return await _concurrencyExceptionsResolver.SaveChangesWithResolvesConcurrencyExceptionsAsync(context, cancellationToken) > 0;
            }
        }

        public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            using (DbContext context = _dbContextProvider.GetDbContext())
            {
                context.Set<TEntity>().Attach(entity);
                context.Set<TEntity>().Remove(entity);
                return await _concurrencyExceptionsResolver.SaveChangesWithResolvesConcurrencyExceptionsAsync(context, cancellationToken) > 0;
            }
        }

        public async Task<List<int>> AddRangeAsync(IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            using (DbContext context = _dbContextProvider.GetDbContext())
            {
                foreach (var entity in entities)
                {
                    (entity as IBaseEntity).CreatedDate = DateTime.Now;
                }
                context.Set<TEntity>().AddRange(entities); 
                if (await _concurrencyExceptionsResolver.SaveChangesWithResolvesConcurrencyExceptionsAsync(context, cancellationToken) > 0)
                {
                    return entities.Select(e => e.Id).ToList();
                }
                return new List<int>();
            }
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            using (DbContext context = _dbContextProvider.GetDbContext())
            {
                //TODO убрать цикл foreach ниже после реализации тестов
                foreach (var entity in entities)
                {
                    (entity as IBaseEntity).CreatedDate = DateTime.Now;
                }
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;

                foreach (TEntity entity in entities)
                {
                    context.Set<TEntity>().Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;
                }

                context.Configuration.AutoDetectChangesEnabled = true;
                context.Configuration.ValidateOnSaveEnabled = true;
                
                return await _concurrencyExceptionsResolver.SaveChangesWithResolvesConcurrencyExceptionsAsync(context,
                           cancellationToken) > 0;
            }
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            using (DbContext context = _dbContextProvider.GetDbContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;

                foreach (TEntity entity in entities)
                {
                    context.Set<TEntity>().Attach(entity);
                    context.Entry(entity).State = EntityState.Deleted;
                }

                context.Configuration.AutoDetectChangesEnabled = true;
                context.Configuration.ValidateOnSaveEnabled = true;
                
                return await _concurrencyExceptionsResolver.SaveChangesWithResolvesConcurrencyExceptionsAsync(context,
                           cancellationToken) > 0;
            }
        }


        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> queryExpression, CancellationToken cancellationToken)
        {
            using (DbContext context = _dbContextProvider.GetDbContext())
            {
                var query = queryExpression.Compile();

                return await query(context.Set<TEntity>())
                    .AsNoTracking()
                    .ToArrayAsync(cancellationToken);
            }
        }

        public async Task<TResult> GetAsync<TResult>(
            Expression<Func<IQueryable<TEntity>, TResult>> queryExpression,
            CancellationToken cancellationToken)
        {
            using (DbContext context = _dbContextProvider.GetDbContext())
            {
                var set = context.Set<TEntity>();
                var query = queryExpression.Compile();
                var factory = Task<TResult>.Factory;
                return await factory.StartNew(() => query(set), cancellationToken);
            }
        }


        Task<int> IReadOnlyRepository<TEntity>.GetMaxIdAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}

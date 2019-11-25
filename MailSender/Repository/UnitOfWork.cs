using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CodeFirstDbContext;
using CodeFirstDbContext.Abstract;
using Entities.Abstract;
using Repository.Abstract;
using AutoMapper;
using Repository.AsyncQueryProvider;

namespace Repository
{
    public sealed class UnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly IDbContextProvider _dbContextProvider;

        private readonly ConcurrentDictionary<int, int> _keysList = new ConcurrentDictionary<int, int>();

        private Task<bool> _isKeysLoaded;

        

        internal UnitOfWork(IDbContextProvider dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _isKeysLoaded = Task.Run(LoadKeysFromDbSetAsync);
        }

        public async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (await CheckExistByIdAsync(entity.Id))
            {
                throw new ArgumentOutOfRangeException(nameof(entity),
                    $"Id со значением {entity.Id} уже содержится в базе данных");
            }

            using (IDbContext context = _dbContextProvider.GetDbContext())
            {
                entity.CreatedDate = DateTime.Now;
                context.Add(entity);
                if (await SaveChangesWithResolvesOptimisticConcurrencyExceptionsAsClientPriorityAsync(context, cancellationToken) > 0)
                {
                    _keysList.TryAdd(entity.Id, entity.Id);
                }
                return entity.Id;
            }
        }

        public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (!await CheckExistByIdAsync(entity.Id))
            {
                throw new ArgumentOutOfRangeException(nameof(entity), $"Id со значением {entity.Id} в базе данных не найден");
            }

            using (IDbContext context = _dbContextProvider.GetDbContext())
            {
                entity.CreatedDate = DateTime.Now;
                context.Update(entity);
                return await SaveChangesWithResolvesOptimisticConcurrencyExceptionsAsClientPriorityAsync(context, cancellationToken) > 0;
            }
        }

        public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (!await CheckExistByIdAsync(entity.Id))
            {
                throw new ArgumentOutOfRangeException(nameof(entity), $"Id со значением {entity.Id} в базе данных не найден");
            }

            using (IDbContext context = _dbContextProvider.GetDbContext())
            {
                context.Remove(entity);
                if (await SaveChangesWithResolvesOptimisticConcurrencyExceptionsAsClientPriorityAsync(context, cancellationToken) > 0)
                {
                    _keysList.TryRemove(entity.Id, out int i);
                    return true;
                }
                return false;
            }

        }

        public async Task<List<int>> AddRangeAsync(IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            if (entities.Any(e => e == null)) throw new ArgumentNullException(nameof(entities));

            if (entities.Any(e => CheckExistByIdAsync(e.Id).Result))
            {
                throw new ArgumentOutOfRangeException(nameof(entities), 
                    $"Id со значением {entities.First(e => CheckExistByIdAsync(e.Id).Result)} уже содержится в базе данных");
            }

            using (IDbContext context = _dbContextProvider.GetDbContext())
            {
                foreach (var entity in entities)
                {
                    entity.CreatedDate = DateTime.Now;
                }
                context.AddRange(entities);
                if (await SaveChangesWithResolvesOptimisticConcurrencyExceptionsAsClientPriorityAsync(context, cancellationToken) > 0)
                {
                    foreach (var entity in entities)
                    {
                        _keysList.TryAdd(entity.Id, entity.Id);
                    }
                    
                }
                return entities.Select(e => e.Id).ToList();
            }
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities.Any(e => e == null)) throw new ArgumentNullException(nameof(entities));
            

            if (entities.Any(e => !CheckExistByIdAsync(e.Id).Result))
            {
                throw new ArgumentOutOfRangeException(nameof(entities), 
                    $"Id со значением {entities.First(e => !CheckExistByIdAsync(e.Id).Result)} в базе данных не найден");
            }

            using (IDbContext context = _dbContextProvider.GetDbContext())
            {
                //TODO убрать цикл foreach ниже после реализации тестов
                foreach (var entity in entities)
                {
                    entity.CreatedDate = DateTime.Now;
                }
                context.UpdateRange(entities);
                if (await SaveChangesWithResolvesOptimisticConcurrencyExceptionsAsClientPriorityAsync(context, cancellationToken) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities.Any(e => e == null)) throw new ArgumentNullException(nameof(entities));

            if (entities.Any(e => !CheckExistByIdAsync(e.Id).Result))
            {
                throw new ArgumentOutOfRangeException(nameof(entities), 
                    $"Id со значением {entities.First(e => !CheckExistByIdAsync(e.Id).Result)} в базе данных не найден");
            }

            using (IDbContext context = _dbContextProvider.GetDbContext())
            {
                context.RemoveRange(entities);
                if (await SaveChangesWithResolvesOptimisticConcurrencyExceptionsAsClientPriorityAsync(context, cancellationToken) > 0)
                {
                    foreach (var entity in entities)
                    {
                        _keysList.TryRemove(entity.Id, out int i);
                    }

                    return true;
                }
                return false;
            }
        }

        private async Task<int> SaveChangesWithResolvesOptimisticConcurrencyExceptionsAsClientPriorityAsync(IDbContext context, CancellationToken cancellationToken)
        {
            int numberOfSavedChanges = 0;
            bool isSaveFailed;
            do
            {
                isSaveFailed = false;
                try
                {
                    numberOfSavedChanges = await context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    isSaveFailed = true;
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
            } while (isSaveFailed);
            return numberOfSavedChanges;
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> queryExpression, CancellationToken cancellationToken)
        {
            using (IDbContext context = _dbContextProvider.GetDbContext())
            {
                var query = queryExpression.Compile();

                var v = await query(context.Set<TEntity>())
                    .ToArrayAsync(cancellationToken);
                return v;
            }
        }

        public async Task<TResult> GetAsync<TResult>(
            Expression<Func<IQueryable<TEntity>, TResult>> queryExpression,
            CancellationToken cancellationToken)
        {
            using (IDbContext context = _dbContextProvider.GetDbContext())
            {
                var set = context.Set<TEntity>();
                var query = queryExpression.Compile();
                var factory = Task<TResult>.Factory;
                return await factory.StartNew(() => query(set), cancellationToken);
            }
        }

        private async Task<bool> LoadKeysFromDbSetAsync()
        {
            using (IDbContext context = _dbContextProvider.GetDbContext())
            {
                var keysFromDb = await context.Set<TEntity>().Select(e => e.Id).ToListAsync().ConfigureAwait(false);
                foreach (var id in keysFromDb)
                {
                    _keysList.TryAdd(id, id);
                }
            }

            return _keysList.Count > 0;
        }

        private async Task<bool> CheckExistByIdAsync(int id)
        {
            await Task.WhenAll(_isKeysLoaded);
            
            return _keysList.TryGetValue(id, out int i);
        }

        public async Task<int> GetMaxIdAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(_isKeysLoaded);

            return _keysList.Max(k => k.Key);
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeFirstDbContext;
using CodeFirstDbContext.Abstract;
using Entities.Abstract;
using Repository.Abstract;
using AutoMapper;

namespace Repository
{
    public sealed class UnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly Type _typeDbContext;

        private readonly ConcurrentDictionary<int, int> _keysList = new ConcurrentDictionary<int, int>();

        private Task<bool> _isKeysLoaded;

        

        internal UnitOfWork(Type typeDbContext)
        {
            _typeDbContext = typeDbContext;
            _isKeysLoaded = Task.Run(LoadKeysFromDbSetAsync);
        }

        public async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.Id != 0) throw new ArgumentOutOfRangeException(nameof(entity.Id), "Id должен быть равен 0");

            if (await CheckExistByIdAsync(entity.Id))
            {
                return 0;
            }

            using (IDbContext context = (IDbContext)Activator.CreateInstance(_typeDbContext))
            {
                entity.CreatedDate = DateTime.Now;
                context.Attach(entity);
                context.Add(entity);
                if (await SaveChangesWithResolvesOptimisticConcurrencyExceptionsAsClientPriorityAsync(context, cancellationToken) > 0)
                {
                    _keysList.TryAdd(entity.Id, entity.Id);
                }
                
                return entity.Id;
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

        public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (!await CheckExistByIdAsync(entity.Id))
            {
                return false;
            }

            using (IDbContext context = (IDbContext)Activator.CreateInstance(_typeDbContext))
            {
                entity.CreatedDate = DateTime.Now;
                context.Attach(entity);
                context.Update(entity);
                return await SaveChangesWithResolvesOptimisticConcurrencyExceptionsAsClientPriorityAsync(context, cancellationToken) > 0;
            }
        }

        public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (!await CheckExistByIdAsync(entity.Id))
            {
                return false;
            }

            using (IDbContext context = (IDbContext)Activator.CreateInstance(_typeDbContext))
            {
                context.Attach(entity);
                context.Remove(entity);
                if (await SaveChangesWithResolvesOptimisticConcurrencyExceptionsAsClientPriorityAsync(context, cancellationToken) > 0)
                {
                    _keysList.TryRemove(entity.Id, out int i);
                    return true;
                }
                
                return false;
            }

        }

        public async Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper, CancellationToken cancellationToken)
        {
            using (IDbContext context = (IDbContext)Activator.CreateInstance(_typeDbContext))
            {
                var query = queryShaper(context.Set<TEntity>());
                return await query.ToArrayAsync(cancellationToken);
            }
        }

        public async Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, TResult> queryShaper,
            CancellationToken cancellationToken)
        {
            using (IDbContext context = (IDbContext)Activator.CreateInstance(_typeDbContext))
            {
                var set = context.Set<TEntity>();
                var query = queryShaper;
                var factory = Task<TResult>.Factory;
                return await factory.StartNew(() => query(set), cancellationToken);
            }
        }

        private async Task<bool> LoadKeysFromDbSetAsync()
        {
            using (IDbContext context = (IDbContext)Activator.CreateInstance(_typeDbContext))
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
    }
}
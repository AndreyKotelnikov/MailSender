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

using AutoMapper;
using Repository.AsyncQueryProvider;
using RepositoryAbstract;

namespace Repository
{
    public sealed class UnitOfWorkContractDecorator<TEntity> : IUnitOfWork<TEntity> where TEntity : class, IUniqueEntity
    {
        private readonly IUnitOfWork<TEntity> _unitOfWork;

        private readonly ConcurrentDictionary<int, int> _keysList = new ConcurrentDictionary<int, int>();

        private readonly Task<bool> _isKeysLoaded;



        internal UnitOfWorkContractDecorator(IUnitOfWork<TEntity> unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            var result = await _unitOfWork.AddAsync(entity, cancellationToken);

            if (result > 0)
            {
                _keysList.TryAdd(result, result);
            }

            return result;
        }

        public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (!await CheckExistByIdAsync(entity.Id))
            {
                throw new ArgumentOutOfRangeException(nameof(entity), $"Id со значением {entity.Id} в базе данных не найден");
            }

            return await _unitOfWork.UpdateAsync(entity, cancellationToken);
        }

        public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (!await CheckExistByIdAsync(entity.Id))
            {
                throw new ArgumentOutOfRangeException(nameof(entity), $"Id со значением {entity.Id} в базе данных не найден");
            }

            var result = await _unitOfWork.DeleteAsync(entity, cancellationToken);

            if (result)
            {
                _keysList.TryRemove(entity.Id, out int i);
            }

            return result;
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

            var result = await _unitOfWork.AddRangeAsync(entities, cancellationToken);

            foreach (var id in result)
            {
                _keysList.TryAdd(id, id);
            }

            return result;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities.Any(e => e == null)) throw new ArgumentNullException(nameof(entities));


            if (entities.Any(e => !CheckExistByIdAsync(e.Id).Result))
            {
                throw new ArgumentOutOfRangeException(nameof(entities),
                    $"Id со значением {entities.First(e => !CheckExistByIdAsync(e.Id).Result)} в базе данных не найден");
            }

            return await _unitOfWork.UpdateRangeAsync(entities, cancellationToken);
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities.Any(e => e == null)) throw new ArgumentNullException(nameof(entities));

            if (entities.Any(e => !CheckExistByIdAsync(e.Id).Result))
            {
                throw new ArgumentOutOfRangeException(nameof(entities),
                    $"Id со значением {entities.First(e => !CheckExistByIdAsync(e.Id).Result)} в базе данных не найден");
            }

            var result = await _unitOfWork.DeleteRangeAsync(entities, cancellationToken);

            if (result)
            {
                foreach (var entity in entities)
                {
                    _keysList.TryRemove(entity.Id, out int i);
                }
            }

            return result;
        }

        

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> queryExpression, CancellationToken cancellationToken) =>
            await _unitOfWork.GetAsync(queryExpression, cancellationToken);

        public async Task<TResult> GetAsync<TResult>(
            Expression<Func<IQueryable<TEntity>, TResult>> queryExpression,
            CancellationToken cancellationToken) =>
            await _unitOfWork.GetAsync(queryExpression, cancellationToken);

        private async Task<bool> LoadKeysFromDbSetAsync()
        {
            var keysFromDb = await _unitOfWork.GetAsync(query => query.Select(e => e.Id).ToList()).ConfigureAwait(false);

            foreach (var id in keysFromDb)
            {
                _keysList.TryAdd(id, id);
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
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DelegateDecompiler.EntityFramework;
using System.Data.Entity.Infrastructure;
using AutoMapper.Extensions.ExpressionMapping;
using Entities;
using Repository.AsyncQueryProvider;
using Domain;
using RepositoryAbstract;

namespace Repository
{
    public sealed class UnitOfWorkMapperDecorator<TUpLayer, TDownLayer> : IUnitOfWork<TUpLayer> 
        where TUpLayer : class
        where TDownLayer : class
    {
        private readonly IUnitOfWork<TDownLayer> _unitOfWork;

        private readonly IMapper _mapper;

        internal UnitOfWorkMapperDecorator(IUnitOfWork<TDownLayer> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork
                ?? throw new NullReferenceException(nameof(unitOfWork));
            _mapper = mapper
                      ?? throw new NullReferenceException(nameof(mapper));

        }

        public async Task<int> AddAsync(TUpLayer entity, CancellationToken cancellationToken = default) =>
            await _unitOfWork.AddAsync(_mapper.Map<TDownLayer>(entity), cancellationToken);

        public async Task<List<int>> AddRangeAsync(IEnumerable<TUpLayer> entities,
            CancellationToken cancellationToken = default) =>
            await _unitOfWork.AddRangeAsync(_mapper.Map<IEnumerable<TDownLayer>>(entities), cancellationToken);

        public async Task<bool> DeleteAsync(TUpLayer entity, CancellationToken cancellationToken = default) =>
            await _unitOfWork.DeleteAsync(_mapper.Map<TDownLayer>(entity), cancellationToken);

        public async Task<bool> DeleteRangeAsync(IEnumerable<TUpLayer> entities, CancellationToken cancellationToken = default) => 
            await _unitOfWork.DeleteRangeAsync(_mapper.Map<IEnumerable<TDownLayer>>(entities), cancellationToken);

        public async Task<IEnumerable<TUpLayer>> GetAsync(
            Expression<Func<IQueryable<TUpLayer>, IQueryable<TUpLayer>>> queryExpression,
            CancellationToken cancellationToken)
        {
            var expressionToEntity = _mapper.MapExpression<Expression<Func<IQueryable<TDownLayer>, IQueryable<TDownLayer>>>>(queryExpression);
            var entityList = await _unitOfWork.GetAsync(expressionToEntity, cancellationToken);
            return _mapper.Map<IEnumerable<TUpLayer>>(entityList);
        }
            
        public async Task<TResult> GetAsync<TResult>(
            Expression<Func<IQueryable<TUpLayer>, TResult>> queryExpression,
            CancellationToken cancellationToken)
        {
            var expressionToEntity = _mapper.MapExpression<Expression<Func<IQueryable<TDownLayer>, TResult>>>(queryExpression);
            var result = await _unitOfWork.GetAsync(expressionToEntity, cancellationToken);
            return result;
        }

        public async Task<int> GetMaxIdAsync(CancellationToken cancellationToken) =>
            await _unitOfWork.GetMaxIdAsync(cancellationToken);

        public async Task<bool> UpdateAsync(TUpLayer entity, CancellationToken cancellationToken = default) =>
            await _unitOfWork.UpdateAsync(_mapper.Map<TDownLayer>(entity), cancellationToken);

        public async Task<bool> UpdateRangeAsync(IEnumerable<TUpLayer> entities, CancellationToken cancellationToken = default) =>
        await _unitOfWork.UpdateRangeAsync(_mapper.Map<IEnumerable<TDownLayer>>(entities), cancellationToken);
    }
}

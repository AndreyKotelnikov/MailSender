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
using Repository.Abstract;
using System.Data.Entity.Infrastructure;
using Repository.AsyncQueryProvider;

namespace Repository
{
    public sealed class UnitOfWorkMapperDecorator<TDomain, TEntity> : IUnitOfWork<TDomain> 
        where TDomain : class
        where TEntity : class
    {
        private readonly IUnitOfWork<TEntity> _unitOfWork;

        private readonly IMapper _mapper;

        internal UnitOfWorkMapperDecorator(IUnitOfWork<TEntity> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(TDomain entity, CancellationToken cancellationToken = default) =>
            await _unitOfWork.AddAsync(_mapper.Map<TEntity>(entity), cancellationToken); 
        

        public async Task<bool> DeleteAsync(TDomain entity, CancellationToken cancellationToken = default) =>
            await _unitOfWork.DeleteAsync(_mapper.Map<TEntity>(entity), cancellationToken);

        public async Task<IEnumerable<TDomain>> GetAsync(Func<IQueryable<TDomain>, IQueryable<TDomain>> queryShaper, CancellationToken cancellationToken) =>
            _mapper.Map<IEnumerable<TDomain>>( 
                await _unitOfWork.GetAsync(
                    e => queryShaper(_mapper.ProjectTo<TDomain>(e)).AsEnumerable().Select(d => _mapper.Map<TEntity>(d)).AsAsyncQueryable()
                    //e => queryShaper(_mapper.ProjectTo<TDomain>(e)).AsAsyncEnumerableQuery().ProjectTo<TEntity>(_mapper.ConfigurationProvider)
                    //e => _mapper.Map<IEnumerable<TEntity>>(queryShaper(_mapper.ProjectTo<TDomain>(e)).AsEnumerable()).AsQueryable()
                    //MappingQueryShaper(queryShaper)
                    , cancellationToken));

        //private Func<IQueryable<TEntity>, IQueryable<TEntity>> MappingQueryShaper(
        //    Func<IQueryable<TDomain>, IQueryable<TDomain>> queryShaperDomain)
        //{
        //    Expression expr;
        //    IQueryProvider provider;

        //    Func<IQueryable<TEntity>, IQueryable<TDomain>> v1 = e =>
        //    {
        //        var method = queryShaperDomain(_mapper.ProjectTo<TDomain>(e));
        //        expr = method.Expression;
        //        provider = method.Provider;
        //        return method;
        //    };

        //    v1.BeginInvoke()

        //    var query = provider.CreateQuery<TEntity>(expr);

        //    Func<IQueryable<TEntity>, IQueryable<TEntity>> v2 = e => v1(e).AsEnumerable().AsQueryable().ProjectTo<TEntity>(_mapper.ConfigurationProvider);
        //    return v2;
        //}


        public async Task<TResult> GetAsync<TResult>(Func<IQueryable<TDomain>, TResult> queryShaper,
            CancellationToken cancellationToken) =>
            await _unitOfWork.GetAsync(
                e => queryShaper(_mapper.ProjectTo<TDomain>(e)),
                cancellationToken);


        public async Task<bool> UpdateAsync(TDomain entity, CancellationToken cancellationToken = default) =>
            await _unitOfWork.UpdateAsync(_mapper.Map<TEntity>(entity), cancellationToken);

    }
}

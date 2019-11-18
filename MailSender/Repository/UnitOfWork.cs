using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeFirstDbContext;
using CodeFirstDbContext.Abstract;
using Repository.Abstract;

namespace Repository
{
    public class UnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity : class
    {
        private Type _typeDbContext;

        internal UnitOfWork(Type typeDbContext)
        {
            _typeDbContext = typeDbContext;
        }

        public Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DiscardChanges()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper, CancellationToken cancellationToken)
        {
            using (IDbContext context = new MailSenderDbContext())
            {
                var query = queryShaper(context.Set<TEntity>());
                return await query.ToArrayAsync(cancellationToken);
            }
        }

        public async Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, TResult> queryShaper,
            CancellationToken cancellationToken)
        {
            using (IDbContext context = new MailSenderDbContext())
            {
                var set = context.Set<TEntity>();
                var query = queryShaper;
                var factory = Task<TResult>.Factory;
                return await factory.StartNew(() => query(set), cancellationToken);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CodeFirstDbContext;
using CodeFirstDbContext.Abstract;
using Entities;
using Repository.Abstract;

namespace Repository
{
    public class Repository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
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

    //public class RecipientRepository: IReadOnlyRepository<Recipient>
    //{

    //    public async Task<IEnumerable<Recipient>> GetAsync(Func<IQueryable<Recipient>, IQueryable<Recipient>> queryShaper, CancellationToken cancellationToken)
    //    {
    //        using (IDbContext context = new MailSenderDbContext())
    //        {
    //            var query = queryShaper(context.Set<Recipient>());
    //            return await query.ToArrayAsync(cancellationToken);
    //        }
    //    }

    //    public async Task<TResult> GetAsync<TResult>(Func<IQueryable<Recipient>, TResult> queryShaper,
    //        CancellationToken cancellationToken)
    //    {
    //        using (IDbContext context = new MailSenderDbContext())
    //        {
    //            var set = context.Set<Recipient>();
    //            var query = queryShaper;
    //            var factory = Task<TResult>.Factory;
    //            return await factory.StartNew(() => query(set), cancellationToken);
    //        }
    //    }
    //}
}

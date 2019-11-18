using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeFirstDbContext.Abstract
{
    public interface IDbContext : IDisposable
    {
        IQueryable<TEntity> Set<TEntity>() where TEntity : class;

    }
}

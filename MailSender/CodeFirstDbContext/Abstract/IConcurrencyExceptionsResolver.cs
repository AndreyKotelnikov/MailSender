using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeFirstDbContext.Abstract
{
    public interface IConcurrencyExceptionsResolver
    {
        Task<int> SaveChangesWithResolvesConcurrencyExceptionsAsync(DbContext context, CancellationToken cancellationToken);
    }
}

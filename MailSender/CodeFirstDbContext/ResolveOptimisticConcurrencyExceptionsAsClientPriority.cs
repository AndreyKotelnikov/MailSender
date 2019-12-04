using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeFirstDbContext.Abstract;

namespace CodeFirstDbContext
{
    public class ResolveOptimisticConcurrencyExceptionsAsClientPriority : IConcurrencyExceptionsResolver
    {
        public async Task<int> SaveChangesWithResolvesConcurrencyExceptionsAsync(DbContext context, CancellationToken cancellationToken)
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
                    //TODO устранить причину частых конкурентных исключений
                    isSaveFailed = true;
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
            } while (isSaveFailed);
            return numberOfSavedChanges;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstDbContext.Abstract;

namespace CodeFirstDbContext
{
    public class CodeFirstDbContextProvider : IDbContextProvider
    {
        private readonly Type _typeDbContext;

        public CodeFirstDbContextProvider(Type typeDbContext)
        {
            _typeDbContext = typeDbContext;
        }

        public DbContext GetDbContext()
        {
            return (DbContext) Activator.CreateInstance(_typeDbContext);
        }
    }
}

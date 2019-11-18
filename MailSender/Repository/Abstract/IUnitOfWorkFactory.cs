using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstract
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork<TEntity> GetUnitOfWork<TEntity>() where TEntity : class;
    }
}

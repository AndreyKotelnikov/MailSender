using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstDbContext.Abstract
{
    public interface IDbContextProvider
    {
        /// <summary>
        /// Предоставляет контекст для подключения к базе данных
        /// </summary>
        /// <returns></returns>
        IDbContext GetDbContext();
    } 
}

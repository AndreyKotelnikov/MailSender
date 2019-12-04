using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryAbstract
{
    /// <summary>
    /// Создание UnitOfWork определённого типа сущности
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// Создаёт UnitOfWork указанного типа сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <returns>Возвращает UnitOfWork указанного типа сущности</returns>
        IUnitOfWork<TEntity> GetCurrentUnitOfWork<TEntity>() where TEntity : class, IUniqueEntity;
    }
}

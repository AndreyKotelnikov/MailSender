using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstDbContext.Abstract;
using Entities.Abstract;
using Repository.Abstract;

namespace Repository
{
    public sealed class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly Type _typeDbContext;

        private readonly ConcurrentDictionary<Type, object> _unitsOfWork = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Конструктор для создания фабрики UnitOfWork
        /// </summary>
        /// <param name="typeDbContext">Тип класса для подключения к базе данных</param>
        public UnitOfWorkFactory(Type typeDbContext)
        {
            _typeDbContext = typeDbContext;
        }

        public IUnitOfWork<TEntity> GetUnitOfWork<TEntity>() where TEntity : class, IBaseEntity
        {
            var type = typeof(TEntity);
            object current;

            if (this._unitsOfWork.TryGetValue(type, out current))
                return (IUnitOfWork<TEntity>)current;

            var newItem = new UnitOfWork<TEntity>(_typeDbContext);
            this._unitsOfWork[type] = newItem;
            return newItem;
        }
    }
}

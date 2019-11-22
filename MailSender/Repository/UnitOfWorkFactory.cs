using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CodeFirstDbContext.Abstract;
using Entities;
using Entities.Abstract;
using MapperLib.Abstract;
using Repository.Abstract;

namespace Repository
{
    public sealed class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly Type _typeDbContext;

        private readonly IMapperFactory _mapperFactory;

        private readonly ConcurrentDictionary<Type, object> _unitsOfWork = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Конструктор для создания фабрики UnitOfWork
        /// </summary>
        /// <param name="typeDbContext">Тип класса для подключения к базе данных</param>
        /// <param name="mapperFactory">Класс для создания объектов меппинга между слоями архитектуры.
        /// Если mapping не требуется, то параметр можно не указывать</param>
        public UnitOfWorkFactory(Type typeDbContext, IMapperFactory mapperFactory = null)
        {
            _typeDbContext = typeDbContext;
            _mapperFactory = mapperFactory;
        }

        public IUnitOfWork<T> GetCurrentUnitOfWork<T>() where T : class
        {
            var type = typeof(T);
            object current;

            if (!_unitsOfWork.TryGetValue(type, out current))
            {
                current = CreateUnitOfWork<T>();
                if (current == null) throw new ArgumentNullException(nameof(type), $"Для {type.Name} не удалось создать UnitOfWork");
                _unitsOfWork[type] = current;
            }

            return (IUnitOfWork<T>)current;
        }

        private IUnitOfWork<T> CreateUnitOfWork<T>() where T : class
        {
            var sourceType = typeof(T);
            if (sourceType.GetInterfaces().Any(t => t == typeof(IBaseEntity)))
            {
                var methodCreateUnitOfWorkEntity = GetType().GetMethod("CreateUnitOfWorkEntity", BindingFlags.Instance | BindingFlags.NonPublic);
                
                return (IUnitOfWork<T>)methodCreateUnitOfWorkEntity?.MakeGenericMethod(sourceType)
                    .Invoke(this, null);
            }

            var distType = _mapperFactory?.GetDestinationType<T> ();
            if (distType == null) 
                throw new ArgumentNullException(nameof(distType), $"Для {sourceType.Name} в объекте {typeof(IMapperFactory).Name} не указан связанный тип Entity");

            var methodCreateUnitOfWorkMapper = GetType().GetMethod("CreateUnitOfWorkMapper", BindingFlags.Instance | BindingFlags.NonPublic);
            return (IUnitOfWork<T>)methodCreateUnitOfWorkMapper?.MakeGenericMethod(sourceType, distType)
                .Invoke(this, null);
        }

        private IUnitOfWork<TEntity> CreateUnitOfWorkEntity<TEntity>() where TEntity : class, IBaseEntity => 
            new UnitOfWork<TEntity>(_typeDbContext);
            

        private IUnitOfWork<TDomain> CreateUnitOfWorkMapper<TDomain, TEntity>() 
            where TDomain : class
            where TEntity : class, IBaseEntity
        {
            IMapper mapper = _mapperFactory.GetMapper<TDomain>();
            IUnitOfWork<TEntity> unitOfWorkEntity = CreateUnitOfWorkEntity<TEntity>();
            IUnitOfWork<TDomain> unitOfWorkMapper = new UnitOfWorkMapperDecorator<TDomain,TEntity>(unitOfWorkEntity, mapper);
            return unitOfWorkMapper;
        }
    }
}

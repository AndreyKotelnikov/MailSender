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
using Repository.Abstract;

namespace Repository
{
    public sealed class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDbContextProvider _dbContextProvider;

        private readonly IMapper _mapper;

        private readonly ConcurrentDictionary<Type, object> _unitsOfWorks = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Конструктор для создания фабрики UnitOfWork
        /// </summary>
        /// <param name="typeDbContext">Тип класса для подключения к базе данных</param>
        /// <param name="mapper">Класс для меппинга между слоями архитектуры.
        /// Если mapping не требуется, то параметр можно не указывать</param>
        public UnitOfWorkFactory(IDbContextProvider dbContextProvider, IMapper mapper = null)
        {
            _dbContextProvider = dbContextProvider;
            _mapper = mapper;
        }

        public IUnitOfWork<T> GetCurrentUnitOfWork<T>() where T : class
        {
            var type = typeof(T);
            object current;

            if (!_unitsOfWorks.TryGetValue(type, out current))
            {
                current = CreateUnitOfWork<T>();
                _unitsOfWorks[type] = current;
            }

            return (IUnitOfWork<T>)current;
        }

        private IUnitOfWork<T> CreateUnitOfWork<T>() where T : class
        {
            if (CheckImplementationOfIBaseEntity(typeof(T)))
            {
                return CreateUnitOfWorkEntity<T>();
            }
            if (_mapper == null)
                throw new ArgumentNullException(typeof(T).Name,
                    $"Тип {nameof(T)} не реализует базовый интерфейс сущностей DataLayer {nameof(IBaseEntity)} - требуется указать объект {nameof(IMapper)}");

            var distType = GetDestinationTypeForDataLayer<T>();
            if (distType == null)
                throw new ArgumentNullException(nameof(distType), $"Для {nameof(T)} в объекте {nameof(IMapper)} не указан связанный тип для DataLayer");

            return CreateUnitOfWorkWithMapper<T>(distType);
        }

        private IUnitOfWork<T> CreateUnitOfWorkEntity<T>() where T : class
        {
            var methodGetNewInstanceOfUnitOfWorkEntity = GetType()
                .GetMethod(nameof(GetNewInstanceOfUnitOfWorkEntity), BindingFlags.Instance | BindingFlags.NonPublic);

            return (IUnitOfWork<T>)methodGetNewInstanceOfUnitOfWorkEntity?.MakeGenericMethod(typeof(T))
                .Invoke(this, null);
        }

        private IUnitOfWork<T> CreateUnitOfWorkWithMapper<T>(Type distType) where T : class
        {
            var methodCreateUnitOfWorkMapper = GetType()
                .GetMethod(nameof(GetNewInctanceOfUnitOfWorkWithMapper), BindingFlags.Instance | BindingFlags.NonPublic);
            return (IUnitOfWork<T>)methodCreateUnitOfWorkMapper?.MakeGenericMethod(typeof(T), distType)
                .Invoke(this, null);
        }

        private IUnitOfWork<TEntity> GetNewInstanceOfUnitOfWorkEntity<TEntity>() where TEntity : class, IBaseEntity => 
            new UnitOfWork<TEntity>(_dbContextProvider);
            

        private IUnitOfWork<TDomain> GetNewInctanceOfUnitOfWorkWithMapper<TDomain, TEntity>() 
            where TDomain : class
            where TEntity : class, IBaseEntity
        {
            IUnitOfWork<TEntity> unitOfWorkEntity = GetNewInstanceOfUnitOfWorkEntity<TEntity>();
            IUnitOfWork<TDomain> unitOfWorkWithMapper = new UnitOfWorkMapperDecorator<TDomain,TEntity>(unitOfWorkEntity, _mapper);
            return unitOfWorkWithMapper;
        }

        private Type GetDestinationTypeForDataLayer<T>() where T : class
        {
            TypeMap[] typeMaps = _mapper.ConfigurationProvider.GetAllTypeMaps();
             var distType = typeMaps.FirstOrDefault(t =>
                    t.SourceType == typeof(T)
                    && CheckImplementationOfIBaseEntity(t.DestinationType))
                ?.DestinationType;
            return distType;
        }

        private bool CheckImplementationOfIBaseEntity(Type type) => type.GetInterfaces().Any(t => t == typeof(IBaseEntity));
    }
}

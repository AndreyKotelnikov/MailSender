using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CodeFirstDbContext;
using CodeFirstDbContext.Abstract;
using Entities;
using Entities.Abstract;
using RepositoryAbstract;

namespace Repository
{
    public sealed class UnitOfWorkFactory: IUnitOfWorkFactory
    {
        private readonly IDbContextProvider _dbContextProvider;

        private readonly IMapper _mapper;

        private readonly ConcurrentDictionary<Type, object> _unitsOfWorks = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Конструктор для создания фабрики UnitOfWork
        /// </summary>
        /// <param name="mapper">Класс для меппинга между слоями архитектуры</param>
        /// <param name="typeDbContext">Тип класса для подключения к базе данных</param>
        public UnitOfWorkFactory(IMapper mapper, IDbContextProvider dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// Конструктор для создания фабрики UnitOfWork
        /// </summary>
        /// <param name="mapper">Класс для меппинга между слоями архитектуры</param>
        public UnitOfWorkFactory(IMapper mapper):this(mapper, null)
        {
        }

        public IUnitOfWork<TUpLayer> GetCurrentUnitOfWork<TUpLayer>() where TUpLayer : class, IUniqueEntity
        {
            var type = typeof(TUpLayer);
            object current;

            if (!_unitsOfWorks.TryGetValue(type, out current))
            {
                current = CreateUnitOfWork<TUpLayer>();
                _unitsOfWorks[type] = current;
            }

            return (IUnitOfWork<TUpLayer>)current;
        }

        private IUnitOfWork<TUpLayer> CreateUnitOfWork<TUpLayer>() where TUpLayer : class, IUniqueEntity
        {
            IUnitOfWork<TUpLayer> baseUnitOfWork;
            if (_mapper == null)
            {
                baseUnitOfWork = GetNewInstanceOfUnitOfWorkBase<TUpLayer>();
            }
            else
            {
                var distType = GetDestinationTypeForDownLayer<TUpLayer>();

                baseUnitOfWork = CreateUnitOfWorkWithMapper<TUpLayer>(distType);
            }
            
            return CreateUnitOfWorkWithDecorators(baseUnitOfWork);
        }

        private IUnitOfWork<T> CreateUnitOfWorkWithDecorators<T>(IUnitOfWork<T> baseUnitOfWork) where T : class, IUniqueEntity
        {
            var unitOfWork = new UnitOfWorkContractDecorator<T>(baseUnitOfWork);
            return unitOfWork;
        }


        private IUnitOfWork<TUpLayer> CreateUnitOfWorkWithMapper<TUpLayer>(Type downLayerType) where TUpLayer : class
        {
            var methodCreateUnitOfWorkMapper = GetType()
                .GetMethod(nameof(GetNewInstanceOfUnitOfWorkWithMapper), BindingFlags.Instance | BindingFlags.NonPublic);
            return (IUnitOfWork<TUpLayer>)methodCreateUnitOfWorkMapper?.MakeGenericMethod(typeof(TUpLayer), downLayerType)
                .Invoke(this, null);
        }

        private IUnitOfWork<T> GetNewInstanceOfUnitOfWorkBase<T>() where T : class, IUniqueEntity
        {
            if (_dbContextProvider == null)
            {
                return null;
            }
            return new UnitOfWorkEF<T>(_dbContextProvider, new ResolveOptimisticConcurrencyExceptionsAsClientPriority());
        }  
            
        private IUnitOfWork<TUpLayer> GetNewInstanceOfUnitOfWorkWithMapper<TUpLayer, TDownLayer>() 
            where TUpLayer : class
            where TDownLayer : class, IUniqueEntity
        {
            IUnitOfWork<TDownLayer> unitOfWorkBase = GetNewInstanceOfUnitOfWorkBase<TDownLayer>();
            IUnitOfWork<TUpLayer> unitOfWorkWithMapper = new UnitOfWorkMapperDecorator<TUpLayer,TDownLayer>(unitOfWorkBase, _mapper);
            return unitOfWorkWithMapper;
        }

        private Type GetDestinationTypeForDownLayer<TUpLayer>() where TUpLayer : class
        {
            TypeMap[] typeMaps = _mapper.ConfigurationProvider.GetAllTypeMaps();
             var distType = typeMaps.Single(t => t.SourceType == typeof(TUpLayer))
                .DestinationType;
            return distType;
        }
    }
}

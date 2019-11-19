using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeFirstDbContext.Abstract
{
    /// <summary>
    /// Интерфейс для работы с подключением к базе данных
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Возвращает экземпляр для доступа к сущностям данного типа
        /// </summary>
        /// <typeparam name="TEntity">Тип сущностей</typeparam>
        /// <returns>Возвращает экземпляр для доступа к сущностям данного типа</returns>
        IQueryable<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Сохраняет изменения в базе данных и возвращает количество применённых изменений.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Возвращает количество применённых изменений</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Прикрепить сущность к сущностной модели данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="entity">Сущность, которую нужно прикрепить к сущностной модели данных</param>
        void Attach<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Помечает сущность как новую в сущностной моделе данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="entity">Сущность, которую нужно пометить как новую в сущностной моделе данных</param>
        void Add<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Помечает сущность как изменённую в сущностной моделе данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="entity">Сущность, которую нужно пометить как изменённую в сущностной моделе данных</param>
        void Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Помечает сущность как удалённую в сущностной моделе данных
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="entity">Сущность, которую нужно пометить как удалённую в сущностной моделе данных</param>
        void Remove<TEntity>(TEntity entity) where TEntity : class;
    }
}

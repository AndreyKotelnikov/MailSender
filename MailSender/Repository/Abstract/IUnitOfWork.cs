using System.Threading;
using System.Threading.Tasks;
using Entities.Abstract;

namespace Repository.Abstract
{
    /// <summary>
    /// Интерфейс для изменения сущностей в базе данных
    /// </summary>
    /// <typeparam name="TEntity">Тип изменяемой сущности</typeparam>
    public interface IUnitOfWork<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Добавляет новый элемент в базу данных и возвращает новый Id элемента. Или возвращает 0, если не получилось добавить элемент.
        /// </summary>
        /// <param name="entity">Новый элемент, где Id = 0</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Новое значение Id. Или 0, если не получилось добавить элемент.</returns>
        Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Изменяет текущий элемент в базе данных и возвращает true, если изменение прошло успешно. Или возвращает false, если не получилось изменить элемент.
        /// </summary>
        /// <param name="entity">Элемент, где Id > 0</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>true, если изменение прошло успешно. Или false, если не получилось изменить элемент.</returns>
        Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет текущий элемент из базы данных и возвращает true, если удаление прошло успешно. Или возвращает false, если не получилось удалить элемент.
        /// </summary>
        /// <param name="entity">Элемент, где Id > 0</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>true, если удаление прошло успешно. Или false, если не получилось удалить элемент.</returns>
        Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    }
}
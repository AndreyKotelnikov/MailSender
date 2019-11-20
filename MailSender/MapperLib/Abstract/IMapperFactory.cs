using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace MapperLib.Abstract
{
    /// <summary>
    /// Создаёт карту для меппинга полей DTO
    /// </summary>
    public interface IMapperFactory
    {
        /// <summary>
        /// Возвращает Mapper для указанного типа. Указывается тип объекта более высокого слоя.
        /// Примеры: если нам нужен Mapper между объектами Doman и Entity - то в TSource указываем тип из Domain.
        /// Если нам нужен Mapper между объектами Model и Doman - то в TSource указываем тип из Model.
        /// </summary>
        /// <typeparam name="TSource">Тип объекта более высокого слоя.</typeparam>
        /// <returns>Возвращает Mapper для указанного типа</returns>
        IMapper GetMapper<TSource>() where TSource : class;
    }
}

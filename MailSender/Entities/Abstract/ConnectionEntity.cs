﻿namespace Entities.Abstract
{
    public abstract class ConnectionEntity : NamedEntity
    {
        /// <summary>
        /// Адрес для связи
        /// </summary>
        public string ConnectAdress { get; set; }

        /// <summary>
        /// Дополнительное описание
        /// </summary>
        public string Description { get; set; }
    }
}

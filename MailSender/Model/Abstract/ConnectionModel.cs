using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Abstract
{
    public abstract class ConnectionModel : NamedModel
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Abstract
{
    public abstract class ConnectionModel : NamedModel
    {
        /// <summary>
        /// Дополнительное описание
        /// </summary>
        [DisplayName("Комментарий")]
        public string Description { get; set; }

        /// <summary>
        /// Адрес для связи
        /// </summary>
        [DisplayName("Адрес для связи")]
        public string ConnectAdress { get; set; }
    }
}

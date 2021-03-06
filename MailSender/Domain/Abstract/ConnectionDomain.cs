﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Abstract
{
    public abstract class ConnectionDomain : NamedDomain
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

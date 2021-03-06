﻿using System.Collections.Generic;
using Entities.Abstract;

namespace Entities
{
    public class ServerEntity : ConnectionEntity
    {
        public int Port { get; set; }

        public bool UseSSL { get; set; } = true;

        public string UserName { get; set; }

        public virtual ICollection<SchedulerTaskEntity> SchedulerTaskEntity { get; set; }
    }
}

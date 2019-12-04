using System.Collections.Generic;
using Domains.Abstract;

namespace Domains
{
    public class ServerDomain : ConnectionDomain
    {
        public int Port { get; set; }

        public bool UseSSL { get; set; } = true;

        public string UserName { get; set; }

        public virtual ICollection<SchedulerTaskDomain> SchedulerTaskDomain { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains.Abstract;

namespace Domains
{
    public class MailMessageDomain : BaseDomain
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public virtual ICollection<SchedulerTaskDomain> SchedulerTaskDomain { get; set; }
    }
}

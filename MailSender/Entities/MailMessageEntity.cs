using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class MailMessageEntity : BaseEntity
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public virtual ICollection<SchedulerTaskEntity> SchedulerTaskEntity { get; set; }
    }
}

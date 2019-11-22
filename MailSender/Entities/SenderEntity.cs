using System.Collections.Generic;
using Entities.Abstract;

namespace Entities
{
    public class SenderEntity : ConnectionEntity
    {
        public virtual ICollection<SchedulerTaskEntity> SchedulerTaskEntity { get; set; }
    }
}

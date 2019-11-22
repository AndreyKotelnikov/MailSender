using System.Collections.Generic;
using Entities.Abstract;

namespace Entities
{
    public class RecipientsListEntity : NamedEntity
    {
        public virtual ICollection<RecipientEntity> RecipientEntities { get; set; }

        public virtual ICollection<SchedulerTaskEntity> SchedulerTaskEntity { get; set; }
    }
}
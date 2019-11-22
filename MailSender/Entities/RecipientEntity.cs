using System.Collections.Generic;
using Entities.Abstract;

namespace Entities
{
    public class RecipientEntity : ConnectionEntity
    {
        public virtual ICollection<RecipientsListEntity> RecipientsListEntities { get; set; }
    }
}

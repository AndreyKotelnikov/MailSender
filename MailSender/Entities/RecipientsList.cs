using System.Collections.Generic;
using Entities.Abstract;

namespace Entities
{
    public class RecipientsList : NamedEntity
    {
        public virtual ICollection<Recipient> Recipients { get; set; }
    }
}
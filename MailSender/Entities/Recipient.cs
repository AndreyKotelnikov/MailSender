using System.Collections.Generic;
using Entities.Abstract;

namespace Entities
{
    public class Recipient : ConnectionEntity
    {
        public virtual ICollection<RecipientsList> Lists { get; set; }
    }
}

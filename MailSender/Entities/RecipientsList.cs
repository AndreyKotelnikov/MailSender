using System.Collections.Generic;
using Entities.Abstract;

namespace Entities
{
    public class RecipientsList : NamedEntity
    {
        public ICollection<int> RecipientsId { get; set; }
    }
}
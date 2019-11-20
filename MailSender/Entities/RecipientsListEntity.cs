using System.Collections.Generic;
using Entities.Abstract;

namespace Entities
{
    public class RecipientsListEntity : NamedEntity
    {
        public ICollection<int> RecipientsId { get; set; }
    }
}
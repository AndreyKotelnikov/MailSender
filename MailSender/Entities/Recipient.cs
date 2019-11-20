using System.Collections.Generic;
using Entities.Abstract;

namespace Entities
{
    public class Recipient : ConnectionEntity
    {
        public ICollection<int> ListsId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;

namespace Domain
{
    public class RecipientsListDomain : NamedDomain
    {
        public ICollection<int> RecipientsId { get; set; }
    }
}

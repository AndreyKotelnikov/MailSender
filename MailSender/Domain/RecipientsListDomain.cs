﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains.Abstract;

namespace Domains
{
    public class RecipientsListDomain : NamedDomain
    {
        public ICollection<int> RecipientsId { get; set; }
    }
}

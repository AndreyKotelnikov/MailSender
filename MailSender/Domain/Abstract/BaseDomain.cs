﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Abstract
{
    public abstract class BaseDomain : IBaseDomain
    {
        public int Id { get; set; }
    }
}

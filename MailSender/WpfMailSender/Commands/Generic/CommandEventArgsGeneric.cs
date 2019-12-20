using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSender.Commands.Generic
{
    public class CommandEventArgsGeneric<TParameter> : EventArgs
    {
        public TParameter Parameter { get; set; }
    }
}

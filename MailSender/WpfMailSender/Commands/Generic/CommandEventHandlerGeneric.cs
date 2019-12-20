using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSender.Commands.Generic
{
    public delegate void CommandEventHandlerGeneric<TParameter>(
        object sender,
        CommandEventArgsGeneric<TParameter> args);
}

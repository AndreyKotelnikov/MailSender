using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSender.Commands.Generic
{
    public delegate void CancelCommandEventHandlerGeneric<TParameter>(
        object sender,
        CancelCommandEventArgsGeneric<TParameter> args);
}

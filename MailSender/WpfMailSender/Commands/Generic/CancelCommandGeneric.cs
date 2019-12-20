using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSender.Commands.Generic
{
    public class CancelCommandGeneric<TParameter> : BaseCommandGeneric<TParameter>
    {
        public CancelCommandGeneric(Action<TParameter> parameterizedAction, bool canExecute = true) : base(parameterizedAction, canExecute)
        {
        }
    }
}

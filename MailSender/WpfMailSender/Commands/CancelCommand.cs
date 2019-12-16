using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSender.Commands
{
    public class CancelCommand : BaseCommand
    {
        public CancelCommand(Action action, bool canExecute = true) : base(action, canExecute)
        {
        }

        public CancelCommand(Action<object> parameterizedAction, bool canExecute = true) : base(parameterizedAction, canExecute)
        {
        }
    }
}

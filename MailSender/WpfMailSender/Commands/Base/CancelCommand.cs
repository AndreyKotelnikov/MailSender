using System;

namespace WpfMailSender.Commands.Base
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

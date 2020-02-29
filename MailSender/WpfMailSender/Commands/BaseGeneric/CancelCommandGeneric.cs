using System;

namespace WpfMailSender.Commands.BaseGeneric
{
    public class CancelCommandGeneric<TParameter> : BaseCommandGeneric<TParameter>
    {
        public CancelCommandGeneric(Action<TParameter> parameterizedAction, Func<TParameter, bool> canExecuteFunc) : base(parameterizedAction, null)
        {
        }
    }
}

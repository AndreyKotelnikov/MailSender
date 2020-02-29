using System;

namespace WpfMailSender.Commands.BaseGeneric
{
    public class CommandEventArgsGeneric<TParameter> : EventArgs
    {
        public TParameter Parameter { get; set; }
    }
}

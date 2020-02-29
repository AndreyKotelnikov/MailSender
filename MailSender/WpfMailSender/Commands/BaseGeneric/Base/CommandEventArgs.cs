using System;

namespace WpfMailSender.Commands.BaseGeneric.Base
{
    public class CommandEventArgs : EventArgs
    {
        public object Parameter { get; set; }
    }
}

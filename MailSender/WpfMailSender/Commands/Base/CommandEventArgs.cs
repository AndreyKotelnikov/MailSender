using System;

namespace WpfMailSender.Commands.Base
{
    public class CommandEventArgs : EventArgs
    {
        public object Parameter { get; set; }
    }
}

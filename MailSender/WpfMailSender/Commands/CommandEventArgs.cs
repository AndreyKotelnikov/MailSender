using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSender.Commands
{
    public class CommandEventArgs : EventArgs
    {
        public object Parameter { get; set; }
    }
}

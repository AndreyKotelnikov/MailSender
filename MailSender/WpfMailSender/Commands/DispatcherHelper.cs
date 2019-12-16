using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSender.Commands
{
    public static class DispatcherHelper
    {
        public static System.Windows.Threading.Dispatcher CurrentDispatcher { get; }
    }
}

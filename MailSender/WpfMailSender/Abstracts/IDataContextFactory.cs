using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMailSender.ViewModels;

namespace WpfMailSender.Abstracts
{
    public interface IDataContextFactory
    {
        object Create(IWindow window);
    }
}

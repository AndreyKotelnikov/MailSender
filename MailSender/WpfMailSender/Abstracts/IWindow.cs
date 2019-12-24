using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSender.Abstracts
{
    public interface IWindow
    {
        void Close();
        IWindow CreateChild(object viewModel);
        void Show();
        bool? ShowDialog();
    }
}

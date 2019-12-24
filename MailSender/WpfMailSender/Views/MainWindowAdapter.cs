using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfMailSender.Abstracts;

namespace WpfMailSender.Views
{
    public class MainWindowAdapter : IWindow
    {
        private readonly MainWindow _mainWindow;
        private readonly IMainWindowViewModelFactory _vmFactory;

        public MainWindowAdapter(MainWindow mainWindow, IMainWindowViewModelFactory vmFactory)
        {
            _mainWindow = mainWindow;
            _vmFactory = vmFactory;

            var vm = _vmFactory.Create(this);
            _mainWindow.DataContext = vm;
        }

        void IWindow.Close()
        {
            throw new NotImplementedException();
        }

        IWindow IWindow.CreateChild(object viewModel)
        {
            throw new NotImplementedException();
        }

        void IWindow.Show() => _mainWindow.Show();
        

        bool? IWindow.ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}

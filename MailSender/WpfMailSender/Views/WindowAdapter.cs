using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfMailSender.Abstracts;

namespace WpfMailSender.Views
{
    public class WindowAdapter : IWindow
    {
        private readonly Window _window;

        public WindowAdapter(Window window, IDataContextFactory dataContextFactory)
        {
            _window = window 
                          ?? throw new ArgumentNullException(nameof(window));
            _window.DataContext = dataContextFactory
                ?.Create(this);
        }

        void IWindow.Close()
        {
            throw new NotImplementedException();
        }

        IWindow IWindow.CreateChild<T>(T viewModel)
        {
            throw new NotImplementedException();
        }

        void IWindow.Show() => _window.Show();
        

        bool? IWindow.ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}

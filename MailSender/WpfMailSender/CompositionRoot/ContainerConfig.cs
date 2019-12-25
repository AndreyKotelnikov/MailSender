using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using RepositoryAbstract;
using WpfMailSender.Abstracts;
using WpfMailSender.ViewModels;
using WpfMailSender.Views;

namespace WpfMailSender.CompositionRoot
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindowDataContextFactory>().As<IDataContextFactory>();
            builder.RegisterType<MainWindow>().As<Window>();
            builder.RegisterType<WindowAdapter>().As<IWindow>();

            return builder.Build();
        }
    }
}

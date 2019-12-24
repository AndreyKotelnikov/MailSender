using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            builder.RegisterType<MainWindowViewModelFactory>().As<IMainWindowViewModelFactory>();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowAdapter>().As<IWindow>();

            return builder.Build();
        }
    }
}

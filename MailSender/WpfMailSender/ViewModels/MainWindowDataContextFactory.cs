using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryAbstract;
using WpfMailSender.Abstracts;

namespace WpfMailSender.ViewModels
{
    public class MainWindowDataContextFactory : IDataContextFactory
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        //TODO Сделать связку со слоем бизнесс-логики в VMFactory
        //public MainWindowDataContextFactory(IUnitOfWorkFactory unitOfWorkFactory)
        //{
        //    _unitOfWorkFactory = unitOfWorkFactory;
        //}


        public object Create(IWindow window)
        {
            return new MainWindowViewModel(window);
        }
    }
}

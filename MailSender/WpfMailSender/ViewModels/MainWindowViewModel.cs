using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Models;
using Models.Abstract;
using RepositoryAbstract;
using WpfMailSender.Utils;
using Type = System.Type;

namespace WpfMailSender.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region private

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private string _title = "Рассыльщик почты";

        private string _status = "Готов к работе";

        private ObservableDictionary<Type, ObservableCollection<IBaseModel>> _models = 
            new ObservableDictionary<Type, ObservableCollection<IBaseModel>>();

        private ObservableDictionary<Type, object> _selectedItem = 
            new ObservableDictionary<Type, object>();

        private ObservableDictionary<Type, int> _selectedIndex = 
            new ObservableDictionary<Type, int>();

        #endregion

        #region properties

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        public ObservableDictionary<Type, ObservableCollection<IBaseModel>> Models
        {
            get => _models;
            set => Set(ref _models, value);
        }

        public ObservableDictionary<Type, object> SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        public ObservableDictionary<Type, int> SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value);
        }

        #endregion

        #region constructors

        public MainWindowViewModel()
        {
            RegistryTypeModel(typeof(RecipientModel));
            RegistryTypeModel(typeof(RecipientsListModel));
            RegistryTypeModel(typeof(SenderModel));
            RegistryTypeModel(typeof(ServerModel));
            RegistryTypeModel(typeof(SchedulerTaskModel));
            RegistryTypeModel(typeof(MailMessageModel));
            UpdateData();
        }

        //public MainWindowViewModel(IUnitOfWorkFactory unitOfWorkFactory = null)
        //{
        //    _unitOfWorkFactory = unitOfWorkFactory;
        //}

        #endregion


        #region methods

        private void RegistryTypeModel(Type typeModel)
        {
            if (typeModel.GetInterfaces().All(i => i != typeof(IBaseModel))) 
                throw new ArgumentException($"Тип {nameof(typeModel)} не содержит реализацию интерфейса {nameof(IBaseModel)}");

            Models.Add(typeModel, new ObservableCollection<IBaseModel>());
            SelectedItem.Add(typeModel, null);
            SelectedIndex.Add(typeModel, -1);
        }

        private void UpdateData()
        {
            Parallel.ForEach(Models.Keys, 
                typeModel => Models[typeModel] = TestDataCreater.CreateTestData(typeModel));
        }

        #endregion


        
    }
}

﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Models;
using Models.Abstract;
using RepositoryAbstract;
using WpfMailSender.Abstracts;
using WpfMailSender.Commands;
using WpfMailSender.Utils;
using WpfMailSender.Views;
using Type = System.Type;

namespace WpfMailSender.ViewModels
{
    
    public class MainWindowViewModel : ViewModelBase, IViewModelSellectedItems, IViewModelCollectionsOfModels
    {
        #region private

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private readonly IWindow _window; 

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

        public ICommand EditItemCommand => new EditItemCommand(SelectedItem, Models);

        #endregion

        #region constructors

        /// <summary>
        /// Only for design Mode
        /// </summary>
        public MainWindowViewModel() : this(null) { }

        public MainWindowViewModel(IWindow window)
        {
            _window = window;

            RegistryTypeModel(typeof(RecipientModel));
            RegistryTypeModel(typeof(RecipientsListModel));
            RegistryTypeModel(typeof(SenderModel));
            RegistryTypeModel(typeof(ServerModel));
            RegistryTypeModel(typeof(SchedulerTaskModel));
            RegistryTypeModel(typeof(MailMessageModel));
            UpdateData();
        }

        //TODO Сделать связку со слоем бизнесс-логики в VM
        //public MainWindowViewModel(IUnitOfWorkFactory unitOfWorkFactory = null)
        //{
        //    _unitOfWorkFactory = unitOfWorkFactory;
        //}

        #endregion


        #region methods

        private void RegistryTypeModel(Type typeModel)
        {
            if (typeModel.GetInterfaces().All(i => i != typeof(IBaseModel))) 
                throw new ArgumentException($"Тип {typeModel} не содержит реализацию интерфейса {nameof(IBaseModel)}");

            if(Models.ContainsKey(typeModel)) 
                throw new ArgumentException($"Тип {typeModel} уже зарегистрирован");

            Models.Add(typeModel, new ObservableCollection<IBaseModel>());
            SelectedItem.Add(typeModel, null);
            SelectedIndex.Add(typeModel, -1);
        }

        private void UpdateData()
        {
            foreach (var typeModel in Models.Keys)
            {
                Models[typeModel] = TestDataCreater.CreateTestData(typeModel);
            }
        }

        #endregion
    }
}

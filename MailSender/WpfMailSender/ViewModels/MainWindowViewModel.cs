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

namespace WpfMailSender.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private string _title = "Рассыльщик почты";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private string _status = "Готов к работе";

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }


        private ObservableDictionary<Type, ObservableCollection<IBaseModel>> _models = 
            new ObservableDictionary<Type, ObservableCollection<IBaseModel>>();

        private ObservableDictionary<Type, object> _selectedItem =
            new ObservableDictionary<Type, object>();

        private ObservableDictionary<Type, int> _selectedIndex =
            new ObservableDictionary<Type, int>();

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

        public MainWindowViewModel()
        {
            RegistryTypeModel(typeof(RecipientModel));
            RegistryTypeModel(typeof(SenderModel));
        }

        //public MainWindowViewModel(IUnitOfWorkFactory unitOfWorkFactory = null)
        //{
        //    _unitOfWorkFactory = unitOfWorkFactory;
        //}


        private void RegistryTypeModel(Type typeModel)
        {
            if (typeModel.GetInterfaces().All(i => i != typeof(IBaseModel))) 
                throw new ArgumentException($"Тип {nameof(typeModel)} не содержит реализацию интерфейса {nameof(IBaseModel)}");

            Models.Add(typeModel, new ObservableCollection<IBaseModel>());
            
            Task.Run(() =>
            {
                Thread.Sleep(10000);
                Models[typeModel] = CreateTestData(typeModel);
            });

            SelectedItem.Add(typeModel, null);
            SelectedIndex.Add(typeModel, 0);
        }

        private ObservableCollection<IBaseModel> CreateTestData(Type typeOfData)
        {
            var methodGetTestModels = GetType()
                .GetMethod(nameof(GetTestDataGeneric), BindingFlags.Instance | BindingFlags.NonPublic);
            return (ObservableCollection<IBaseModel>)methodGetTestModels?.MakeGenericMethod(typeOfData)
                .Invoke(this, null);
        }

        private ObservableCollection<IBaseModel> GetTestDataGeneric<T>() where T : class, IBaseModel, new()
        {
            T[] models = Enumerable.Range(1, 10).Select(m => Activator.CreateInstance<T>()).ToArray();
            if (typeof(T).BaseType == typeof(ConnectionModel))
            {
                for (var i = 0; i < models.Length; i++)
                {
                    var connModel = models[i] as ConnectionModel;
                    connModel.Id = i + 1;
                    connModel.Name = $"{typeof(T).Name.Replace("Model", string.Empty)} {i + 1}";
                    connModel.ConnectAdress = $"server_{i + 1}@mail.ru";
                    connModel.Description = i % 3 == 0 ? $"Комментарий {i + 1}" : null;
                }
            }
            else 
            if (typeof(T).BaseType == typeof(NamedModel))
            {
                for (var i = 0; i < models.Length; i++)
                {
                    var namedModel = models[i] as NamedModel;
                    namedModel.Id = i + 1;
                    namedModel.Name = $"{typeof(T).Name.Replace("Model", string.Empty)} {i + 1}";
                }
            }
            else
            {
                for (var i = 0; i < models.Length; i++)
                {
                    var namedModel = models[i] as BaseModel;
                    namedModel.Id = i + 1;
                }
            }

            var collection = new ObservableCollection<IBaseModel>(models);
            
            return collection;
        }
    }
}

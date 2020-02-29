using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Models;
using Models.Abstract;
using WpfMailSender.Abstracts;
using WpfMailSender.Utils;

namespace WpfMailSender.ViewModels
{
    public class EditItemsViewModel : ViewModelBase, IViewModelSellectedItems, IViewModelCollectionsOfModels
    {
        private IEnumerable<IBaseModel> _itemsForEdit;
        
        private string _title = "Редактирование элемента";

        private object _selectedItemObject;
        private ObservableDictionary<Type, object> _selectedItemCollection;
        private ObservableDictionary<Type, ObservableCollection<IBaseModel>> _models;

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public IEnumerable<IBaseModel> ItemsForEdit
        {
            get => _itemsForEdit;
            set => Set(ref _itemsForEdit, value);
        }

        public object SelectedItemObject
        {
            get => _selectedItemObject;
            set => Set(ref _selectedItemObject, value);
        }

        public ObservableDictionary<Type, object> SelectedItem => _selectedItemCollection;

        public ObservableDictionary<Type, ObservableCollection<IBaseModel>> Models => _models;

        public EditItemsViewModel(IEnumerable<IBaseModel> itemsForEdit, 
            ObservableDictionary<Type, object> selectedItemCollection, 
            ObservableDictionary<Type, ObservableCollection<IBaseModel>> models
            )
        {
            _itemsForEdit = itemsForEdit;
            _selectedItemObject = itemsForEdit.First();
            _selectedItemCollection = selectedItemCollection;
            _models = models;
        }

        /// <summary>
        /// Only for design Mode
        /// </summary>
        public EditItemsViewModel() : this(TestDataCreater.CreateTestData(typeof(RecipientModel)), null, null)
        {
        }

        

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using Models.Abstract;
using WpfMailSender.Commands.BaseGeneric;
using WpfMailSender.Components;
using WpfMailSender.Utils;
using WpfMailSender.ViewModels;
using WpfMailSender.Views;

namespace WpfMailSender.Commands
{
    public class EditItemCommand : BaseCommandGeneric<IBaseModel>, ICommand
    {
        private ObservableDictionary<Type, object> _selectedItemCollection;
        private ObservableDictionary<Type, ObservableCollection<IBaseModel>> _models;

        public EditItemCommand(
            ObservableDictionary<Type, object> selectedItemCollection, 
            ObservableDictionary<Type, ObservableCollection<IBaseModel>> models
            )
        {
            _selectedItemCollection = selectedItemCollection;
            _models = models;
            ParameterizedAction = EditSellectedItem;
            CanExecuteFunc = CanExecute;
        }

        private bool CanExecute(IBaseModel selectedItem)
        {
            return selectedItem != null;
        }

        private void EditSellectedItem(IBaseModel selectedItem)
        {
            var editWindow = new EditItemsWindow
            {
                DataContext = new EditItemsViewModel(new []{ selectedItem}, _selectedItemCollection, _models)
            };
            editWindow.ShowDialog();
        }

         
    }
}

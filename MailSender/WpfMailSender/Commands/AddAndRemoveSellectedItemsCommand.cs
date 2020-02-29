using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Models.Abstract;
using WpfMailSender.Commands.BaseGeneric;
using WpfMailSender.Components;
using WpfMailSender.Utils;

namespace WpfMailSender.Commands
{
    public class AddAndRemoveSellectedItemsCommand : BaseCommandGeneric<(IEnumerable<IBaseModel>, IEnumerable<IBaseModel>)>
    {
        private readonly ObservableDictionary<Type, object> _sellectedCollection;
        private readonly Type _typeOfSellectedItem;
        private readonly PropertyInfo _propertyInfo;
        private readonly ButtonForAddAndRemoveSellectedItems _button;

        public AddAndRemoveSellectedItemsCommand(
            ObservableDictionary<Type, object> sellectedCollection, 
            Type typeOfSellectedItem, 
            string propertyName, 
            ButtonForAddAndRemoveSellectedItems button
            )
        {
            _sellectedCollection = sellectedCollection;
            _typeOfSellectedItem = typeOfSellectedItem;
            _button = button;
            _propertyInfo = typeOfSellectedItem.GetProperty(propertyName);
            ParameterizedAction = AddAndRemoveSellectedItems;
            CanExecuteFunc = CanExecute;
        }

        private bool CanExecute((IEnumerable<IBaseModel>, IEnumerable<IBaseModel>) collectionsWithSelectedItems)
        {
            (IEnumerable<IBaseModel> itemsForRemove, IEnumerable<IBaseModel> itemsForAdd) = collectionsWithSelectedItems;
            return itemsForAdd.Any() || itemsForRemove.Any();
        }

        private void AddAndRemoveSellectedItems((IEnumerable<IBaseModel>, IEnumerable<IBaseModel>) collectionsWithSelectedItems)
        {
            (IEnumerable<IBaseModel> itemsForRemove, IEnumerable<IBaseModel> itemsForAdd) = collectionsWithSelectedItems;
            
            var sellectedItem = _sellectedCollection[_typeOfSellectedItem];
            var propertyValueOfSellectedItem = _propertyInfo.GetValue(sellectedItem) as IList;
            var arrowDirection = (ArrowDirectionEnum)_button.GetValue(ButtonForAddAndRemoveSellectedItems.ArrowDirectionProperty);

            if (arrowDirection == ArrowDirectionEnum.Down)
            {
                RemoveItems(itemsForRemove, propertyValueOfSellectedItem);
            }

            if (arrowDirection == ArrowDirectionEnum.Up)
            {
                AddItems(itemsForAdd, propertyValueOfSellectedItem, sellectedItem);
            }
        }

        private void RemoveItems(IEnumerable<IBaseModel> itemsForRemove, IList propertyValueOfSellectedItem)
        {
            foreach (var itemForRemove in itemsForRemove)
            {
                var innerItemForRemove = propertyValueOfSellectedItem.Cast<IBaseModel>()
                    .First(i => i.Id == itemForRemove.Id);
                propertyValueOfSellectedItem.Remove(innerItemForRemove);
            }
        }

        private void AddItems(IEnumerable<IBaseModel> itemsForAdd, IList propertyValueOfSellectedItem, object sellectedItem)
        {
            if (propertyValueOfSellectedItem == null)
            {
                propertyValueOfSellectedItem = CollectionElementTypeConverter.CreateEmptyList(
                    _propertyInfo.PropertyType
                        .GetGenericArguments()
                        .Single()
                    );
                _propertyInfo.SetValue(sellectedItem, propertyValueOfSellectedItem);
            }
            foreach (var itemForAdd in itemsForAdd)
            {
                propertyValueOfSellectedItem.Add(itemForAdd);
            }
        }
    }
}

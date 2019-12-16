using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Models.Abstract;
using WpfMailSender.Commands.Base;
using WpfMailSender.Utils;

namespace WpfMailSender.Commands
{
    public class AddAndRemoveSellectedItemsCommand : BaseCommand
    {
        private readonly ObservableDictionary<Type, object> _sellectedCollection;
        private readonly Type _typeOfSellectedItem;
        private readonly PropertyInfo _propertyInfo;


        public AddAndRemoveSellectedItemsCommand(ObservableDictionary<Type, object> sellectedCollection, Type typeOfSellectedItem, string propertyName)
        {
            _sellectedCollection = sellectedCollection;
            _typeOfSellectedItem = typeOfSellectedItem;
            _propertyInfo = typeOfSellectedItem.GetProperty(propertyName);
            ParameterizedAction = AddAndRemoveSellectedItems;
        }


        public void AddAndRemoveSellectedItems(object valueTuple)
        {
            (IEnumerable<IBaseModel> itemsForRemove, IEnumerable<IBaseModel> itemsForAdd) = (ValueTuple<IEnumerable<IBaseModel> , IEnumerable<IBaseModel>>) valueTuple;
            var sellectedItem = _sellectedCollection[_typeOfSellectedItem];

            var propertyValueOfSellectedItem = _propertyInfo.GetValue(sellectedItem) as IList;
            foreach (var itemForRemove in itemsForRemove)
            {
                var innerItemForRemove = propertyValueOfSellectedItem.Cast<IBaseModel>().First(i => i.Id == itemForRemove.Id);
                propertyValueOfSellectedItem.Remove(innerItemForRemove);
            }

            if (propertyValueOfSellectedItem == null)
            {
                propertyValueOfSellectedItem = CollectionElementTypeConvertor.CreateList(_propertyInfo.PropertyType.GetGenericArguments().Single());
                _propertyInfo.SetValue(sellectedItem, propertyValueOfSellectedItem);
            }
            foreach (var itemForAdd in itemsForAdd)
            {
                propertyValueOfSellectedItem.Add(itemForAdd);
            }
        }
    }
}

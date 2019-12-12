using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Models.Abstract;

namespace WpfMailSender.Utils
{
    public class SelectionFilterCreater
    {
        private readonly IEnumerable _sourceCollection;
        private readonly string _propertyName;
        private readonly Type _selectedItemType;
        private readonly bool _isReverseFilter;
        private object _selectedItem;
        private CollectionViewSource _sourceItemsView;

        public SelectionFilterCreater(IEnumerable sourceCollection, string propertyName, Type selectedItemType) 
            : this(sourceCollection, propertyName, selectedItemType, false) { }

        public SelectionFilterCreater(IEnumerable sourceCollection, string propertyName, Type selectedItemType, bool isReverseFilter)
        {
            _sourceCollection = sourceCollection;
            _propertyName = propertyName;
            _selectedItemType = selectedItemType;
            _isReverseFilter = isReverseFilter;
        }

        public ICollectionView GetCollectionWithSelectionFilter(Selector observedSelector)
        {
            _sourceItemsView = new CollectionViewSource { Source = _sourceCollection };
            _sourceItemsView.Filter += Filter;
            observedSelector.SelectionChanged += SelectionChanged;
            return _sourceItemsView.View;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedList = (e.AddedItems as IList<object>);
            if (selectedList?.Count == 1)
            {
                _selectedItem = selectedList.FirstOrDefault(i => i.GetType() == _selectedItemType);
                if (_selectedItem != null)
                {
                    _sourceItemsView.View.Refresh();
                }
            }
        }

        private void Filter(object sender, FilterEventArgs e)
        {
            if (e.Item is IBaseModel obj 
                && _selectedItem != null)
            {
                var propertyInfo = _selectedItem.GetType().GetProperty(_propertyName);
                if (propertyInfo == null)
                    throw new ArgumentNullException(nameof(propertyInfo), $"Свойство с именем {_propertyName} в выбранном элементе не найдено");

                var propertyCollection = (IEnumerable<IBaseModel>)propertyInfo.GetValue(_selectedItem);
                if (propertyCollection != null)
                {
                    e.Accepted = _isReverseFilter 
                        ? propertyCollection.All(p => p.Id != obj.Id)
                        : propertyCollection.Any(p => p.Id == obj.Id);
                    return;
                }

                e.Accepted = _isReverseFilter;
                return;
            }
            e.Accepted = false;
        }
    }
}

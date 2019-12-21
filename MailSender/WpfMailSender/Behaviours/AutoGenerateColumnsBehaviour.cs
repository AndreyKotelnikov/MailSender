using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using MaterialDesignThemes.Wpf;
using Models;
using Models.Abstract;
using WpfMailSender.Abstracts;
using WpfMailSender.Utils;
using WpfMailSender.ViewModels;
using DataGridComboBoxColumn = System.Windows.Controls.DataGridComboBoxColumn;

namespace WpfMailSender.Behaviours
{
    public class AutoGenerateColumnsBehaviour : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.AutoGeneratingColumn += OnAutoGeneratingColumn;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.AutoGeneratingColumn -= OnAutoGeneratingColumn;
        }


        public static void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs eventArgs)
        {
           
            if (sender is DataGrid dataGrid 
                && eventArgs.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                var type = descriptor.PropertyType;
                if (descriptor.IsPropertyDisplayNameNull()
                    || type.IsPropertyEnumerable())
                {
                    eventArgs.Cancel = true;
                    return; 
                }
                
                if (descriptor.IsPropertyImplementedBaseInterface())
                {
                    eventArgs.Column.IsReadOnly = true;
                }

                if (type.IsPropertyModel())
                {
                    var newComboBoxColumn = new DataGridComboBoxColumn();
                    FillPropertiesOfComboBoxColumn(newComboBoxColumn, dataGrid, descriptor);
                    eventArgs.Column = newComboBoxColumn;
                }

                eventArgs.Column.Header = descriptor.DisplayName ?? descriptor.Name;
                eventArgs.Column.Width = DataGridLength.Auto;
            }
            else
            {
                eventArgs.Cancel = true;
            }
            
        }

        private static void FillPropertiesOfComboBoxColumn(DataGridComboBoxColumn newComboBoxColumn, DataGrid dataGrid, PropertyDescriptor descriptor)
        {
            var sourceItems = (dataGrid.DataContext as IViewModelCollectionsOfModelsAndSellectedItems)?.Models[descriptor.PropertyType]
                .Prepend(CreateNullItem(descriptor.PropertyType));

            CollectionElementTypeConverter.SetValueAsConvertToOriginTypeItems(descriptor.PropertyType, sourceItems, "ItemsSource", newComboBoxColumn);

            newComboBoxColumn.DisplayMemberPath = descriptor.PropertyType
                                                      .GetProperties()
                                                      .FirstOrDefault(p => p.Name.ToLower() == "name")
                                                      ?.Name
                                                  ?? string.Empty;

            var valueBinding = new Binding(descriptor.Name)
            {
                Converter = new ValueConverterById(),
                ConverterParameter = newComboBoxColumn.ItemsSource
            };
            newComboBoxColumn.SelectedItemBinding = valueBinding;

            newComboBoxColumn.SortMemberPath = descriptor.Name;
        }

        private static IBaseModel CreateNullItem(Type itemType)
        {
            var nullItem = (IBaseModel)Activator.CreateInstance(itemType);
            nullItem.Id = -1;
            if (nullItem is NamedModel nullItemWithName)
            {
                nullItemWithName.Name = "(Пусто)";
            }
            return nullItem;
        }
    }
}

﻿using System;
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
using WpfMailSender.Utils;
using WpfMailSender.ViewModels;

namespace WpfMailSender.Behaviours
{
    public class AutoGenerateColumnsBehaviour : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.AutoGeneratingColumn += OnGeneratingColumn;
            AssociatedObject.AutoGeneratedColumns += OnAutoGeneratedColumns;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.AutoGeneratingColumn -= OnGeneratingColumn;
            AssociatedObject.AutoGeneratedColumns -= OnAutoGeneratedColumns;
        }


        private static void OnGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs eventArgs)
        {
           
            if (sender is DataGrid dataGrid 
                && eventArgs.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                if (IsPropertyDisplayNameNull(descriptor))
                {
                    eventArgs.Cancel = true;
                    return; 
                }
                
                if (IsPropertyImplementedBaseInterface(descriptor.Name))
                {
                    eventArgs.Column.IsReadOnly = true;
                }

                if (IsPropertyModel(descriptor.PropertyType))
                {
                    var newComboBoxColumn = new DataGridComboBoxColumn();
                    FillPropertiesOfNewComboBoxColumn(newComboBoxColumn, dataGrid, descriptor);
                    eventArgs.Column = newComboBoxColumn;
                }

                if (IsPropertyEnumerableOfModels(descriptor))
                {
                    //TODO Определить логику отображения свойства со связью многие-ко-многим
                }

                eventArgs.Column.Header = descriptor.DisplayName ?? descriptor.Name;
            }
            else
            {
                eventArgs.Cancel = true;
            }
            
        }

        private static bool IsPropertyEnumerableOfModels(PropertyDescriptor descriptor) =>
            descriptor.PropertyType
                .GetInterfaces()
                .Any(i => i == typeof(IEnumerable))
            && (descriptor.PropertyType
                    .GenericTypeArguments
                    .SingleOrDefault()
                    ?.GetInterfaces()
                    .Any(i => i == typeof(IBaseModel))
                ?? false);

        private static void FillPropertiesOfNewComboBoxColumn(DataGridComboBoxColumn newComboBoxColumn, DataGrid dataGrid, PropertyDescriptor descriptor)
        {
            var sourceItems = (dataGrid.DataContext as MainWindowViewModel)?.Models[descriptor.PropertyType]
                .Prepend(CreateNullItem(descriptor.PropertyType));

            var propertyInfoOfItemsSource = newComboBoxColumn.GetType().GetProperty("ItemsSource");
            CollectionElementTypeConvertor.SetValueAsConvertToOriginTypeItems(descriptor.PropertyType, sourceItems, propertyInfoOfItemsSource, newComboBoxColumn);

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

        private static bool IsPropertyModel(Type propertyType) => 
            propertyType.GetInterfaces()
                .Any(i => i == typeof(IBaseModel));

        private static bool IsPropertyDisplayNameNull(PropertyDescriptor descriptor) => 
            (descriptor.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute)
            ?.DisplayName == null;

        private static bool IsPropertyImplementedBaseInterface(string propertyName) => 
            typeof(IBaseModel).GetProperties()
                .Any(p => p.Name == propertyName)
            || typeof(IBaseModel).GetInterfaces()
                .SelectMany(i => i.GetProperties())
                .Any(p => p.Name == propertyName);

        private static IBaseModel CreateNullItem(Type itemType)
        {
            var nullItem = (IBaseModel)Activator.CreateInstance(itemType);
            nullItem.Id = -1;
            return nullItem;
        }

        private void OnAutoGeneratedColumns(object sender, EventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                var count = dataGrid.Columns.Count;
                foreach (var column in dataGrid.Columns)
                {
                    column.DisplayIndex = count - 1;
                    count--;
                }
            }
        }
    }
}

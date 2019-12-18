using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media.Media3D;
using Models.Abstract;
using WpfMailSender.Abstracts;
using WpfMailSender.Commands;
using WpfMailSender.Components;
using WpfMailSender.Utils;
using WpfMailSender.ViewModels;

namespace WpfMailSender.Behaviours
{
    public class GenerateDataGridForPropertiesManyToManyModelsBehaviour : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.AutoGeneratingColumn += OnAutoGeneratingColumn;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.AutoGeneratingColumn -= OnAutoGeneratingColumn;
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs eventArgs)
        {
            if (sender is DataGrid mainDataGrid
                && eventArgs.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                if (descriptor.PropertyType.IsPropertyEnumerableOfModels()
                    && !descriptor.IsPropertyDisplayNameNull())
                {
                    DataGrid dataGridWithPropertyCollection = CreatDataGridWithPropertyCollection(mainDataGrid, descriptor);
                    DataGrid dataGridWithInvertedPropertyCollection = CreatDataGridWithPropertyCollection(mainDataGrid, descriptor, true);
                    var sellectButton = CreatButtonForAddAndRemoveSellectedItems(mainDataGrid, descriptor, dataGridWithPropertyCollection, dataGridWithInvertedPropertyCollection);
                    Grid gridWithSellectButton = PutElementIntoGrid(sellectButton, 0, 1);
                    RenderingUIElements(mainDataGrid, dataGridWithPropertyCollection, gridWithSellectButton, dataGridWithInvertedPropertyCollection);
                }
            }
        }

        private Grid PutElementIntoGrid(UIElement elementForPutInto, int row, int column)
        {
            var grid = CreateGridWithColumns(GridUnitType.Star, 5, 1, 5);
            Grid.SetRow(elementForPutInto, row);
            Grid.SetColumn(elementForPutInto, column);
            grid.Children.Add(elementForPutInto);
            return grid;
        }

        private ButtonForAddAndRemoveSellectedItems CreatButtonForAddAndRemoveSellectedItems
            (DataGrid mainDataGrid, PropertyDescriptor descriptor, DataGrid dataGridForRemoveItems, DataGrid dataGridForAddItems)
        {
            var button = new ButtonForAddAndRemoveSellectedItems();

            dataGridForRemoveItems.SelectionChanged += button.CollectionForRemoveItems_SelectionChanged;
            dataGridForRemoveItems.GotFocus += button.CollectionForRemoveItems_GotFocus;

            dataGridForAddItems.SelectionChanged += button.CollectionForAddItems_SelectionChanged;
            dataGridForAddItems.GotFocus += button.CollectionForAddItems_GotFocus;

            var command = CreateAddAndRemoveSellectedItemsCommand(mainDataGrid, descriptor, button);
            
            button.Command = command;
            button.CommandParameter = (dataGridForRemoveItems.SelectedItems.Cast<IBaseModel>(), dataGridForAddItems.SelectedItems.Cast<IBaseModel>()); 

            return button;
        }

        private AddAndRemoveSellectedItemsCommand CreateAddAndRemoveSellectedItemsCommand(DataGrid mainDataGrid, PropertyDescriptor descriptor, ButtonForAddAndRemoveSellectedItems button)
        {
            var sellectedItemDictionary = (mainDataGrid.DataContext as IViewModelCollectionsOfModelsAndSellectedItems)
                .SelectedItem;
            var command = new AddAndRemoveSellectedItemsCommand(sellectedItemDictionary, descriptor.ComponentType, descriptor.Name, button);
            command.Executed += (s, e) =>
            {
                sellectedItemDictionary.RaisePropertyChangedEventForIndexerName();
            };

            return command;
        }

        private void RenderingUIElements(FrameworkElement mainFrameworkElement, params UIElement[] elementsForRendering)
        {
            Grid grid = CreateGridWithRows(GridUnitType.Star, 5, 1, 5);

            var lastGridColumn = FindLastGridColumn(mainFrameworkElement);
            Grid.SetColumn(grid, lastGridColumn + 1);
            Grid.SetRow(grid, Grid.GetRow(mainFrameworkElement));

            var parent = mainFrameworkElement.Parent as Panel;
            parent.Children.Add(grid);

            for (int i = 0; i < elementsForRendering.Length; i++)
            {
                Grid.SetRow(elementsForRendering[i], i);
                grid.Children.Add(elementsForRendering[i]);
            }
        }

        private int FindLastGridColumn(FrameworkElement mainFrameworkElement)
        {
            var panel = mainFrameworkElement.Parent as Panel;
            if (panel == null) throw new ArgumentNullException(nameof(panel));

            return panel.Children.OfType<UIElement>()
                .Where(e => ((e is DataGrid || ((e as Grid)?.Children.OfType<UIElement>().Count(d => d is DataGrid) == 2))
                             && Grid.GetRow(e) == Grid.GetRow(mainFrameworkElement)))
                .Select(Grid.GetColumn)
                .Max();
        }

        private Grid CreateGridWithRows(GridUnitType gridUnitType, params int[] heightRowsRatio)
        {
            var grid = new Grid();
            foreach (var height in heightRowsRatio)
            {
                var gridLenght = new GridLength(height, gridUnitType);
                var rowDefinition = new RowDefinition();
                rowDefinition.Height = gridLenght;
                grid.RowDefinitions.Add(rowDefinition);
            }

            return grid;
        }

        private Grid CreateGridWithColumns(GridUnitType gridUnitType, params int[] widthColumnsRatio)
        {
            var grid = new Grid();
            foreach (var width in widthColumnsRatio)
            {
                var gridLenght = new GridLength(width, gridUnitType);
                var columnDefinition = new ColumnDefinition();
                columnDefinition.Width = gridLenght;
                grid.ColumnDefinitions.Add(columnDefinition);
            }

            return grid;
        }

        private DataGrid CreatDataGridWithPropertyCollection(DataGrid dataGrid, PropertyDescriptor descriptor, bool isInverted = false)
        {
            var newDataGrid = new DataGrid();

            var multiBinding = new MultiBinding
            {
                Converter = new ToPropertyCollectionMultiValueConverter(), 
                ConverterParameter = isInverted
            };

            AddBindingsForMultiBinding(multiBinding, dataGrid, descriptor);

            newDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, multiBinding);

            FillPropertiesAndSubscriptions(newDataGrid, dataGrid);

            return newDataGrid;
        }

        private void AddBindingsForMultiBinding(MultiBinding multiBinding, DataGrid dataGrid, PropertyDescriptor descriptor)
        {
            var sellectedItemBinding = new Binding
            {
                Source = dataGrid.DataContext,
                Path = new PropertyPath("SelectedItem[(0)]", descriptor.ComponentType)
            };

            var collectionOfPropertyBinding = new Binding
            {
                Source = dataGrid.DataContext,
                Path = new PropertyPath("Models[(0)]", descriptor.PropertyType.GetGenericArguments().Single())
            };

            multiBinding.Bindings.Add(sellectedItemBinding);
            multiBinding.Bindings.Add(collectionOfPropertyBinding);
        }

        private void FillPropertiesAndSubscriptions(DataGrid dataGrid, DataGrid mainDataGrid)
        {
            mainDataGrid.CopyValuePropertiesTo(dataGrid, p => p.Name != "ItemsSource");
            dataGrid.IsReadOnly = true;
            dataGrid.HorizontalAlignment = HorizontalAlignment.Left;
            dataGrid.CanUserAddRows = false;
            dataGrid.CanUserDeleteRows = false;

            dataGrid.AutoGeneratingColumn += AutoGenerateColumnsBehaviour.OnAutoGeneratingColumn;
            dataGrid.AutoGeneratedColumns += BindingStyleForAutoGenerateColumnsBehaviour.OnAutoGeneratedColumns;
        }
    }
}

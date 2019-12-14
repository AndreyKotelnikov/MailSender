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
                    DataGrid dataGridWithSelectFilter = CreatDataGridWithSelectFilter(mainDataGrid, descriptor);
                    DataGrid dataGridWithReverseSelectFilter = CreatDataGridWithSelectFilter(mainDataGrid, descriptor, true);
                    var sellectButton = CreatButtonForAddAndRemoveSellectedItems(dataGridWithSelectFilter, dataGridWithReverseSelectFilter);
                    Grid gridWithSellectButton = PutElementIntoGrid(sellectButton, 0, 1);
                    RenderingUIElements(mainDataGrid, dataGridWithSelectFilter, gridWithSellectButton, dataGridWithReverseSelectFilter);
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

        private ButtonForAddAndRemoveSellectedItems CreatButtonForAddAndRemoveSellectedItems(Selector sellectorForRemoveItems, Selector sellectorForAddItems)
        {
            var sellectButton = new ButtonForAddAndRemoveSellectedItems();
            return sellectButton;
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

        private DataGrid CreatDataGridWithSelectFilter(DataGrid dataGrid, PropertyDescriptor descriptor, bool isReverseFilter = false)
        {
            var newDataGrid = new DataGrid();
            var elementType = descriptor.PropertyType.GenericTypeArguments.Single();
            var sourceItems = (dataGrid.DataContext as MainWindowViewModel)?.Models[elementType];
            CollectionElementTypeConvertor.SetValueAsConvertToOriginTypeItems(elementType, sourceItems, "ItemsSource", newDataGrid);

            var filterHelper = new SelectionFilterCreater(newDataGrid.ItemsSource, descriptor.Name, descriptor.ComponentType, isReverseFilter);
            newDataGrid.ItemsSource = filterHelper.GetCollectionWithSelectionFilter(dataGrid);

            FillPropertiesAndSubscriptions(newDataGrid, dataGrid);

            return newDataGrid;
        }

        private void FillPropertiesAndSubscriptions(DataGrid dataGrid, DataGrid mainDataGrid)
        {
            mainDataGrid.CopyValuePropertiesTo(dataGrid, p => p.Name != "ItemsSource");
            dataGrid.IsReadOnly = true;
            dataGrid.HorizontalAlignment = HorizontalAlignment.Left;
            dataGrid.CanUserAddRows = false;

            dataGrid.AutoGeneratingColumn += AutoGenerateColumnsBehaviour.OnAutoGeneratingColumn;
            dataGrid.AutoGeneratedColumns += BindingStyleForAutoGenerateColumnsBehaviour.OnAutoGeneratedColumns;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfMailSender.Utils
{
    public static class RenderingHelper
    {
        public static void RenderingUIElements(FrameworkElement mainFrameworkElement, params UIElement[] elementsForRendering)
        {
            Grid grid = CreateGridWithRows(GridUnitType.Star, 1, 5, 1, 5);
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

        public static int FindLastGridColumn(FrameworkElement mainFrameworkElement)
        {
            var panel = mainFrameworkElement.Parent as Panel;
            if (panel == null) throw new ArgumentNullException(nameof(panel));

            return panel.Children.OfType<UIElement>()
                .Where(e => ((e is DataGrid || ((e as Grid)?.Children.OfType<UIElement>().Count(d => d is DataGrid) == 2))
                             && Grid.GetRow(e) == Grid.GetRow(mainFrameworkElement)))
                .Select(Grid.GetColumn)
                .Max();
        }

        public static Grid CreateGridWithRows(GridUnitType gridUnitType, params int[] heightRowsRatio)
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

        public static Grid CreateGridWithColumns(GridUnitType gridUnitType, params int[] widthColumnsRatio)
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

        public static void AddColumnsToGrid(Grid grid, GridUnitType gridUnitType, params int[] widthColumnsRatio)
        {
            foreach (var width in widthColumnsRatio)
            {
                var gridLenght = gridUnitType == GridUnitType.Auto
                                 ? GridLength.Auto
                                 : new GridLength(width, gridUnitType);
                var columnDefinition = new ColumnDefinition
                {
                    Width = gridLenght
                };
                grid.ColumnDefinitions.Add(columnDefinition);
            }
        }

        public static Grid PutElementIntoGrid(UIElement elementForPutInto, int row, int column)
        {
            var grid = new Grid();
            AddColumnsToGrid(grid, GridUnitType.Star, 3, 1, 3);
            Grid.SetRow(elementForPutInto, row);
            Grid.SetColumn(elementForPutInto, column);
            grid.Children.Add(elementForPutInto);
            return grid;
        }

        public static FrameworkElement GetLastParent(FrameworkElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (element.Parent == null)
            {
                return element;
            }

            return GetLastParent(element.Parent as FrameworkElement);
        }
    }
}

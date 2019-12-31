using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using Models.Abstract;

namespace WpfMailSender.Utils
{
    public class DataGridItemsRefreshValueConverter : MarkupExtension, IValueConverter
    {
        private Window _window;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string nameElement)
            {
                var dataGrid = _window?.FindName(nameElement) as DataGrid;
                dataGrid?.Items.Refresh();
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var propertyInfo = serviceProvider.GetType()
                .GetProperty("System.Xaml.IRootObjectProvider.RootObject", BindingFlags.NonPublic | BindingFlags.Instance);
            _window = propertyInfo.GetValue(serviceProvider) as Window;
            return this;
        }
    }
}

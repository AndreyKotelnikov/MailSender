using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Models.Abstract;

namespace WpfMailSender.Utils
{
    public class ValueConverterById : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var source = value as IBaseModel;
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Id == -1)
            {
                return null;
            }
            
            var targetCollection = parameter as IEnumerable<IBaseModel>;
            if (targetCollection == null) throw new ArgumentNullException(nameof(targetCollection));

            return targetCollection.Single(o => o.Id == source.Id);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            Convert(value, targetType, parameter, culture);
    }
}

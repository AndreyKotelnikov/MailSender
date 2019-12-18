using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Models.Abstract;

namespace WpfMailSender.Utils
{
    public class ToPropertyCollectionMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var modelCollection = values[1] as IEnumerable<IBaseModel>;
            var elementType = modelCollection?.FirstOrDefault()?.GetType();

            if (elementType == null)
            {
                return null;
            }

            if (values[0] != null)
            {
                var propertyInfo = values[0].GetType().GetProperties().Single(p =>
                    p.PropertyType.GetGenericArguments().SingleOrDefault() == elementType);
                var propertyCollection = (propertyInfo?.GetValue(values[0]) as IList)?.Cast<IBaseModel>();

                var isInverted = (bool)(parameter ?? false);

                if (propertyCollection != null)
                {
                    var resultCollection =
                        modelCollection.Where(v => isInverted
                            ? propertyCollection.All(e => e.Id != v.Id)
                            : propertyCollection.Any(e => e.Id == v.Id));
                    return resultCollection;
                }

                if (isInverted)
                {
                    // I still do not understand why the data is displayed incorrectly without "Where"...
                    return modelCollection.Where(e => true); 
                }
            }

            return CollectionElementTypeConverter.CreateEmptyList(elementType);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

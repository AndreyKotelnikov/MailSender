using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Models.Abstract;

namespace WpfMailSender.Utils
{
    public static class PropertyTypeExtentions
    {
        public static bool IsPropertyEnumerableOfModels(this Type type) =>
            type.GetInterfaces()
                .Any(i => i == typeof(IEnumerable))
            && (type.GenericTypeArguments
                    .SingleOrDefault()
                    ?.GetInterfaces()
                    .Any(i => i == typeof(IBaseModel))
                ?? false);

        public static bool IsPropertyEnumerable(this Type type) =>
            type.GetInterfaces()
                .Any(i => i == typeof(IEnumerable))
            && type != typeof(string);

        public static bool IsPropertyModel(this Type type) =>
            type.GetInterfaces()
                .Any(i => i == typeof(IBaseModel));

        public static bool IsPropertyDisplayNameNull(this PropertyDescriptor descriptor) =>
            (descriptor.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute)
            ?.DisplayName == null;

        public static bool IsPropertyImplementedBaseInterface(this PropertyDescriptor descriptor) =>
            typeof(IBaseModel).GetProperties()
                .Any(p => p.Name == descriptor.Name)
            || typeof(IBaseModel).GetInterfaces()
                .SelectMany(i => i.GetProperties())
                .Any(p => p.Name == descriptor.Name);

        public static void CopyValuePropertiesTo<T>(this T source, T targert, Func<PropertyInfo, bool> predicate)
        {
            var propertiesForCopy = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(predicate);
            foreach (var propertyInfo in propertiesForCopy)
            {
                if (propertyInfo.SetMethod != null)
                {
                    var value = propertyInfo.GetValue(source);
                    if (value != null)
                    {
                        propertyInfo.SetValue(targert, value);
                    }
                }
            }
        }
    }
}

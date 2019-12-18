using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Models.Abstract;

namespace WpfMailSender.Utils
{
    public static class CollectionElementTypeConverter
    {
        public static IEnumerable<IBaseModel> GetSetItemsSourceInOriginType(DependencyObject obj)
        {
            return (IEnumerable<IBaseModel>)obj.GetValue(SetItemsSourceInOriginTypeProperty);
        }

        public static void SetSetItemsSourceInOriginType(DependencyObject obj, IEnumerable<IBaseModel> value)
        {
            obj.SetValue(SetItemsSourceInOriginTypeProperty, value);
        }

        // Using a DependencyProperty as the backing store for SetItemsSourceInOriginType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SetItemsSourceInOriginTypeProperty =
            DependencyProperty.RegisterAttached("SetItemsSourceInOriginType",
                typeof(IEnumerable<IBaseModel>),
                typeof(CollectionElementTypeConverter),
                new PropertyMetadata(null, SetItemsSourceInOriginTypeChanged));

        private static void SetItemsSourceInOriginTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ItemsControl itemsControl 
                && e.NewValue is IEnumerable<IBaseModel> collection)
            {
                var elementType = collection.FirstOrDefault()?.GetType();
                if (elementType == null)
                {
                    return;
                }

                SetValueAsConvertToOriginTypeItems(elementType, collection, "ItemsSource", itemsControl);
            }
        }

        public static void SetValueAsConvertToOriginTypeItems(Type elementType, IEnumerable<IBaseModel> collection, string propertyName, object instance)
        {
            var propertyInfo = instance.GetType().GetProperty(propertyName);
            var methodSetImplicitItemsSourceGeneric = typeof(CollectionElementTypeConverter)
                .GetMethod(nameof(SetValueAsConvertToOriginTypeItemsGeneric), BindingFlags.Static | BindingFlags.NonPublic);
            methodSetImplicitItemsSourceGeneric?.MakeGenericMethod(elementType)
                .Invoke(null, new [] { collection, propertyInfo, instance });
        }

        private static void SetValueAsConvertToOriginTypeItemsGeneric<T>(IEnumerable<IBaseModel> collection, PropertyInfo propertyInfo, object instance)
        {
            var originTypeOfItemsCollection = new ObservableCollection<T>(collection.Cast<T>().ToArray());

            propertyInfo.SetValue(instance, originTypeOfItemsCollection);
        }

        public static IList CreateEmptyList(Type elementType)
        {
            var methodCreateListGeneric = typeof(CollectionElementTypeConverter)
                .GetMethod(nameof(CreateEmptyListGeneric), BindingFlags.Static | BindingFlags.NonPublic);
            return (IList) methodCreateListGeneric?.MakeGenericMethod(elementType)
                .Invoke(null, null);
        }

        private static IList CreateEmptyListGeneric<T>()
        {
            return new List<T>();
        }
    }
}

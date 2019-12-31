using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace WpfMailSender.Behaviours
{
    public class RaiseEventPropertyChangedBehaviour : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.SourceUpdated += AssociatedObject_SourceUpdated;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SourceUpdated -= AssociatedObject_SourceUpdated; 
        }

        private void AssociatedObject_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var path = BindingOperations.GetBinding(textBox, TextBox.TextProperty)
                    .Path;

                var proprtyName = new string(path.Path.TakeWhile(ch => ch != '[').ToArray());

                RaisePropertyChangedEvent(textBox.DataContext, proprtyName);

                RaisePropertyChangedEvent(textBox.DataContext, "Models");
            }
        }

        private void RaisePropertyChangedEvent(object dataContext, string propertyName)
        {
            var propertyInfo = dataContext.GetType().GetProperty(propertyName);

            var methodInfo = propertyInfo.PropertyType.GetMethod("RaisePropertyChangedEventForIndexerName");
            methodInfo.Invoke(propertyInfo.GetValue(dataContext), null);
        }
    }
}

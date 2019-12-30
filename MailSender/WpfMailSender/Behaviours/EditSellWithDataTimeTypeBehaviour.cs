using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using WpfMailSender.Abstracts;
using WpfMailSender.Utils;
using WpfMailSender.ViewModels;
using WpfMailSender.Views;

namespace WpfMailSender.Behaviours
{
    public class EditSellWithDataTimeTypeBehaviour : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PreparingCellForEdit += AssociatedObject_PreparingCellForEdit;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreparingCellForEdit -= AssociatedObject_PreparingCellForEdit;
        }

        private void AssociatedObject_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            if (!(sender is DataGrid dataGrid))
            {
                return;
            }
            
            var propertyInfo = e.EditingElement.DataContext
                .GetType()
                .GetProperty(e.Column.SortMemberPath);
            if (propertyInfo?.PropertyType != typeof(DateTime))
            {
                return;
            }
            
            var dateTime = (DateTime)propertyInfo.GetValue(e.EditingElement.DataContext);
            var dataContext = new EditDateTimeViewModel(dateTime);

            var editWindow = new EditDateTimeWindow
            {
                Owner = RenderingHelper.GetLastParent(dataGrid) as Window,
                DataContext = dataContext
            };

            editWindow.Closed += (s, eArg) =>
            {
                propertyInfo.SetValue(e.EditingElement.DataContext, dataContext.DateTime);
                dataGrid.CommitEdit();
            };

            editWindow.ShowDialog();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfMailSender.Components
{
    /// <summary>
    /// Логика взаимодействия для ButtonForAddAndRemoveSellectedItems.xaml
    /// </summary>
    public partial class ButtonForAddAndRemoveSellectedItems
    {
        public ButtonForAddAndRemoveSellectedItems()
        {
            InitializeComponent();
        }

       

        public static readonly DependencyProperty ArrowDirectionProperty = DependencyProperty.Register(
            "ArrowDirection",
            typeof(ArrowDirectionEnum),
            typeof(ButtonForAddAndRemoveSellectedItems),
            new PropertyMetadata(ArrowDirectionEnum.None));

        public ArrowDirectionEnum ArrowDirection
        {
            get => (ArrowDirectionEnum) GetValue(ArrowDirectionProperty);
            set => SetValue(ArrowDirectionProperty, value);
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(ButtonForAddAndRemoveSellectedItems),
            new PropertyMetadata(default(ICommand)));

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(ButtonForAddAndRemoveSellectedItems),
            new PropertyMetadata(default(object)));

        public object CommandParameter
        {
            get { return (object) GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public void CollectionForRemoveItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetArrowDirection(e.AddedItems.Count, ArrowDirectionEnum.Down);
        }

        public void CollectionForAddItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetArrowDirection(e.AddedItems.Count, ArrowDirectionEnum.Up);
        }

        public void CollectionForRemoveItems_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is MultiSelector multiSelector)
            {
                SetArrowDirection(multiSelector.SelectedItems.Count, ArrowDirectionEnum.Down);
            }
        }

        public void CollectionForAddItems_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is MultiSelector multiSelector)
            {
                SetArrowDirection(multiSelector.SelectedItems.Count, ArrowDirectionEnum.Up);
            }
        }

        private void SetArrowDirection(int itemsCount, ArrowDirectionEnum arrowDirection)
        {
            ArrowDirection = itemsCount > 0 
                ? arrowDirection 
                : ArrowDirectionEnum.None;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            var t = this;
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
    }
}

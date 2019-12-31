using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Models.Abstract;

namespace WpfMailSender.Utils
{
    [ContentProperty("Parameters")]
    public class ParameterBinding : MarkupExtension
    {
        public Binding Binding { get; set; }
        public IList Parameters { get; set; }


        public ParameterBinding()
        {
            Parameters = new List<object>();
        }

        public ParameterBinding(Binding b, object p0)
        {
            Binding = b;
            Parameters = new[] { p0 };
        }

        public ParameterBinding(Binding b, object p0, object p1)
        {
            Binding = b;
            var propertyInfo = (p0 as Type)?.GetProperty(p1.ToString());
            Parameters = new[] { p0, propertyInfo };
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Binding.Path = new PropertyPath(Binding.Path.Path, Parameters.Cast<object>().ToArray());
            
            return Binding.ProvideValue(serviceProvider);
        }
    }
}

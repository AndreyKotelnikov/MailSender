using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Abstract;
using WpfMailSender.Utils;

namespace WpfMailSender.Abstracts
{
    public interface IViewModelCollectionsOfModels
    {
        ObservableDictionary<Type, ObservableCollection<IBaseModel>> Models { get; }
    }
}

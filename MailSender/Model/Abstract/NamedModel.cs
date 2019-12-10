using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Abstract
{
    /// <summary>
    /// Именованная модель
    /// </summary>
    public abstract class NamedModel : BaseModel
    {
        /// <summary>
        /// Наименование
        /// </summary>
        [DisplayName("Наименование")]
        public string Name { get; set; }
    }
}

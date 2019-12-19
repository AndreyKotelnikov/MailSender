using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Abstract;

namespace Models
{
    public class RecipientsListModel : NamedModel
    {
        [DisplayName("Список получателей") ]
        public ICollection<RecipientModel> RecipientsModel { get; set; }

        public ICollection<SchedulerTaskModel> SchedulerTasksModel { get; set; }
    }
}

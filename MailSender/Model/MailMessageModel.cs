using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Abstract;

namespace Models
{
    public class MailMessageModel : BaseModel
    {
        [DisplayName(null)]
        public string Body { get; set; }
        
        public string Subject { get; set; }

        [DisplayName(null)]
        public ICollection<SchedulerTaskModel> SchedulerTaskModel { get; set; }

        public ICollection<RecipientsListModel> RecipientsLists { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Abstract;

namespace Models
{
    public class MailMessageModel : BaseModel
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public virtual ICollection<SchedulerTaskModel> SchedulerTaskModel { get; set; }
    }
}

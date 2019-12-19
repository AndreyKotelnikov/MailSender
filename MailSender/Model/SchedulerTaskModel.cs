using System;
using System.Net.Mail;
using Models.Abstract;

namespace Models
{
    public class SchedulerTaskModel : NamedModel
    {
        public virtual MailMessageModel MailMessageModel { get; set; }

        public virtual RecipientsListModel RecipientsListModel { get; set; }
        
        public virtual SenderModel SenderModel { get; set; }

        public virtual ServerModel ServerModel { get; set; }
        
        public DateTime PlanedTime { get; set; }
    }
}

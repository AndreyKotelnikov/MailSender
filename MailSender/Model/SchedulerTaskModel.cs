using System;
using System.ComponentModel;
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
        
        [DisplayName("Плановое время")]
        public DateTime PlanedTime { get; set; }
    }
}

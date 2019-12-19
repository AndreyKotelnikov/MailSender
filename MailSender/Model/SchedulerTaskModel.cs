using System;
using System.Net.Mail;
using Models.Abstract;

namespace Models
{
    public class SchedulerTaskModel : NamedModel
    {
        public DateTime PlanedTime { get; set; }

        public int MailMessageId { get; set; }

        public virtual MailMessageModel MailMessageModel { get; set; }

        public int SenderId { get; set; }

        public virtual SenderModel SenderModel { get; set; }

        public int ListId { get; set; }

        public virtual RecipientsListModel RecipientsListModel { get; set; }

        public int ServerId { get; set; }

        public virtual ServerModel ServerModel { get; set; }
    }
}

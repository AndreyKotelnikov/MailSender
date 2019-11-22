using System;
using System.Net.Mail;
using Entities.Abstract;

namespace Entities
{
    public class SchedulerTaskEntity : NamedEntity
    {
        public DateTime PlanedTime { get; set; }

        public int MailMessageId { get; set; }

        public virtual MailMessageEntity MailMessageEntity { get; set; }

        public int SenderId { get; set; }

        public virtual SenderEntity SenderEntity { get; set; }

        public int ListId { get; set; }

        public virtual RecipientsListEntity RecipientsListEntity { get; set; }

        public int ServerId { get; set; }

        public virtual ServerEntity ServerEntity { get; set; }
    }
}

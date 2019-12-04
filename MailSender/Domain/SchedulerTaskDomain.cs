using System;
using System.Net.Mail;
using Domains.Abstract;

namespace Domains
{
    public class SchedulerTaskDomain : NamedDomain
    {
        public DateTime PlanedTime { get; set; }

        public int MailMessageId { get; set; }

        public virtual MailMessageDomain MailMessageDomain { get; set; }

        public int SenderId { get; set; }

        public virtual SenderDomain SenderDomain { get; set; }

        public int ListId { get; set; }

        public virtual RecipientsListDomain RecipientsListDomain { get; set; }

        public int ServerId { get; set; }

        public virtual ServerDomain ServerDomain { get; set; }
    }
}

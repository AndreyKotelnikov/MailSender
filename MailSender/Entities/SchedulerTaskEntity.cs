using System;
using System.Net.Mail;
using Entities.Abstract;

namespace Entities
{
    public class SchedulerTaskEntity : NamedEntity
    {
        public DateTime PlanedTime { get; set; }

        public int MailMessageId { get; set; }

        public int SenderId { get; set; }

        public int ListId { get; set; }

        public int ServerId { get; set; }
    }
}

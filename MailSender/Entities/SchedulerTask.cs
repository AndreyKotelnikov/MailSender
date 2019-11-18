using System;
using System.Net.Mail;
using Entities.Abstract;

namespace Entities
{
    public class SchedulerTask : NamedEntity
    {
        public DateTime PlanedTime { get; set; }

        public virtual MailMessage MailMessage { get; set; }

        public virtual Sender Sender { get; set; }

        public virtual RecipientsList List { get; set; }

        public virtual Server Server { get; set; }
    }
}

using System.Collections.Generic;
using Models.Abstract;

namespace Models
{
    public class ServerModel : ConnectionModel
    {
        public int Port { get; set; }

        public bool UseSSL { get; set; } = true;

        public string UserName { get; set; }

        public virtual ICollection<SchedulerTaskModel> SchedulerTaskModel { get; set; }
    }
}

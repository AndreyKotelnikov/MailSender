using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Abstract;

namespace Models
{
    [DisplayName("Получатели")]
    public class RecipientModel : ConnectionModel
    {
        [DisplayName("Списки получателей")]
        public ICollection<RecipientsListModel> RecipientsListModel { get; set; }

        [DisplayName("Тест списка")]
        public ServerModel Server { get; set; }
    }
}

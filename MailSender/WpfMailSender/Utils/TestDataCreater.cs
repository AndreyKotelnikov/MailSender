using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Abstract;

namespace WpfMailSender.Utils
{
    public static class TestDataCreater
    {
        public static ObservableCollection<IBaseModel> CreateTestData(Type typeOfData)
        {
            List<IBaseModel> models = Enumerable.Range(1, 10).Select(m => Activator.CreateInstance(typeOfData)).Cast<IBaseModel>().ToList();
            if (typeOfData == typeof(RecipientModel))
            {
                for (var i = 0; i < models.Count; i++)
                {
                    var recipientModel = models[i] as RecipientModel;
                    recipientModel.Id = i + 1;
                    recipientModel.Name = $"{typeOfData.Name.Replace("Model", string.Empty)} {i + 1}";
                    recipientModel.ConnectAdress = $"server_{i + 1}@mail.ru";
                    recipientModel.Description = i % 3 == 0 ? $"Комментарий {i + 1}" : null;
                    if (i == 3)
                    {
                        recipientModel.RecipientsListModel = new List<RecipientsListModel>
                        {
                            new RecipientsListModel(){Id = 1, Name = "List 1", RecipientsModel = new List<RecipientModel>{new RecipientModel{Id = i}, new RecipientModel{Id = i + 1}}},
                            new RecipientsListModel(){Id = 2, Name = "List 2", RecipientsModel = new List<RecipientModel>{new RecipientModel{Id = i}, new RecipientModel{Id = i + 1}}}
                        };
                        recipientModel.Server = new ServerModel { Id = 1, Name = "Test Server" };
                    }
                }
            }
            else
            if (typeOfData.BaseType == typeof(ConnectionModel))
            {
                for (var i = 0; i < models.Count; i++)
                {
                    var connModel = models[i] as ConnectionModel;
                    connModel.Id = i + 1;
                    connModel.Name = $"{typeOfData.Name.Replace("Model", string.Empty)} {i + 1}";
                    connModel.ConnectAdress = $"server_{i + 1}@mail.ru";
                    connModel.Description = i % 3 == 0 ? $"Комментарий {i + 1}" : null;
                }
            }
            else
            if (typeOfData.BaseType == typeof(NamedModel))
            {
                for (var i = 0; i < models.Count; i++)
                {
                    var namedModel = models[i] as NamedModel;
                    namedModel.Id = i + 1;
                    namedModel.Name = $"{typeOfData.Name.Replace("Model", string.Empty)} {i + 1}";
                }
            }
            else
            {
                for (var i = 0; i < models.Count; i++)
                {
                    var namedModel = models[i] as BaseModel;
                    namedModel.Id = i + 1;
                }
            }

            var collection = new ObservableCollection<IBaseModel>(models);

            return collection;
        }
    }
}

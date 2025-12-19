using Abp.Domain.Repositories;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class SqoopeMsgTypeDAL
    {
        IRepository<SqoopeMsgType, Guid> db;
        public SqoopeMsgTypeDAL(IRepository<SqoopeMsgType, Guid> sqoopemsgtypeRepository)
        {
            db = sqoopemsgtypeRepository;
        }

        public string GetMessageTemplateByMsgCode(string msgCode)
        {
            string template = "";
            template = db.GetAll().Where(x => x.Code == msgCode).Select(x => x.MessageTemplate).FirstOrDefault();

            return template;
        }

        public string GetMessageKeyByMsgCode(string msgCode)
        {
            string MessageKey = "";
            var ms = db.GetAll().Where(x => x.Code == msgCode).Select(x => x.Id);
            if (ms != null)
            {
                MessageKey = ms.FirstOrDefault().ToString();
            }
            return MessageKey;
        }
    }
}

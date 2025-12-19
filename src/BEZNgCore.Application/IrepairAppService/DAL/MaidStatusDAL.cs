using Abp.Domain.Repositories;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class MaidStatusDAL
    {
        IRepository<MaidStatus, Guid> db;
        public MaidStatusDAL(IRepository<MaidStatus, Guid> maidstatusRepository)
        {
            db = maidstatusRepository;
        }

        public string GetStatus(Guid? maidStatusKey)
        {
            string status = "";
            try
            {
                status = db.GetAll().Where(x => x.Id == maidStatusKey).Select(x => x.MaidStatusName).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Guid GetMaidStatusKey(string status)
        {
            Guid MaidStatusKey = Guid.Empty;
            try
            {
                MaidStatusKey = db.GetAll().Where(x => x.MaidStatusName == status).Select(x => x.Id).FirstOrDefault();
            }
            catch (Exception e)
            {
                string msg = e.InnerException.Message;
                Console.WriteLine(msg);
            }
            return MaidStatusKey;
        }

        public List<MaidStatus> GellMaidStatus()
        {
            List<MaidStatus> lst = new List<MaidStatus>();
            try
            {
                lst = db.GetAll().OrderByDescending(x => x.Seq).ToList();
            }
            catch { }
            return lst;
        }
    }
}

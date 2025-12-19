using Abp.Domain.Repositories;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class WorkOrderEnteryDAL
    {
        IRepository<MWorkOrder, int> db;
        public WorkOrderEnteryDAL(IRepository<MWorkOrder, int> mworkorderRepository)
        {
            db = mworkorderRepository;
        }
        public List<MWorkOrder> GetAllSeqno(DateTime yesterday)
        {
            List<MWorkOrder> lst = new List<MWorkOrder>();
            lst = db.GetAll()
                         .Where(x => x.EnteredDateTime.Value.Date.Year == yesterday.Year && x.EnteredDateTime.Value.Date.Month == yesterday.Month && x.EnteredDateTime.Value.Date > yesterday)
                         .ToList();
            return lst;
        }
        public async Task<int> SaveAsync(MWorkOrder d)
        {
            int success = 0;
            try
            {
                success = await db.InsertAndGetIdAsync(d);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return success;
        }
    }
}

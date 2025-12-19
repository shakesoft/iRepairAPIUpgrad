using Abp.Domain.Repositories;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class HistoryDAL
    {
        IRepository<History, Guid> db;
        public HistoryDAL(IRepository<History, Guid> historyRepository)
        {
            db = historyRepository;
        }
        public async Task<int> SaveAsync(History d)
        {
            int success = 0;
            try
            {
                await db.InsertAsync(d);
                success = 1;
            }
            catch (Exception ex)
            {
            }
            return success;
        }
        public async Task UpdateAsync(History d)
        {
            try
            {
                await db.UpdateAsync(d);

            }
            catch (Exception ex)
            {
            }
        }
        public List<History> RetivehistoryforUpdate(Guid? SourceKey)
        {
            string ModuleName = "iClean"; string TableName = "Room";
            string detail = "STARTS";
            List<History> lst = new List<History>();
            try
            {
                lst = db.GetAll().Where(x => x.ModuleName == ModuleName && x.TableName == TableName
                                && x.SourceKey == SourceKey && x.Detail.Contains(detail) && x.LinkKey == null).OrderByDescending(x => x.ChangedDate).ToList();
            }
            catch { }
            return lst;
        }

    }
}

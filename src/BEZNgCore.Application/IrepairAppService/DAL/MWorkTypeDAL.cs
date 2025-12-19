using Abp.Domain.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class MWorkTypeDAL
    {
        IRepository<MWorkType, int> db;
        public MWorkTypeDAL(IRepository<MWorkType, int> mworktypeRepository)
        {
            db = mworktypeRepository;
        }
        public List<DDLWorkTypeOutput> GetAllData()
        {
            List<DDLWorkTypeOutput> lst = new List<DDLWorkTypeOutput>();
            try
            {
                lst = db.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Description)
                .Select(x => new DDLWorkTypeOutput
                {
                    Seqno = x.Id,
                    Description = x.Description
                })
                .ToList();
            }
            catch { }
            return lst;
        }
    }
}

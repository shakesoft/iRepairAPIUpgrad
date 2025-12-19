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
    public class MareaDAL
    {
        IRepository<MArea, int> db;
        public MareaDAL(IRepository<MArea, int> mareaRepository)
        {
            db = mareaRepository;
        }
        public List<DDLAreaOutput> GetAllData()
        {
            List<DDLAreaOutput> lst = new List<DDLAreaOutput>();
            try
            {
                lst = db.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Description)
                .Select(x => new DDLAreaOutput
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

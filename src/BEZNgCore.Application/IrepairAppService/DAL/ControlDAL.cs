using Abp.Domain.Repositories;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class ControlDAL
    {
        IRepository<Control, Guid> db;
        public ControlDAL(IRepository<Control, Guid> controlRepository)
        {
            db = controlRepository;
        }
        public DateTime GetSystemdate()
        {
            DateTime dt = DateTime.Now;
            try
            {
                var d = db.GetAll().Select(x => x.SystemDate).FirstOrDefault();
                if (d != null)
                {
                    dt = d.Value;
                }

            }
            catch (Exception ex)
            {
            }
            return dt;
        }

    }
}

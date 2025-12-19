using Abp.Domain.Repositories;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class LostFoundDAL
    {
        IRepository<LostFound, Guid> db;
        public LostFoundDAL(IRepository<LostFound, Guid> lostfoundRepository)
        {
            db = lostfoundRepository;
        }
        public Guid Save(LostFound d)
        {
            Guid success = Guid.Empty;
            try
            {
                success = db.InsertAndGetId(d);
                
            }
            catch (Exception ex)
            {
            }
            return success;
        }
        public int Update(LostFound d)
        {
            int success = 0;
            try
            {
                List<LostFound> lst = new List<LostFound>();

                lst = db.GetAll().Where(x => x.Id == d.Id).ToList();
                if (lst != null)
                {
                    lst[0].LostFoundStatusKey = d.LostFoundStatusKey;
                    lst[0].ReportedDate = d.ReportedDate;
                    lst[0].ItemName = d.ItemName;
                    lst[0].Area = d.Area;
                    lst[0].Owner = d.Owner;
                    lst[0].OwnerFolio = d.OwnerFolio;
                    lst[0].OwnerRoomKey = d.OwnerRoomKey;
                    lst[0].OwnerContactNo = d.OwnerContactNo;
                    lst[0].Founder = d.Founder;
                    lst[0].FounderFolio = d.FounderFolio;
                    lst[0].FounderRoomKey = d.FounderRoomKey;
                    lst[0].FounderContactNo = d.FounderContactNo;
                    lst[0].Description = d.Description;
                    lst[0].Instruction = d.Instruction;
                    lst[0].AdditionalInfo = d.AdditionalInfo;
                    lst[0].StaffKey = d.StaffKey;
                    lst[0].Sort = d.Sort;
                    lst[0].Sync = d.Sync;
                    lst[0].AutoReference = d.AutoReference;
                    lst[0].Reference = d.Reference;
                    lst[0].TenantId = d.TenantId;
                    success = 1;
                }

                //db.Update(d);

            }
            catch (Exception ex)
            {
            }
            return success;
        }
    }
}

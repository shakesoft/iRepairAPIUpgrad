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
    public class StaffDAL
    {
        IRepository<Staff, Guid> db;
        public StaffDAL(IRepository<Staff, Guid> staffRepository)
        {
            db = staffRepository;
        }

        public Staff GetStaffInfoByStaffKey(Guid staffkey)
        {
            Staff s = new Staff();
            try
            {
                s = db.GetAll().Where(x => x.Id == staffkey).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return s;
        }
        public List<MaidOutput> GetAllData()
        {
            List<MaidOutput> lst = new List<MaidOutput>();
            try
            {
                lst = db.GetAll().Where(x => x.Active == 1).OrderBy(x => x.UserName)
                .Select(x => new MaidOutput
                {
                    StaffKey = x.Id,
                    UserName = x.UserName
                })
                .ToList();
            }
            catch { }
            return lst;
        }

        public Guid GetMaidKey(Guid StaffKey)
        {
            Guid MaidKey = Guid.Empty;
            try
            {
                MaidKey = db.GetAll().Where(x => x.Id == StaffKey).Select(x => x.MaidKey).FirstOrDefault().Value;
            }
            catch (Exception ex)
            {
            }
            return MaidKey;
        }

        public Staff GetStaffInfoByAttendantKey(string previousAttendantKey)
        {
            Staff s = new Staff();
            Guid MaidKey = new Guid(previousAttendantKey);
            try
            {
                s = db.GetAll().Where(x => x.MaidKey == MaidKey).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return s;
        }
        public bool IsLoginUserBlockRoomSupervisor(Guid staffkey)
        {
            bool result = false;
            
            var Sec_BlockRoom = db.GetAll().Where(x => x.Id == staffkey).Select(x => x.Sec_BlockRoom).FirstOrDefault();
            if (Sec_BlockRoom != null)
            {
                if (Sec_BlockRoom == 10)
                    result = true;
            }

            return result;
        }


    }
}

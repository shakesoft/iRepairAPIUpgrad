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
    public class RoomStatusDAL
    {
        IRepository<RoomStatus, Guid> db;
        public RoomStatusDAL(IRepository<RoomStatus, Guid> roomstatusRepository)
        {
            db = roomstatusRepository;
        }
        public List<GetRoomStatus> GetAllRoomStatus()
        {
            List<GetRoomStatus> lst = new List<GetRoomStatus>();
            try
            {
                lst = db.GetAll().OrderByDescending(s => s.Seq)
                                        .Select(s => new GetRoomStatus
                                        {
                                            RoomStatusKey = s.Id,
                                            RoomStatus = s.RoomStatusName
                                        })
                                        .ToList();
            }
            catch { }
            return lst;
        }
        public Guid GetRoomStatusKey(string RoomStatus)
        {
            Guid RoomStatusKey = Guid.Empty;
            try
            {
                RoomStatusKey = db.GetAll().Where(x => x.RoomStatusName == RoomStatus).Select(x => x.Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return RoomStatusKey;
        }
    }
}

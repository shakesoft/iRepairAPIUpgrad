using Abp.Domain.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.CustomizeRepository
{
    public interface IRoomRepository : IRepository<Room, Guid>
    {
        Task<List<GetHotelRoomFloorOutPut>> GetHotelRoomFloor();
        Task<List<GetDashRoomByMaidKeyOutput>> GetRoomByMaidKey();
        Task<List<MaidHasStartedTaskOutput>> GetRoomCountByMaidKey(DateTime dtBusinessDate, string maidKey, string floorNo);
        Task<List<MaidStatusListOutPut>> BindMaidStatusListCount(DateTime dtBusinessDate, string maidKey, string floorNo);
        // List<MaidStatusListOutPut> BindMaidStatusListCountSupAsync(DateTime dtBusinessDate, string maidKey, string floorNo);
        //Task<List<MaidStatusListOutPut>> BindMaidStatusListCountSup(DateTime dtBusinessDate, string maidKey, string floorNo);
    }
}

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
    public interface IMaidStatusRepository : IRepository<MaidStatus, Guid>
    {
        List<ViewLogOutput> GetHistory(string logType, string staffKey, string roomKey);
        List<ViewLogIROutput> GetIRHistory(string logType, string staffKey);
        Task<List<GetDashRoomByMaidStatusKeyOutput>> GetDashRoomByMaidStatusKey(DateTime dtBusinessDate, string maidStatusKey, string maidKey, string floorNo, string roomStatusKey = "");
        Task<List<GetMaidStatusOutput>> GetMaidStatusKeyByStatusAsync(string houseKeepingMaidStatusInspectionRequired);
        Task<List<GetMaidStatusOutput>> GetBusinessDate();
        Task<List<GetDashRoomByMaidKeyOutput>> GetRoomByMaidKey(DateTime dtBusinessDate, string maidKey, string maidStatusKey, string roomStatusKey, string floor, string guestStatus);
        List<MaidOutput> GetAllAttendant();
        List<DDLRoomOutput> GetAllRoom();
        List<TechnicianOutput> GetStaffAllTechnician();
    }
}

using Abp.Domain.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.CustomizeRepository
{

    public interface IMworkorderdalRepository : IRepository<RoomStatus, Guid>
    {
        DataTable GetWOByTechnician(int technicianID, int v1, string v2);
        Task<List<GetMaidStatusOutput>> GetBusinessDate();
        Task<List<GetDashRoomByMaidKeyOutput>> GetRoomByMaidKey(DateTime dtBusinessDate, string maidKey, string maidStatusKey, string roomStatusKey, string floor, string guestStatus);
        DataTable GetUnassignedTechWorkOrderCount();
        List<HistoryDto> GetTodayHistory(Guid staffkey);
        List<WOStatusOutput> GetWorkOrderStatusCountByTechnician(int technicalID);
        List<GetMyTaskDataOutput> GetWorkOrderByTechnician(int technicianID, int woStatus, string roomStatus, int priority);
        DataTable GetWIPWorkOrder();
        DataTable GetWorkOrderByID(int mworkorderno);
        bool IsRoomBlockExist(BlockRoom room);
        DataTable GetReservationByRoomKeyDateRange(Guid roomkey, DateTime dateTime);
        int Insert(List<BlockRoom> listRoom);
        int InsertHistory(History history);
        List<GetViewBlockRoomOutput> GetBlockRoomWorkOrderBy(int? tech, string room, DateTime? dtFromDate, DateTime? dtToDate);
        List<GetAllWoOutput> GetIncompletedWorkOrderStatus(int tech, int worktype, int workstatus, int area, string unit, Guid staff, int mpriority);
        DataTable GetBlockRoomByKey(string key);
        DataTable GetBlockRoomByWorkOrderID(int mworkorderno);
        int UpdateBlockroom(BlockRoom room);
        int UpdateWorkNote(MWorkNote workNote);
        DataTable GetWorkNoteByKey(string litWorkNoteKey);
        int InsertWorkNote(MWorkNote workNote);
        List<HistoryICleanDto> GetIcleanTodayHistory(Guid staffkey);
        List<DDLMPriorityOutput> GetDDLPriority();
    }
}

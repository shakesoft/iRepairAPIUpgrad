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

    public interface IMworkordertimesheetdalRepository : IRepository<MWorkOrderStatus, int>
    {
        bool MaidHasStartedTask(int seqno);
        List<WoTimeSheetListOutput> GetWorkTimeSheetByWOID(int woID);
        List<BlockUnBlockRoomListOutput> GetBlockRoomByWorkOrderID(int seqno);
        DataTable GetWorkOrderByID(int seqno);
        List<WoSecurityAuditlist> GetWorkOrderHistory(string mWorkOrderKey);
        List<WoWorkNote> GetWorkNotes(string mWorkOrderKey);
        List<VWorkOrderOutput> GetUnassignedTechWorkOrder();
        List<VWorkOrderOutput> GetWorkOrderByStatus(int woStatus);
        int UpdatAssignTechnicianToWorkOrder(List<MWorkOrderInput> listWork);
        int InsertHistoryList(List<History> listHistory);
        int UpdatWOStatusToWorkOrder(List<MWorkOrderInput> listWork);
        string GetMTechnicianBySeqNo(int technicalID);
        int Inserttimesheet(MWorkTimeSheetInput timeSheet);
        int UpdateRoomMaidStatus(Room room);
        List<GetMaidStatusOutput> GetMaidStatusKeyByStatus(string status);
        DataTable GetWorkTimeSheetByHdr_Seqno(int hdr_Seqno);
        int UpdateByHdr_Seqno(MWorkTimeSheetInput timeSheet);
        DataTable GetBlockRoomByWorkOrderIDDatatable(int v);
        int UpdateStatus(List<BlockRoom> listRoom);
        int UpdatWorkOrder(MWorkOrderInput work);
        string GetMaidStatusByRoomKey(Guid key);
        DataTable GetWorkTimeSheetByID(int seqno);
        int UpdateTimesheet(MWorkTimeSheetInput timeSheet);
        List<DDLRoomOutput> GetAllRoom();
        List<DDLAreaOutput> GetAllArea();
        List<DDLWorkTypeOutput> GetAllWorkType();
        List<MaidOutput> GetAllReportedBy();
        List<DDLWorkStatusOutput> GetAllCurrentStatus();
        List<DDPriorityOutput> GetAllPriority();
        List<DDLTechnicianOutput> GetAllTechnician();
        string GetAreaByKey(int key);
        string GetWorkTypeByKey(int key);
        string GetRoomByKey(Guid key);
        string GetWorkStatusByKey(int seqno);
        string GetReportedName(Guid id);
        string GetPriorityName(int sort);
        Guid GetTechnicianKey(Guid id);
        int GetTechnicalID(Guid technicianKey);
        List<DDLNoteTemplateOutput> GetAllNoteTemplate();
        DataTable GetDocumentByWoKey(Guid id);
        int CheckWoImage(WOImage image);
        int InsertWoImage(WOImage image);
        int UpdateWoImage(WOImage image);
        int MRCheckExit(Guid id, Guid? maidStatusKey);
    }
}

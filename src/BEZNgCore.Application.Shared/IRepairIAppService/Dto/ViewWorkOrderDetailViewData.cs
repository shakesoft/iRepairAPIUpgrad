using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class ViewWorkOrderDetailViewData
    {
        public ViewWorkOrderDetailViewData()
        {
            
            WoTimeSheetList = new HashSet<WoTimeSheetListOutput>();
            BlockUnBlockRoomList = new HashSet<BlockUnBlockRoomListOutput>();
            WoWorkNoteList = new HashSet<WoWorkNote>();
            WoSecurityAuditlist = new HashSet<WoSecurityAuditlist>();
        }
        public WorkOrderInfoDropdown WorkOrderInfoDropdown { get; set; }
        public WorkOrderInfoOutput WorkOrderInfoOutput { get; set; }
        public ICollection<WoTimeSheetListOutput> WoTimeSheetList { get; set; }
        public ICollection<BlockUnBlockRoomListOutput> BlockUnBlockRoomList { get; set; }
        public ICollection<WoWorkNote> WoWorkNoteList { get; set; }
        public ICollection<WoSecurityAuditlist> WoSecurityAuditlist { get; set; }

    }
    public class ViewWODetailWOInfo
    {
        public WorkOrderInfoDropdown WorkOrderInfoDropdown { get; set; }
        public WorkOrderInfoOutput WorkOrderInfoOutput { get; set; }

    }
    public class ViewWODetailWOImgInfo
    {
        public ViewWODetailWOImgInfo()
        {
            imglst = new HashSet<FWOImageD>();
        }
        
        public WorkOrderInfoDropdown WorkOrderInfoDropdown { get; set; }
        public WorkOrderInfoOutput WorkOrderInfoOutput { get; set; }
        public ICollection<FWOImageD> imglst { get; set; }

}
    public class ViewWODetailTimeSheet
    {
        public ViewWODetailTimeSheet()
        {
            btnStartEnabled = true;
            btnEndEnabled = true;

            WoTimeSheetList = new HashSet<WoTimeSheetListOutput>();
        }
     
        public ICollection<WoTimeSheetListOutput> WoTimeSheetList { get; set; }
        public bool btnStartEnabled { get; set; }
        public bool btnEndEnabled { get; set; }
    }
    public class ViewWODetailBlockUnBlockRoom
    {
        public ViewWODetailBlockUnBlockRoom()
        {
            BlockUnBlockRoomList = new HashSet<BlockUnBlockRoomListOutput>();
        }

        public ICollection<BlockUnBlockRoomListOutput> BlockUnBlockRoomList { get; set; }

    }
    public class ViewWODetailWorkNote
    {
        public ViewWODetailWorkNote()
        {
            WoWorkNoteList = new HashSet<WoWorkNote>();
        }
        public ICollection<WoWorkNote> WoWorkNoteList { get; set; }

    }
    public class ViewWODetailSecurityAudit
    {
        public ViewWODetailSecurityAudit()
        {
            WoSecurityAuditlist = new HashSet<WoSecurityAuditlist>();
        }
        public ICollection<WoSecurityAuditlist> WoSecurityAuditlist { get; set; }

    }
    public class WorkOrderInfoDropdown
    {
        public WorkOrderInfoDropdown()
        {

            Room = new HashSet<DDLRoomOutput>();
            Area = new HashSet<DDLAreaOutput>();
            WorkType = new HashSet<DDLWorkTypeOutput>();
            ReportedBy = new HashSet<MaidOutput>();
            CurrentStatus = new HashSet<DDLWorkStatusOutput>();
            Technician = new HashSet<DDLTechnicianOutput>();
        }
        
        public ICollection<DDLRoomOutput> Room { get; set; }
        public ICollection<DDLAreaOutput> Area { get; set; }
        public ICollection<DDLWorkTypeOutput> WorkType { get; set; }
        public ICollection<MaidOutput> ReportedBy { get; set; }
        public ICollection<DDLWorkStatusOutput> CurrentStatus { get; set; }
        public ICollection<DDPriorityOutput> Priority { get; set; }
        
        public ICollection<DDLTechnicianOutput> Technician { get; set; }
    }
    public class WorkOrderInfoOutput
    {
        public WorkOrderInfoOutput()
        {
           
            EnteredBy = "-";
            CompletedBy = "-";
            SignedOffBy = "-";
            CancelledBy = "-";
            LastUpdateBy = "-";
            MArea = "-1";
            MAreaDes = "";
            Technician = "-1";
            TechnicianName= "--- Please assign technician ----";
            Priority = "99";
            PriorityDesc = "-- Please select --";
            WorkOrderStatus = "0";
            WorkOrderStatusDesc = "Initial Entry";
            MWorkType = "-1";
            MWorkTypeDes = "";
            RoomKey = Guid.Empty.ToString();
            Unit = "";

        }
        public string MWorkOrderKey { get; set; }
        public string WorkDescription { get; set; }
        public string Notes { get; set; }
        public string RoomKey { get; set; }
        public string Unit { get; set; }
        public string MArea { get; set; }//
        public string MAreaDes { get; set; }//
        public string MWorkType { get; set; }//
        public string MWorkTypeDes { get; set; }//
        public string ReportedByKey { get; set; }//
        public string ReportedByName { get; set; }//
        public DateTime ReportedDate { get; set; }
        public string WorkOrderStatus { get; set; }//
        public string WorkOrderStatusDesc { get; set; }//
        public string Priority { get; set; }//
        public string PriorityDesc { get; set; }//
        public string Technician { get; set; }//
        public string TechnicianName { get; set; }//
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
      
        public string EnteredBy { get; set; }
        public string CompletedBy { get; set; }
        public string SignedOffBy { get; set; }
        public string CancelledBy { get; set; }
        public string LastUpdateBy { get; set; }
    }
    
    public class WoTimeSheetListOutput
    {
        public string WorkDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Technician { get; set; }
        public string Notes { get; set; }
        public int Seqno { get; set; }
    }
    

    public class BlockUnBlockRoomListOutput
    {
        public Guid RoomBlockKey { get; set; }
        public string Unit { get; set; }
        public string BlockDate { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        #region Mobile UI
        public string GetBlockRoomStatusSymbol { get; set; }
        public string GetEditBlockRoomButton { get; set; }
        public string BlockedBy { get; set; }
        public string UnblockedBy { get; set; } = "";
        #endregion

    }
    public class WoWorkNote
    {
        public Guid MWorkNotesKey { get; set; }
        public string Details { get; set; }
        public string CreatedOn { get; set; }
        #region MobileUI
        public string GetEditWorkNoteButton { get; set; }
        #endregion

    }
    
    public class WoSecurityAuditlist
    {
        public Guid HistoryKey { get; set; }
        public string ChangedDate { get; set; }
        public string UserName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Detail { get; set; }
    }
   
    public class DDPriorityOutput
    { 
        public int Sort { get; set; }
        public string Priority { get; set; }
    }
}

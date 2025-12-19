using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class BlockRoomEntryViewData
    {
        
             public BlockRoomEntryViewData()
        {

            DDLWorkOrder = new HashSet<DDLWorkOrderOutput>();
            DDLRoom = new HashSet<DDLRoomOutput>();
            FromDate=DateTime.Now;
            ToDate=DateTime.Now;
            DDLStatus = new HashSet<DDLStatusOutput>();
            Reason = "Maintenance";
            ddlroomkey = Guid.Empty.ToString();
            ddlroomname = "--Please select--";
        }
        public ICollection<DDLWorkOrderOutput> DDLWorkOrder { get; set; }
        public ICollection<DDLRoomOutput> DDLRoom { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public ICollection<DDLStatusOutput> DDLStatus { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public string ddlWrokOrderValue { get;set; }
        public string ddlWrokOrderName { get; set; }
        public string ddlroomkey { get; set; }
        public string ddlroomname { get; set; }


    }
    public class DDLWorkOrderOutput
    {
        public string Seqno { get; set; }
        public string Description { get; set; }
    }
    public class DDLStatusOutput
    {
        public Guid RoomStatusKey { get; set; }
        public string RoomStatus { get; set; }
    }
    public class PopupWorkNoteInput
    {
       public string litWOID { get; set; }
        public string litWOKey { get; set; }
        public string litWorkNoteKey { get; set; }
        public string Description { get; set; }
    }
}

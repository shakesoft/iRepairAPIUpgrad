using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class PopupBlockRoomViewData
    {
        public PopupBlockRoomViewData()
        {
           
            DDLRoom = new HashSet<DDLRoomOutput>();
            FromDate = DateTime.Now;
            ToDate = DateTime.Now.AddDays(1);
            DDLStatus = new HashSet<DDLStatusOutput>();
            Reason = "Maintenance";
            chkIsBlock = true;
            btnUpdate = true;
            btnDelete = true;
            ddlroomkey = Guid.Empty.ToString();
            ddlroomname = "--Please select--";
            btnAdd = true;

        }
       
        public ICollection<DDLRoomOutput> DDLRoom { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public ICollection<DDLStatusOutput> DDLStatus { get; set; }
        public string Reason { get; set; }
        public string litTitle { get; set; }
        public string litWOID { get; set; }
        public bool chkIsBlock { get; set; }
        public bool btnUpdate { get; set; }
        public bool btnDelete { get; set; }
        public string ddlroomkey { get; set; }
        public string ddlroomname { get; set; }
        public bool btnAdd { get; set; }
        public string Note { get; set; }
        public string ddlStatusname { get; set; }
        public string litRoomBlockKey { get; set; }
    }
}

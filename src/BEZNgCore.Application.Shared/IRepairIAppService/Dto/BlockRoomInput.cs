using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class BlockRoomInput
    {
        public BlockRoomInput()
        {
            chkIsBlock=true;
            FromDate=DateTime.Now;
            ToDate=DateTime.Now.AddDays(1);
            Reason = "Maintenance";
        }
        public string litWOID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }
        public bool chkIsBlock { get; set; }
        public string ddlroomkey { get; set; }
        public string ddlroomname { get; set; }
        public string Note { get; set; }
        public string ddlStatusname { get; set; }
        public string litRoomBlockKey { get; set; }
    }
}

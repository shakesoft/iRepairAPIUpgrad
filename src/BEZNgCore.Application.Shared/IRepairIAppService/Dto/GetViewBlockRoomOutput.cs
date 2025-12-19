using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetViewBlockRoomOutput
    {
        public int Seqno { get; set; }
        public string BlockDate { get; set; }
        public string Room { get; set; }
        public string WorkOrder{get;set;}
        public string Blockroomstatus { get; set; }
    }
}

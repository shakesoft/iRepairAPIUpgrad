using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetAllWoOutput
    {
        public int Seqno { get; set; }
        public  string Room { get; set; }
        public string MAreaDesc { get; set; }
        public string MWorkTypeDesc { get; set; }
        public string MTechnicianName { get; set; }
        public string MWorkOrderStatusDesc { get; set; }
        public string Description { get; set; }
        public DateTime ReportedOn { get; set; }
        public string ReportedOnDes { get; set; }
        public string MPority { get; set; }

    }
}

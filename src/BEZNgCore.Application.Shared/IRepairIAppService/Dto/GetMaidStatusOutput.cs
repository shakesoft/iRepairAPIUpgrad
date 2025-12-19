using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetMaidStatusOutput
    {
        public GetMaidStatusOutput()
        {
            Status = "";
            MaidStatusKey = "";
            BusinessDate = DateTime.Now;
        }
        public string Status { get; set; }
        public string MaidStatusKey { get; set; }
        public DateTime BusinessDate { get; set; }
    }
}

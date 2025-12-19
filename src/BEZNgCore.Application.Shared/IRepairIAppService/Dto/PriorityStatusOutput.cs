using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class PriorityStatusOutput
    {
        public string strPriorityStatus { get; set; }
        public string strPriorityDesc { get; set; }
        public PriorityStatusOutput(string status, string code) => (strPriorityStatus, strPriorityDesc) = (status, code);
    }
}

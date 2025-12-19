using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class ViewLogIROutput
    {
        public string DateTime { get; set; }
        public string LogType { get; set; }
        public string Details { get; set; }
        public DateTime ChangedDate { get; set; }

    }
}

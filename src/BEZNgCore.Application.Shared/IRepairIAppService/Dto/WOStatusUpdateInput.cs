using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class WOStatusUpdateInput
    {
        public WOStatusUpdateInput()
        {

            ddlWostatusKey = "-1";
            ddlWostatusName = "--- Please select work order status ----";
            listWO = new HashSet<string>();
            cleanStatus = "";
        }

        public string ddlWostatusKey { get; set; }
        public string ddlWostatusName { get; set; }
        public ICollection<string> listWO { get; set; }

        public string cleanStatus { get; set; }
    }
}

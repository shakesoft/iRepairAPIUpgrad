using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class AssignInput
    {
        public AssignInput()
        {
            PreviousAttendantKey = "";
            strAssignedAttendantKey = "";
            strAssignedAttendantName = "";
            PreviousAttendantName = "none";
        }
        public string PreviousAttendantKey { get; set; }
        public string strAssignedAttendantKey { get; set; }
        public string strAssignedAttendantName { get; set; }
        public string roomNo { get; set; }
        public string PreviousAttendantName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetDashboardViewData
    {
        public int GetAssignedTasks { get; set; }
        public int GetUnassignedTechWorkOrderCount { get; set; }
    }
}

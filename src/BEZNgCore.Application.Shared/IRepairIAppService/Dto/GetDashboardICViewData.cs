using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetDashboardICViewData
    {
        public int AssignedTask { get; set; }
        public int RoomToInspect { get; set; }
    }
    public class NGetDashboardICViewData
    {
        public int AssignedTask { get; set; }
        public int RoomToInspect { get; set; }
        public int GuestRequest { get; set; }
    }
    public class GetDashboardGuestViewData
    {
        public int GuestRequest { get; set; }
    }
}

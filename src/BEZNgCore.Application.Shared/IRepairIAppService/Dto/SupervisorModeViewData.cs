    using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class SupervisorModeViewData
    {
        public SupervisorModeViewData()
        {
            GetHotelFloors = new HashSet<GetHotelFloor>();
            GetRoomStatuss = new HashSet<GetRoomStatus>();
            StaffList = new HashSet<DDLAttendantOutput>();
            MaidStatusCount = new HashSet<MaidStatusListOutPut>();
        }
        public ICollection<GetHotelFloor> GetHotelFloors { get; set; }
        public ICollection<GetRoomStatus> GetRoomStatuss { get; set; }
        public ICollection<DDLAttendantOutput> StaffList { get; set; }
        public ICollection<MaidStatusListOutPut> MaidStatusCount { get; set; }
    }
}

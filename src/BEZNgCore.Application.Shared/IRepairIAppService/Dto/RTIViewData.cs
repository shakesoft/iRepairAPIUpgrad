using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class RTIViewData
    {
        public RTIViewData()
        {
            GetHotelFloors = new HashSet<GetHotelFloor>();
            GetRoomStatuss = new HashSet<GetRoomStatus>();
            StaffList = new HashSet<DDLAttendantOutput>();
        }
        public ICollection<GetHotelFloor> GetHotelFloors { get; set; }
        public ICollection<GetRoomStatus> GetRoomStatuss { get; set; }
        public ICollection<DDLAttendantOutput> StaffList { get; set; }
        public int TotoalRoomCount { get; set; }
    }
}

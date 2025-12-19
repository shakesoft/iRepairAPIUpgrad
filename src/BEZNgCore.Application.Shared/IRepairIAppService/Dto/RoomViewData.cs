using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class RoomViewData
    {
        public RoomViewData()
        {
            GetHotelFloors = new HashSet<GetHotelFloor>();
            GetRoomStatuss = new HashSet<GetRoomStatus>();
            GetGuestStatuss = new HashSet<GetGuestStatus>();
        }
        public ICollection<GetHotelFloor> GetHotelFloors { get; set; }
        public ICollection<GetRoomStatus> GetRoomStatuss { get; set; }
        public ICollection<GetGuestStatus> GetGuestStatuss { get; set; }
    }
}

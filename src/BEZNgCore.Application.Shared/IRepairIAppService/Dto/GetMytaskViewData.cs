using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetMytaskViewData
    {
        public GetMytaskViewData()
        {
            GetHotelFloors = new HashSet<GetHotelFloor>();
            GetRoomStatuss = new HashSet<GetRoomStatus>();
            MaidStatusListOutPuts = new HashSet<MaidStatusListOutPut>();
            GetGuestStatuss = new HashSet<GetGuestStatus>();
            Attendant = "";
            Assigned = "0 min (0hour 0 min)";
            StartTime = "00:00";
            ExpectedEndTime = "00:00";
        }
        public ICollection<GetHotelFloor> GetHotelFloors { get; set; }
        public ICollection<GetRoomStatus> GetRoomStatuss { get; set; }
        public ICollection<MaidStatusListOutPut> MaidStatusListOutPuts { get; set; }
        public ICollection<GetGuestStatus> GetGuestStatuss { get; set; }
        public string Attendant { get; set; }
        public string Assigned { get; set; }
        public string StartTime { get; set; }
        public string ExpectedEndTime { get; set; }
    }
   public class HouseKeeping
   {
        public string Unit { get; set; }
        public string RoomStatus { get; set; }
        public string RoomType { get; set; }

        public string Services { get; set; }
    }
    public class GetGuestStatus
    {
        public Guid? GuestStatusKey { get; set; }
        public int? StatusCode { get; set; }
        public string Status { get; set; }
    }
}

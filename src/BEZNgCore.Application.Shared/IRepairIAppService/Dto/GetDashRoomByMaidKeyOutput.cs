using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetDashRoomByMaidKeyOutput
    {
        public Guid? RoomKey { get; set; }
        public string Unit { get; set; }
        public Guid? MaidStatusKey { get; set; }
        public string MaidStatus { get; set; }
        public Guid? RoomStatusKey { get; set; }
        public string RoomStatus { get; set; }
        public Guid? ReservationKey { get; set; }
        public string RoomType { get; set; }
        public string InterconnectRoom { get; set; }
        public int? Floor { get; set; }
        public string LinenChange { get; set; }
        public int? DND { get; set; }
        public string CleaningTime { get; set; }
        public int? LinenDays { get; set; }
        public int? Bed { get; set; }
        public DateTime? CheckInDate { get; set; }
        public string CheckInTime { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string CheckOutTime { get; set; }
        public string Maid { get; set; }
        public string HMMNotes { get; set; }
        public string GuestArrived { get; set; }
        public string GuestArrivedOrign { get; set; }
        public int? Status { get; set; }
        public int? PreCheckInCount { get; set; }
        public string GuestStatus { get; set; }
        public int? Adult { get; set; }
        public int? Child { get; set; }
        public string ETA { get; set; }
        public string ETD { get; set; }
        public string Group1 { get; set; }
        public string Group2 { get; set; }
        public string Group3 { get; set; }
        public string Group4 { get; set; }
       
        
        #region for mobileui
        public string LinenChangeDes { get; set; }
        public string Items { get; set; }
        public string GetRoomDNDButton { get; set; }
        public string GetStartOrEndTaskButton { get; set; }
        public string GetGuestArrived { get; set; }
        public string PreCheckInCountDes { get; set; }
        public string RoomStatusColor { get; set; }
        public string AttendantStatusColor { get; set; }
        public string StartStatus { get; set; }
        public string LoginStaffKey { get; set; }
        public string Pax { get; set; }
        public string MarketSegment { get; set; }
        public string GetOptButton { get; set; }
        #endregion

        #region DndImg
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string imageSrc { get; set; }
        #endregion
    }
}

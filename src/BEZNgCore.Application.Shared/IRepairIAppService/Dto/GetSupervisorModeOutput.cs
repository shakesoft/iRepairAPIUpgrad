using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetSupervisorModeOutput
    {
        public Guid? RoomKey { get; set; }
        public string Unit { get; set; }
        public string MaidStatus { get; set; }
        public string RoomStatus { get; set; }
        public string RoomType { get; set; }
        public string InterconnectRoom { get; set; }
        public string LinenChange { get; set; }
        public int? DND { get; set; }
        public string Maid { get; set; }
        public string HMMNotes { get; set; }
        public string GuestArrived { get; set; }
        public string GuestArrivedOrign { get; set; }
        public int? Status { get; set; }
        public int? PreCheckInCount { get; set; }
        public Guid? ReservationKey { get; set; }
        public string GuestStatus { get; set; }
        public int? Adult { get; set; }
        public int? Child { get; set; }
        #region MobileUI
        public string Items { get; set; }
        public string Pax { get; set; }
        public string GetGuestArrived { get; set; }
        public string PreCheckInCountDes { get; set; }
        public string LinenChangeDes { get; set; }
        public string RoomStatusDes { get; set; }
        public string RoomStatusColor { get; set; }
        public string AttendantStatusColor { get; set; }
        public string GetRoomDNDButton { get; set; }
        public string GetRoomCleanButton { get; set; }
        public string GetRoomDirtyButton { get; set; }
        public string GetHistoryLog { get; set; }
        #endregion

        #region DndImg
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string imageSrc { get; set; }
        #endregion
    }
}

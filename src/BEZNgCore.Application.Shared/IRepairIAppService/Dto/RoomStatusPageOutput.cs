using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class RoomStatusPageOutput
    {
        public string Unit { get; set; }
        public string RoomType { get; set; }
        public string RoomStatus { get; set; }
        public string MaidStatus { get; set; }
        public string Guest { get; set; }
        public string Maid { get; set; }
        public int? DND { get; set; }
        public string RoomstatusTextColor { get; set; }
        public string RoomstatusPBGColor { get; set; }
        public string DNDColor { get; set; }
        public string DNDStatus { get; set; }
        public string MaidStatusTextColor { get; set; }
        public string GuestDes { get; set; }
        public string MaidDes { get; set; }
    }
}

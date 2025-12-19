using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetRoomStatus
    {
        public Guid? RoomStatusKey { get; set; }
        public string RoomStatus { get; set; }
    }
}

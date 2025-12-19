using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetHotelRoom
    {
        public Guid RoomKey { get; set; }
        public string Unit { get; set; }
    }
}

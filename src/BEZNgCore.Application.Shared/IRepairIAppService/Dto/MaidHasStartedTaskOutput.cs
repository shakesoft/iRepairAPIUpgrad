using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class MaidHasStartedTaskOutput
    {
        public Guid? MaidStatusKey { get; set; }
        public string MaidStatus { get; set; }
        public int? RoomCount { get; set; }
    }
}

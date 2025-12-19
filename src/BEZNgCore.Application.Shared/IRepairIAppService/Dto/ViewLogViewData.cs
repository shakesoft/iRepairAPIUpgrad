using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class ViewLogViewData
    {
        public ViewLogViewData()
        {

            LogType = new HashSet<LogTypeOutput>();
            Attendant = new HashSet<MaidOutput>();
            Room = new HashSet<DDLRoomOutput>();
        }
        public ICollection<LogTypeOutput> LogType { get; set; }
        public ICollection<MaidOutput> Attendant { get; set; }
        public ICollection<DDLRoomOutput> Room { get; set; }
    }
}

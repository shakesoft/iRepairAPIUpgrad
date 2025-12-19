using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class WorkOrderEntryViewData
    {
        public WorkOrderEntryViewData()
        {
            GetReportedBy = new HashSet<MaidOutput>();
            Room = new HashSet<DDLRoomOutput>();
            Area = new HashSet<DDLAreaOutput>();
            WorkType = new HashSet<DDLWorkTypeOutput>();
        }
        public ICollection<MaidOutput> GetReportedBy { get; set; }
        public ICollection<DDLRoomOutput> Room { get; set; }
        public ICollection<DDLAreaOutput> Area { get; set; }
        public ICollection<DDLWorkTypeOutput> WorkType { get; set; }
    }
}

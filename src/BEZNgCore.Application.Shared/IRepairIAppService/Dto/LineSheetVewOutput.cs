using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class LineSheetVewOutput
    {
        public LineSheetVewOutput()
        {
            AttendantCheckList = new HashSet<LineSheetOutput>();
        }
        public ICollection<LineSheetOutput> AttendantCheckList { get; set; }
        public string roomKey { get; set; }
    }
}

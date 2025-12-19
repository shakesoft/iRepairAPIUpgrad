using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class LineSheetSupVewOutput
    {
        public LineSheetSupVewOutput()
        {
            SupCheckList = new HashSet<LineSheetSupOutput>();
        }
        public ICollection<LineSheetSupOutput> SupCheckList { get; set; }
        public string roomKey { get; set; }
    }
    public class LineSheetSupOutput
    {
        public Guid InspectionChecklistKey { get; set; }
        public Guid ItemKey { get; set; }
        public string Quantity { get; set; }
        public string Combined { get; set; }
        public int Checked { get; set; }
        public bool chkLinenItem { get; set; }
    }
}

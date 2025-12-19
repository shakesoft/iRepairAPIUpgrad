using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class LineSheetOutput
    {
        public Guid LinenChecklistKey { get; set; }
        public Guid ItemKey { get; set; }
        public int Quantity { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string Combined { get; set; }
    }
}

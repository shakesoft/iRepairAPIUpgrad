using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class LostFoundOutPut
    {
        public Guid LostFoundKey { get; set; }
        public string AutoReference { get; set; }
        public string Reference { get; set; }
        public DateTime ReportedDate { get; set; }
        public string ItemName { get; set; }
        public string LostFoundStatus { get; set; }
        public string MArea { get; set; }
        public string Owner { get; set; }
        public string Founder { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public string AdditionalInfo { get; set; }
    }
}

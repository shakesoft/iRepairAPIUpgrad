using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class ViewLogOutput
    {
        public DateTime? ChangedDate { get; set; }
        public string TableName { get; set; }
        public string Detail { get; set; }
        public string ChangeDateDes { get; set; }

    }
}

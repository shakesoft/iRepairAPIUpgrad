using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class SetupTabInput
    {
        public SetupTabInput()
        {
            chkActive = true;
        }
        public string litType { get; set; }
        public string litTitle { get; set; }
        public string Description { get; set; }
        public bool chkActive { get; set; }
    }
}

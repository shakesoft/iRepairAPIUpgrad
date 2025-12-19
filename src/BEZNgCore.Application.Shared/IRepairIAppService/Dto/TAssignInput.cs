using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class TAssignInput
    {
        public TAssignInput()
        {

            ddlTechnicianKey = "-1";
            ddlTechnicianName = "--- Please assign technician ----";
            listWO = new HashSet<string>();
        }

        public string ddlTechnicianKey { get; set; }
        public string ddlTechnicianName { get; set; }
        public ICollection<string> listWO { get; set; }



    }
}

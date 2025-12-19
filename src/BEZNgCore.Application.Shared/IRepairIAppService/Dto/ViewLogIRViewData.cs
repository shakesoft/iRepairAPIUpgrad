using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class ViewLogIRViewData
    {
        public ViewLogIRViewData()
        {

            LogType = new HashSet<LogTypeIROutput>();
            Technician = new HashSet<TechnicianOutput>();
           
        }
        public ICollection<LogTypeIROutput> LogType { get; set; }
        public ICollection<TechnicianOutput> Technician { get; set; }
    }
}

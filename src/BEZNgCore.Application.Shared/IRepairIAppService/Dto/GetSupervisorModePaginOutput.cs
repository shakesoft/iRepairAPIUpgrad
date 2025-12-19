using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
   
    public class GetSupervisorModePaginOutput
    {
        public GetSupervisorModePaginOutput()
        {
            SupervisorModeList = new HashSet<GetSupervisorModeOutput>();
            PaginattionList = new HashSet<PageListItem>();
        }
        public ICollection<GetSupervisorModeOutput> SupervisorModeList { get; set; }
        public ICollection<PageListItem> PaginattionList { get; set; }
      
    }
}

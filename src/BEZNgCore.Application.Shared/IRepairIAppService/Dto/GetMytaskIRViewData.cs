using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetMytaskIRViewData
    {
        public GetMytaskIRViewData()
        {
            GetWOStatusOutput = new HashSet<WOStatusOutput>();
            GetRoomStatuss = new HashSet<GetRoomStatus>();
            GetPriorityStatusOutPuts = new HashSet<DDLMPriorityOutput>();
        }
        public ICollection<WOStatusOutput> GetWOStatusOutput { get; set; }
        public ICollection<GetRoomStatus> GetRoomStatuss { get; set; }
        public ICollection<DDLMPriorityOutput> GetPriorityStatusOutPuts { get; set; }
       
        
    }
}

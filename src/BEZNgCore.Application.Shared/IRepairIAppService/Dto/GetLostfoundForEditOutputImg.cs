using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetLostfoundForEditOutputImg
    {
        
        public GetLostfoundForEditOutputImg()
        {
            imglst = new HashSet<FWOImageD>();
        }
        public LostFoundDto LostFound { get; set; }
        public ICollection<FWOImageD> imglst { get; set; }
    }
   
}

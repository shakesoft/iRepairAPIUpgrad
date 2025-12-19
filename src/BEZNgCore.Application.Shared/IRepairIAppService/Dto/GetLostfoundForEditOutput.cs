using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetLostfoundForEditOutput
    {
        public LostFoundDto LostFound { get; set; }
    }
    public class LostFoundDto : EntityDto<Guid?>
    {

        //public int? TenantId { get; set; }
        //public Guid? LostFoundKey { get; set; }
        public Guid? LostFoundStatusKey { get; set; }
        public DateTime ReportedDate { get; set; }
        public string ItemName { get; set; }
        public int? Area { get; set; }
        public string Owner { get; set; }
        public string OwnerFolio { get; set; }
        public Guid? OwnerRoomKey { get; set; }
        public string OwnerContactNo { get; set; }
        public string Founder { get; set; }
        public string FounderFolio { get; set; }
        public Guid? FounderRoomKey { get; set; }
        public string FounderContactNo { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public string AdditionalInfo { get; set; }
        public Guid? StaffKey { get; set; }
        //public string AutoReference { get; set; }//
        public string Reference { get; set; }
    }
}

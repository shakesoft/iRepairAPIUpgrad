using Abp.Application.Services.Dto;
using BEZNgCore.IrepairControl;
using System;
using System.ComponentModel.DataAnnotations;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class CreateOrEditHistoryDto : EntityDto<Guid?>
    {
        //public int? TenantId { get; set; }

        //public  Guid? LinkKey { get; set; }

        [StringLength(HistoryConsts.MaxModuleNameLength, MinimumLength = HistoryConsts.MinModuleNameLength)]
        public string ModuleName { get; set; }

        public Guid? StaffKey { get; set; }

        [StringLength(HistoryConsts.MaxOperationLength, MinimumLength = HistoryConsts.MinOperationLength)]
        public string Operation { get; set; }

        public DateTime? ChangedDate { get; set; }

        [StringLength(HistoryConsts.MaxTableNameLength, MinimumLength = HistoryConsts.MinTableNameLength)]
        public string TableName { get; set; }

        //public Guid? SourceKey { get; set; }

        public int? Sort { get; set; }

        public int? Sync { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Seq { get; set; }

        //[Timestamp]
        //public  byte[] TS { get; set; }

        [StringLength(HistoryConsts.MaxDetailLength, MinimumLength = HistoryConsts.MinDetailLength)]
        public string Detail { get; set; }

        //[StringLength(HistoryConsts.MaxOldValueLength, MinimumLength = HistoryConsts.MinOldValueLength)]
        //public  string OldValue { get; set; }

        //[StringLength(HistoryConsts.MaxNewValueLength, MinimumLength = HistoryConsts.MinNewValueLength)]
        //public  string NewValue { get; set; }

        //[StringLength(HistoryConsts.MaxSourceLinkLength, MinimumLength = HistoryConsts.MinSourceLinkLength)]
        //public  string SourceLink { get; set; }
    }
}

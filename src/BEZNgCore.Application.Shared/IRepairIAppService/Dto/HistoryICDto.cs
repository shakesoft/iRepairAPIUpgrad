using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class HistoryICDto : EntityDto<Guid>
    {
        public Guid HistoryKey { get; set; }
        public DateTime? ChangedDate { get; set; }
        public string TableName { get; set; }
        public string Detail { get; set; }

    }
    public class HistoryICleanDto : EntityDto<Guid>
    {
        public Guid HistoryKey { get; set; }
        public DateTime? ChangedDate { get; set; }

        public string ChangedDateStr { get; set; }
        public string TableName { get; set; }
        public string Detail { get; set; }

    }
}

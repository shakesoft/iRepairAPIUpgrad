using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class ReservationOptOutModel
    {
        public Guid ReservationOptOutKey { get; set; }
        public string ReservationOptOutCode { get; set; }
        public string ReservationOptOutReason { get; set; }
        public Guid? AttendantID { get; set; }
        public Guid? ReservationKey { get; set; }
        public DateTime? OptOut { get; set; }
        public string Unit { get; set; }
        public int? TenantId { get; set; }

    }
}

using System;
using Abp.Domain.Entities.Auditing;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class MWorkOrderD : CreationAuditedEntity<int>
    {
       
        public  int Seqno { get; set; }
        public  Guid? MWorkOrderKey { get; set; }
        public  int? SqoopeWorkOrderID { get; set; }
        public  string Room { get; set; }
        public  Guid? RoomKey { get; set; }
        public  int? MArea { get; set; }
        public  int? MWorkType { get; set; }
        public  int? MWorkOrderStatus { get; set; }
        public  int? MTechnician { get; set; }
        public  string Description { get; set; }
        public  string Notes { get; set; }
        public  DateTime? ScheduledFrom { get; set; }
        public  DateTime? ScheduledTo { get; set; }
        public  string SignedOff { get; set; }
        public  string Cancelled { get; set; }
        public  string EnteredBy { get; set; }
        public  Guid? EnteredStaffKey { get; set; }
        public  DateTime? EnteredDateTime { get; set; }
        public  string CompletedBy { get; set; }
        public  Guid? CompletedStaffKey { get; set; }
        public  DateTime? CompletedDateTime { get; set; }
       
        public  string SignedOffBy { get; set; }
        public  Guid? SignedOffStaffKey { get; set; }
        public  DateTime? SignedOffDateTime { get; set; }
        
        public  string CancelledBy { get; set; }
        public  Guid? CancelledStaffKey { get; set; }
        public  DateTime? CancelledDateTime { get; set; }
        public  string LastUpdateBy { get; set; }
        public  Guid? LastUpdateStaffKey { get; set; }
        public  DateTime? LastUpdateDateTime { get; set; }
        public  string StaffName { get; set; }
        public  Guid? ReportedBy { get; set; }
        public  DateTime? ReportedOn { get; set; }
        public  int? Priority { get; set; }

    }
}

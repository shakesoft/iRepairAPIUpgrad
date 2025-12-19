using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("MWorkOrder")]
    public class MWorkOrder : Entity<int>, IMayHaveTenant
    {
        [Column("Seqno")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid? MWorkOrderKey { get; set; }
        public virtual int? SqoopeWorkOrderID { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string Room { get; set; }
        public virtual Guid? RoomKey { get; set; }
        public virtual int? MArea { get; set; }
        public virtual int? MWorkType { get; set; }
        public virtual int? MWorkOrderStatus { get; set; }
        public virtual int? MTechnician { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string Description { get; set; }
        [StringLength(2000, MinimumLength = 0)]
        public virtual string Notes { get; set; }
        public virtual DateTime? ScheduledFrom { get; set; }
        public virtual DateTime? ScheduledTo { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string SignedOff { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string Cancelled { get; set; }
        [StringLength(15, MinimumLength = 0)]
        public virtual string EnteredBy { get; set; }
        public virtual Guid? EnteredStaffKey { get; set; }
        public virtual DateTime? EnteredDateTime { get; set; }
        [StringLength(15, MinimumLength = 0)]
        public virtual string CompletedBy { get; set; }
        public virtual Guid? CompletedStaffKey { get; set; }
        public virtual DateTime? CompletedDateTime { get; set; }
        [StringLength(15, MinimumLength = 0)]
        public virtual string SignedOffBy { get; set; }
        public virtual Guid? SignedOffStaffKey { get; set; }
        public virtual DateTime? SignedOffDateTime { get; set; }
        [StringLength(15, MinimumLength = 0)]
        public virtual string CancelledBy { get; set; }
        public virtual Guid? CancelledStaffKey { get; set; }
        public virtual DateTime? CancelledDateTime { get; set; }
        [StringLength(15, MinimumLength = 0)]
        public virtual string LastUpdateBy { get; set; }
        public virtual Guid? LastUpdateStaffKey { get; set; }
        public virtual DateTime? LastUpdateDateTime { get; set; }
        [StringLength(500, MinimumLength = 0)]
        public virtual string StaffName { get; set; }
        public virtual Guid? ReportedBy { get; set; }
        public virtual DateTime? ReportedOn { get; set; }
        public virtual int? Priority { get; set; }

    }
}

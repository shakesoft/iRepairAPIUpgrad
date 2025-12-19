using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairModel
{
    [Table("ReservationRoom")]
    public class ReservationRoom : Entity<Guid>, IMayHaveTenant
    {
        [Column("ReservationRoomKey")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
    
        public virtual Guid? ReservationKey { get; set; }
        public virtual DateTime? DocDate { get; set; }
        public virtual Guid? RoomKey { get; set; }
        public virtual Guid? RoomTypeKey { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Remark { get; set; }
        public virtual decimal? Rate { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }

        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual decimal? AdditionalBed { get; set; }
        public virtual int? Alloted { get; set; }
        public virtual Guid? AllotmentLineKey { get; set; }
        public virtual int? Override { get; set; }
        public virtual decimal? OldRate { get; set; }
        public virtual decimal? OldAdditional { get; set; }
        public virtual Guid? OverrideStaffKey { get; set; }
        [StringLength(30, MinimumLength = 0)]
        public virtual string OverrideStaffName { get; set; }
        public virtual DateTime? OverrideTime { get; set; }
        public virtual Guid? OldRoomKey { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string OverrideReason { get; set; }
        public virtual decimal? PointsValue { get; set; }
        public virtual Guid? SourceKey { get; set; }
        public virtual Guid? CompanyKey { get; set; }
        public virtual Guid? TravelAgentKey { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string RateCode { get; set; }
        public virtual Guid? RateTypeKey { get; set; }
        public virtual Guid? OldRateTypeKey { get; set; }
        public virtual Guid? Group1Key { get; set; }
        public virtual Guid? Group2Key { get; set; }
        public virtual Guid? Group3Key { get; set; }
        public virtual Guid? Group4Key { get; set; }
        public virtual Guid? OrgRoomTypeKey { get; set; }
        public virtual Guid? OldRoomTypeKey { get; set; }
        public virtual Guid? OrgRatePromotionTypeKey { get; set; }
        public virtual Guid? RatePromotionTypeKey { get; set; }
        public virtual int Adult { get; set; }
        public virtual int? Child { get; set; }
        public virtual Guid? GroupBlockReservationKey { get; set; }
        public virtual Guid? CommCodeKey { get; set; }
        public virtual int? CommPayable { get; set; }
       
    }
}

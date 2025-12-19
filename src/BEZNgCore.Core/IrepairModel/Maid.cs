using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("Maid")]
    public class Maid : Entity<Guid>, IMayHaveTenant
    {
        [Column("MaidKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }

        [StringLength(50, MinimumLength = 0)]
        public virtual string FirstName { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string LastName { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] Ts { get; set; }
        [StringLength(101, MinimumLength = 0)]
        public string Name { get; set; }
        public virtual int? Active { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string MobileNo { get; set; }
        public virtual int? Floor { get; set; }
        public virtual int? Section { get; set; }
        public virtual int? Mon { get; set; }
        public virtual int? Tue { get; set; }
        public virtual int? Wed { get; set; }
        public virtual int? Thur { get; set; }
        public virtual int? Fri { get; set; }
        public virtual int? Sat { get; set; }
        public virtual int? Sun { get; set; }
        public virtual decimal? Min { get; set; }
        public virtual decimal? Max { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual Guid? RoomTypeKey { get; set; }
    }
}

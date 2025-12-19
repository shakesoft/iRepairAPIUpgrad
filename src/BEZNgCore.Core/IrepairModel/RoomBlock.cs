using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("RoomBlock")]
    public class RoomBlock : Entity<Guid>, IMayHaveTenant
    {
        [Column("RoomBlockKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual int? Active { get; set; }
        public virtual Guid? RoomKey { get; set; }
        public virtual DateTime? BlockDate { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        [StringLength(30, MinimumLength = 0)]
        public virtual string BlockStaff { get; set; }
        [StringLength(30, MinimumLength = 0)]
        public virtual string UnblockStaff { get; set; }
        public virtual DateTime? BlockTime { get; set; }
        public virtual DateTime? UnblockTime { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Reason { get; set; }
        [StringLength(512, MinimumLength = 0)]
        public virtual string Comment { get; set; }
        public virtual int? MWorkOrderNo { get; set; }

        //public virtual Room Room { get; set; }
    }
}

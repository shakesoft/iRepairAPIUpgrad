using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("RoomStatus")]
    public class RoomStatus : Entity<Guid>, IMayHaveTenant
    {
        [Column("RoomStatusKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        [Column("RoomStatus")]
        [StringLength(50, MinimumLength = 0)]
        public virtual string RoomStatusName { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] Ts { get; set; }
    }
}

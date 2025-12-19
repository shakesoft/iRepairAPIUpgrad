using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
namespace BEZNgCore.IrepairModel
{
    [Table("GuestStatus")]
    public class GuestStatus : Entity<Guid>, IMayHaveTenant
    {
        [Column("GuestStatusKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual int? StatusCode { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Status { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] Ts { get; set; }
    }
}

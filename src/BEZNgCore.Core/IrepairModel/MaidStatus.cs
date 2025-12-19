using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("MaidStatus")]
    public class MaidStatus : Entity<Guid>, IMayHaveTenant
    {
        [Column("MaidStatusKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        [Column("MaidStatus")]
        [StringLength(50, MinimumLength = 0)]
        public virtual string MaidStatusName { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
    }
}

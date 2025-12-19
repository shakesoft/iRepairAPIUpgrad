using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("SqoopeGroup")]
    public class SqoopeGroup : Entity<Guid>, IMayHaveTenant
    {
        [Column("SqoopeGroupKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual int? Active { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Code { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Description { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Remark { get; set; }
        public virtual Guid? MessageKey { get; set; }
    }
}

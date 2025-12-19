using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("SqoopeMsgGroupLink")]
    public class SqoopeMsgGroupLink : Entity<Guid>, IMayHaveTenant
    {
        [Column("SqoopeMsgGroupLinkKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid SqoopeMessageKey { get; set; }
        public virtual Guid SqoopeGroupKey { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Remark { get; set; }
        public virtual Guid? CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual Guid? ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        public virtual int? Sort { get; set; }
    }
}

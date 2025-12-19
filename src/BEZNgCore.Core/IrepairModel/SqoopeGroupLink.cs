using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{

    [Table("SqoopeGroupLink")]
    public class SqoopeGroupLink : Entity<Guid>, IMayHaveTenant
    {
        [Column("SqoopeLinkStaffkey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid? SqoopeGroupKey { get; set; }
        public virtual Guid? StaffKey { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Remark { get; set; }
        public virtual Guid? CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual Guid? ModifiedBy { get; set; }
    }
}

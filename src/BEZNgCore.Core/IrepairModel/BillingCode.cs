using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairModel
{
    [Table("BillingCode")]
    public class BillingCode : Entity<Guid>, IMayHaveTenant
    {

        [Column("BillingCodeKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual int? Active { get; set; }
        public virtual Guid? PostcodeKey { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual int? Type { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string Code { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string Description { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string MappedGroup { get; set; }
    }
}

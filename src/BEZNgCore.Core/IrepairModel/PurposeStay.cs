using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("PurposeStay")]
    public class PurposeStay : Entity<Guid>, IMayHaveTenant
    {
        [Column("PurposeStayKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        [Column("PurposeStay")]
        [StringLength(50, MinimumLength = 0)]
        public virtual string PurposeStayName { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual int? Active { get; set; }
    }
}

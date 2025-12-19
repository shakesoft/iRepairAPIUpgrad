using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("Nationality")]
    public class Nationality : Entity<Guid>, IMayHaveTenant
    {
        [Column("NationalityKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        [Column("Nationality")]
        [StringLength(50, MinimumLength = 0)]
        public virtual string NationalityName { get; set; }
        [StringLength(5, MinimumLength = 0)]
        public virtual string PassportCode { get; set; }
        public virtual Guid? FlashCodeKey { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string IDCode { get; set; }
    }
}

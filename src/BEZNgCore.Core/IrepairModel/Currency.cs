using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("Currency")]
    public class Currency : Entity<Guid>, IMayHaveTenant
    {
        [Column("CurrencyKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        [Column("Currency")]
        [StringLength(10, MinimumLength = 0)]
        public virtual string CurrencyName { get; set; }
        public virtual int? BaseCurrency { get; set; }
        public virtual int? SecondaryCurrency { get; set; }
        public virtual double? ExchangeRate { get; set; }
    }
}


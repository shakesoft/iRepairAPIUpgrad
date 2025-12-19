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
    [Table("Postcode")]
    public class Postcode : Entity<Guid>, IMayHaveTenant
    {
        [Column("PostcodeKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }

        [Column("PostCode")]
        [StringLength(50, MinimumLength = 0)]
        public string PostCodeName { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual Guid? BillingCodeKey { get; set; }
        public virtual int? Active { get; set; }
        public virtual Guid? GLCodeKey { get; set; }
        public virtual int? Tax1 { get; set; }
        public virtual int? Tax2 { get; set; }
        public virtual int? Tax3 { get; set; }
        public virtual decimal? Tax1Min { get; set; }
        public virtual decimal? Tax2Min { get; set; }
        public virtual decimal? Tax3Min { get; set; }
        public virtual decimal? Tax1Max { get; set; }
        public virtual decimal? Tax2Max { get; set; }
        public virtual decimal? Tax3Max { get; set; }
        public virtual decimal? Tax1Zero { get; set; }
        public virtual decimal? Tax2Zero { get; set; }
        public virtual decimal? Tax3Zero { get; set; }
        public virtual int? PayCommission { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public string GL { get; set; }
        public virtual Guid? CurrencyKey { get; set; }
        public virtual Guid? Tax1Key { get; set; }
        public virtual Guid? Tax2Key { get; set; }
        public virtual Guid? Tax3Key { get; set; }
        public virtual int? ARC_Sent { get; set; }
        public virtual Guid? FlashCodeKey { get; set; }
    }
}

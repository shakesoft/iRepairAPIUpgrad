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
    [Table("ReservationBillingContact")]
    public class ReservationBillingContact : Entity<Guid>, IMayHaveTenant
    {
        [Column("ReservationBillingContactKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual Guid? ReservationKey { get; set; }
        public virtual int? Billing { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string AccountType { get; set; }
        public virtual Guid? AccountKey { get; set; }
        [StringLength(15, MinimumLength = 0)]
        public virtual string InvoiceNo { get; set; }
        public virtual int Invoice { get; set; }
    }
}

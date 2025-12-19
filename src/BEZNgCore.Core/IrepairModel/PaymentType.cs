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
    [Table("PaymentType")]
    public class PaymentType : Entity<Guid>, IMayHaveTenant
    {
        [Column("PaymentTypeKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual Guid? PostCodeKey { get; set; }
        public virtual int? CompanyAR { get; set; }
        public virtual Guid? CompanyKey { get; set; }
        public virtual int? Adjustment { get; set; }
        public virtual int? CompulsoryCCNo { get; set; }
        public virtual int? CompulsoryRef { get; set; }
        public virtual int? CompulsoryName { get; set; }
        public virtual int? CompulsoryVoucherNo { get; set; }
        public virtual Guid? PaymentGroupKey { get; set; }
        public virtual byte[] Picture { get; set; }
        public virtual int? ARPayment { get; set; }
        [Column("PaymentType")]
        [StringLength(10, MinimumLength = 0)]
        public virtual string PaymentTypeName { get; set; }
        public virtual byte[]? ExternalAR { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExternalARAccNo { get; set; }
        public virtual Guid? SurchargePostCodeKey { get; set; }
        public virtual decimal? SurchargePercent { get; set; }
        public virtual decimal? SurchargeThreshold { get; set; }
        public virtual int? Payment { get; set; }
        public virtual int? Payout { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string ExtPaymentTravelClick { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string ExtPaymentSiteminder { get; set; }
        public virtual int? ARC_Sent { get; set; }
        public virtual int? Active { get; set; }
        public virtual decimal? CreditCardFee { get; set; }
        public virtual int? Offline { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string EftposCardType { get; set; }
        public virtual int? ARSurcharge { get; set; }
    }
}

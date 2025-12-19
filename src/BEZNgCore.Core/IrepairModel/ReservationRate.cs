using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("ReservationRate")]
    public class ReservationRate : Entity<Guid>, IMayHaveTenant
    {
        [Column("ReservationRateKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid? ReservationKey { get; set; }
        public virtual DateTime? ChargeDate { get; set; }
        public virtual decimal? Rate { get; set; }
        public virtual decimal? Tax1 { get; set; }
        public virtual decimal? Tax2 { get; set; }
        public virtual decimal? Tax3 { get; set; }
        public virtual int? Overwrite { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        [StringLength(512, MinimumLength = 0)]
        public virtual string OverwriteReason { get; set; }
        public virtual Guid? OverwriteStaff { get; set; }
        public virtual DateTime? OverwriteTime { get; set; }
        public virtual int? BillTo { get; set; }
        public virtual int? Status { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Ref1 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Ref2 { get; set; }
        public virtual Guid? PostCodeKey { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string Covers { get; set; }
        public virtual DateTime? PostDate { get; set; }
        public virtual Guid? RoomKey { get; set; }
        [StringLength(30, MinimumLength = 0)]
        public virtual string UserName { get; set; }
        public virtual int? Void { get; set; }
        public virtual Guid? ARTransKey { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string BillToName { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string InvoiceNo { get; set; }
        public virtual decimal? Total { get; set; }
        [StringLength(60, MinimumLength = 0)]
        public virtual string VoucherNo { get; set; }
        public virtual Guid? ShiftKey { get; set; }
        [StringLength(15, MinimumLength = 0)]
        public virtual string ShiftNo { get; set; }
        public virtual Guid? VoidSourceKey { get; set; }
        public virtual Guid? ItemKey { get; set; }
        public virtual int? Consolidated { get; set; }
        public virtual Guid? ForeignCurrencyKey { get; set; }
        public virtual decimal? ForeignAmount { get; set; }
        public virtual Guid? PaymentTypeKey { get; set; }
        public virtual Guid? StaffKey { get; set; }
        public virtual decimal? SecondaryAmount { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Description { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string CardName { get; set; }
        public virtual decimal? ForeignExchangeRate { get; set; }
        public virtual decimal? SecondaryExchangeRate { get; set; }
        public virtual byte? AdditionalBed { get; set; }
        public virtual int? RedemptPoint { get; set; }
        public virtual int? AwardedPoint { get; set; }
        public virtual Guid? RateTypeKey { get; set; }
        [StringLength(30, MinimumLength = 0)]
        public virtual string CardCVV { get; set; }
        public virtual int? BatchInvoicePrint { get; set; }
        public virtual Guid? OrgSourcePostCodeKey { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string OrgSourceDescription { get; set; }
        public virtual DateTime? OrgSourcePostDate { get; set; }
    }
}

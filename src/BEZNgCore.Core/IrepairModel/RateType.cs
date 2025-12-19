using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("RateType")]
    public class RateType : Entity<Guid>, IMayHaveTenant
    {
        [Column("RateTypeKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual int? Active { get; set; }
        public virtual int? NoOfNight { get; set; }
        public virtual int? Priority { get; set; }
        public virtual int? Override { get; set; }
        public virtual Guid? LastModifiedStaff { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual int? ApplicableForAll { get; set; }
        public virtual Guid PostCodeKey { get; set; }
        public virtual Guid? RateTypeGroupingKey { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string PrintRate { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string RateCode { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Description { get; set; }
        [StringLength(2048, MinimumLength = 0)]
        public virtual string Notes { get; set; }
        public virtual Guid? RateTypeKeyBase { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string RateLinkAmount { get; set; }
        public virtual int? RateLinkType { get; set; }
        public virtual int? RateLinkRound { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string BAR { get; set; }
        public virtual int? ChannelSet { get; set; }
        public virtual int AdvanceDay { get; set; }
        public virtual int Online { get; set; }
        public virtual int MaxAdvanceDay { get; set; }
        public virtual int ChannelSet01 { get; set; }
        public virtual int ChannelSet02 { get; set; }
        public virtual int ChannelSet03 { get; set; }
        public virtual int ChannelSet04 { get; set; }
        public virtual int ChannelSet05 { get; set; }
        public virtual int ChannelSet06 { get; set; }
        public virtual int ChannelSet07 { get; set; }
        public virtual int ChannelSet08 { get; set; }
        public virtual int ChannelSet09 { get; set; }
        public virtual int ChannelSet10 { get; set; }
        public virtual int ChannelSet11 { get; set; }
        public virtual int ChannelSet12 { get; set; }
        public virtual int Promo { get; set; }
        public virtual Guid? Group1Key { get; set; }
        public virtual Guid? Group2Key { get; set; }
        public virtual Guid? Group3Key { get; set; }
        public virtual Guid? Group4Key { get; set; }
        [StringLength(2048, MinimumLength = 0)]
        public virtual string TermsConditions { get; set; }
        [StringLength(2048, MinimumLength = 0)]
        public virtual string CancellationPolicy { get; set; }
        public virtual decimal ExtraAdultRate { get; set; }
        public virtual int FirstNight { get; set; }
        public virtual Guid? CommCodeKey { get; set; }
        public virtual int? CommPayable { get; set; }
        public virtual Guid? CancellationRuleKey { get; set; }
    }
}

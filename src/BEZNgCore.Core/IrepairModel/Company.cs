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
    [Table("Company")]
    public class Company : Entity<Guid>, IMayHaveTenant
    {
        [Column("CompanyKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual int? Type { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string AccNo { get; set; }
        public virtual int? Status { get; set; }
        public virtual int? Active { get; set; }
        [StringLength(15, MinimumLength = 0)]
        public virtual string IATA { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string RegistrationNo { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Tel { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Fax { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string EMail { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string Mailing_Postal { get; set; }
        public virtual Guid? Mailing_CountryKey { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string Billing_Postal { get; set; }
        public virtual Guid? Billing_CountryKey { get; set; }
        public virtual decimal? CreditLimit { get; set; }
        public virtual int? StopCredit { get; set; }
        public virtual int? Terms { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string Billing { get; set; }
        [StringLength(30, MinimumLength = 0)]
        public virtual string Web { get; set; }
        public virtual Guid? Group1Key { get; set; }
        public virtual Guid? Group2Key { get; set; }
        public virtual Guid? Group3Key { get; set; }
        public virtual Guid? Group4Key { get; set; }
        public virtual Guid? SourceKey { get; set; }
        public virtual Guid? StaffKey { get; set; }
        public virtual decimal? CommissionRate { get; set; }
        public virtual int? PayCommission { get; set; }
        public virtual int? ChargeBack { get; set; }
        public virtual Guid? LastModifiedStaff { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual Guid? Mailing_RegionKey { get; set; }
        public virtual Guid? Billing_RegionKey { get; set; }
        public virtual string Contract_Remark { get; set; }
        public virtual string Company_Remark { get; set; }
        public virtual int? GST { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExtraField1 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExtraField2 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExtraField3 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExtraField4 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExtraField5 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExtraField6 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExtraField7 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExtraField8 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExtraField9 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExtraField10 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExtraField11 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ExtraField12 { get; set; }
        public virtual Guid? EventTypeKey { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string Shortcode { get; set; }
        [StringLength(64, MinimumLength = 0)]
        public virtual string Name { get; set; }
        [StringLength(120, MinimumLength = 0)]
        public virtual string Mailing_Address { get; set; }
        [StringLength(120, MinimumLength = 0)]
        public virtual string Billing_Address { get; set; }
        [Column("Company")]
        [StringLength(77, MinimumLength = 0)]
        public virtual string CompanyName { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Billing_City { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Mailing_City { get; set; }
        public virtual int? PMTSurchageOptOut { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string OnlinePwd { get; set; }
        public virtual decimal? InterfaceCommission { get; set; }
        public virtual Guid? Users { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string Computer { get; set; }
        public virtual DateTime? Access { get; set; }
        public virtual Guid? CommCodeKey { get; set; }
        public virtual int? CommPayable { get; set; }
    }
}

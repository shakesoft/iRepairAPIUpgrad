using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("Guest")]
    public class Guest : Entity<Guid>, IMayHaveTenant
    {
        [Column("GuestKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }

        [StringLength(20, MinimumLength = 0)]
        public virtual string AccNo { get; set; }
        public virtual int? Status { get; set; }
        public virtual int? Active { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string CarNo { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string Gender { get; set; }
        public virtual DateTime? DOB { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Tel { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Mobile { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Fax { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string EMail { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string Postal { get; set; }
        public virtual Guid? CountryKey { get; set; }
        public virtual Guid? NationalityKey { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Interest { get; set; }
        public virtual decimal? CreditLimit { get; set; }
        public virtual int? Terms { get; set; }
        public virtual Guid? Group1Key { get; set; }
        public virtual Guid? Group2Key { get; set; }
        public virtual Guid? Group3Key { get; set; }
        public virtual Guid? Group4Key { get; set; }
        public virtual Guid? SourceKey { get; set; }
        public virtual Guid? StaffKey { get; set; }
        public virtual int? DefaultCompany { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Company1Name { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Company1Relation { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Company1Department { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Company1Occupation { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Company2Name { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Company2Relation { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Company2Department { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Company2Occupation { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Company3Name { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Company3Relation { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Company3Department { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Company3Occupation { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Company4Name { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Company4Relation { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Company4Department { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Company4Occupation { get; set; }
        public virtual Guid? LastModifiedStaff { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual DateTime? PassportExpiry { get; set; }
        public virtual int? Extra1 { get; set; }
        public virtual int? Extra2 { get; set; }
        public virtual int? Extra3 { get; set; }
        public virtual int? Extra4 { get; set; }
        public virtual int? Extra5 { get; set; }
        public virtual int? Extra6 { get; set; }
        public virtual int? Extra7 { get; set; }
        public virtual int? Extra8 { get; set; }
        public virtual int? Extra9 { get; set; }
        public virtual int? Extra10 { get; set; }
        public virtual int? Extra11 { get; set; }
        public virtual int? Extra12 { get; set; }
        public virtual int? Extra13 { get; set; }
        public virtual int? Extra14 { get; set; }
        public virtual int? Extra15 { get; set; }
        public virtual int? Extra16 { get; set; }
        public virtual int? Extra17 { get; set; }
        public virtual int? Extra18 { get; set; }
        public virtual int? Extra19 { get; set; }
        public virtual int? Extra20 { get; set; }
        public virtual int? Extra21 { get; set; }
        public virtual int? Extra22 { get; set; }
        public virtual int? Extra23 { get; set; }
        public virtual int? Extra24 { get; set; }
        public virtual Guid? RegionKey { get; set; }
        public virtual int? GuestStay { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Title { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Company { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string LastName { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string FirstName { get; set; }
        [StringLength(110, MinimumLength = 0)]
        public virtual string Name { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string ShortCode { get; set; }
        [Column("Guest")]
        [StringLength(123, MinimumLength = 0)]
        public virtual string GuestName { get; set; }
        [StringLength(120, MinimumLength = 0)]
        public virtual string Address { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string City { get; set; }
        public virtual Guid? GuestIdentityTypeKey { get; set; }
        [StringLength(60, MinimumLength = 0)]
        public virtual string Passport { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string LanguageCode { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string Subscribe { get; set; }
        public virtual Guid? PropertyKey { get; set; }
        public virtual Guid? OrgGuestKey { get; set; }
        [StringLength(15, MinimumLength = 0)]
        public virtual string OrgAccNo { get; set; }
        public virtual int? DoNotContact { get; set; }
        public virtual int? OldGuestStay { get; set; }
        public virtual Guid? Users { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string Computer { get; set; }
        public virtual DateTime? Access { get; set; }
        [StringLength(60, MinimumLength = 0)]
        public virtual string tPassport { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string X_Company { get; set; }
        [StringLength(120, MinimumLength = 0)]
        public virtual string X_Dorm { get; set; }
        [StringLength(120, MinimumLength = 0)]
        public virtual string X_Sector { get; set; }

    }
}

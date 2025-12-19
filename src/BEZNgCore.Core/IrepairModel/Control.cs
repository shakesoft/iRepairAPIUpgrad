using Abp.Domain.Entities;
using BEZNgCore.IrepairControl;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("Control")]
    public class Control : Entity<Guid>, IMayHaveTenant
    {

        [Column("ControlKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }

        [StringLength(ControlConsts.MaxNameLength, MinimumLength = ControlConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(ControlConsts.MaxTelLength, MinimumLength = ControlConsts.MinTelLength)]
        public virtual string Tel { get; set; }

        [StringLength(ControlConsts.MaxFaxLength, MinimumLength = ControlConsts.MinFaxLength)]
        public virtual string Fax { get; set; }

        [StringLength(ControlConsts.MaxEMailLength, MinimumLength = ControlConsts.MinEMailLength)]
        public virtual string EMail { get; set; }

        [StringLength(ControlConsts.MaxAddressLength, MinimumLength = ControlConsts.MinAddressLength)]
        public virtual string Address { get; set; }

        [StringLength(ControlConsts.MaxCityLength, MinimumLength = ControlConsts.MinCityLength)]
        public virtual string City { get; set; }

        [StringLength(ControlConsts.MaxPostalLength, MinimumLength = ControlConsts.MinPostalLength)]
        public virtual string Postal { get; set; }

        public virtual Guid CountryKey { get; set; }

        [StringLength(ControlConsts.MaxGuestPrefixLength, MinimumLength = ControlConsts.MinGuestPrefixLength)]
        public virtual string GuestPrefix { get; set; }

        [StringLength(ControlConsts.MaxCompanyPrefixLength, MinimumLength = ControlConsts.MinCompanyPrefixLength)]
        public virtual string CompanyPrefix { get; set; }

        [StringLength(ControlConsts.MaxTravelAgentPrefixLength, MinimumLength = ControlConsts.MinTravelAgentPrefixLength)]
        public virtual string TravelAgentPrefix { get; set; }

        [StringLength(ControlConsts.MaxContactPrefixLength, MinimumLength = ControlConsts.MinContactPrefixLength)]
        public virtual string ContactPrefix { get; set; }

        [StringLength(ControlConsts.MaxFolioPrefixLength, MinimumLength = ControlConsts.MinFolioPrefixLength)]
        public virtual string FolioPrefix { get; set; }

        public virtual int? Sort { get; set; }

        public virtual int? Sync { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }


        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual int? Version { get; set; }

        public virtual DateTime? SystemDate { get; set; }

        public virtual DateTime? PMSStartDate { get; set; }

        [StringLength(ControlConsts.MaxLicenseKeyLength, MinimumLength = ControlConsts.MinLicenseKeyLength)]
        public virtual string LicenseKey { get; set; }

        public virtual byte[] Logo { get; set; }

        [StringLength(ControlConsts.MaxLogoFileNameLength, MinimumLength = ControlConsts.MinLogoFileNameLength)]
        public virtual string LogoFileName { get; set; }

        public virtual int? OEM { get; set; }

        [StringLength(ControlConsts.MaxMaintenanceKeyLength, MinimumLength = ControlConsts.MinMaintenanceKeyLength)]
        public virtual string MaintenanceKey { get; set; }

        [StringLength(ControlConsts.MaxEventManagerKeyLength, MinimumLength = ControlConsts.MinEventManagerKeyLength)]
        public virtual string EventManagerKey { get; set; }

    }
}

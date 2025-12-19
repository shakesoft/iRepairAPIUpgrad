using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("GeneralProfile")]
    public class GeneralProfile : Entity<Guid>, IMayHaveTenant
    {
        [Column("GeneralProfileKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid? SetupKey { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string ProfileName { get; set; }
        [StringLength(500, MinimumLength = 0)]
        public virtual string ProfileValue { get; set; }
        public virtual Guid? LastModifiedStaff { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }

        [Timestamp]
        public virtual byte[] TS { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Description { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string DataType { get; set; }
    }
}

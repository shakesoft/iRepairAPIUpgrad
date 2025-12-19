using Abp.Domain.Entities;
using BEZNgCore.IrepairControl;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("Title")]
    public class Title : Entity<Guid>, IMayHaveTenant
    {
        [Column("TitleKey")]
        public override Guid Id { get; set; }

        public int? TenantId { get; set; }

        [Column("Title")]
        [StringLength(TitleConsts.MaxTitleLength, MinimumLength = TitleConsts.MinTitleLength)]
        public virtual string TitleName { get; set; }

        public virtual int? Sort { get; set; }

        public virtual int? Sync { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }

        [Timestamp]
        public virtual byte[] TS { get; set; }

        public virtual int? Active { get; set; }

    }
}

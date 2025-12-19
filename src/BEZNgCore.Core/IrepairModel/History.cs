using Abp.Domain.Entities;
using BEZNgCore.IrepairControl;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("History")]
    public class History : Entity<Guid>, IMayHaveTenant
    {
        [Column("HistoryKey")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }

        public virtual Guid? LinkKey { get; set; }

        [StringLength(HistoryConsts.MaxModuleNameLength, MinimumLength = HistoryConsts.MinModuleNameLength)]
        public virtual string ModuleName { get; set; }

        public virtual Guid? StaffKey { get; set; }

        [StringLength(HistoryConsts.MaxOperationLength, MinimumLength = HistoryConsts.MinOperationLength)]
        public virtual string Operation { get; set; }

        public virtual DateTime? ChangedDate { get; set; }

        [StringLength(HistoryConsts.MaxTableNameLength, MinimumLength = HistoryConsts.MinTableNameLength)]
        public virtual string TableName { get; set; }

        public virtual Guid? SourceKey { get; set; }

        public virtual int? Sort { get; set; }

        public virtual int? Sync { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }

        [Timestamp]
        public virtual byte[] TS { get; set; }

        [StringLength(HistoryConsts.MaxDetailLength, MinimumLength = HistoryConsts.MinDetailLength)]
        public virtual string Detail { get; set; }

        [StringLength(HistoryConsts.MaxOldValueLength, MinimumLength = HistoryConsts.MinOldValueLength)]
        public virtual string OldValue { get; set; }

        [StringLength(HistoryConsts.MaxNewValueLength, MinimumLength = HistoryConsts.MinNewValueLength)]
        public virtual string NewValue { get; set; }

        [StringLength(HistoryConsts.MaxSourceLinkLength, MinimumLength = HistoryConsts.MinSourceLinkLength)]
        public virtual string SourceLink { get; set; }

    }
}

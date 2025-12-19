using Abp.Domain.Entities;
using BEZNgCore.IrepairControl;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("Item")]
    public class Item : Entity<Guid>, IMayHaveTenant
    {
        [Column("ItemKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual int? Active { get; set; }
        public virtual decimal? SalesPrice { get; set; }
        public virtual decimal? CostPrice { get; set; }
        public virtual Guid? LastModifiedStaff { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual Guid? GLKey { get; set; }
        public virtual Guid PostCodeKey { get; set; }
        public virtual int? HKg { get; set; }
        public virtual int? Rgn { get; set; }
        public virtual int? Cfm { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string Calculator { get; set; }
        public virtual int? To_GH { get; set; }
        public virtual int? Fm_GH { get; set; }
        public virtual int? Flag1 { get; set; }
        public virtual int? Flag2 { get; set; }
        public virtual int? Flag3 { get; set; }
        public virtual int? Flag4 { get; set; }
        public virtual int? Flag5 { get; set; }
        public virtual int? ChargeDateOffSet { get; set; }
        public virtual int? News { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Code { get; set; }
        [StringLength(500, MinimumLength = 0)]
        public virtual string Description { get; set; }
        public virtual decimal? SalesPrice01 { get; set; }
        public virtual decimal? SalesPrice02 { get; set; }
        public virtual decimal? SalesPrice03 { get; set; }
        public virtual decimal? SalesPrice04 { get; set; }
        public virtual decimal? SalesPrice05 { get; set; }
        public virtual decimal? SalesPrice06 { get; set; }
        public virtual decimal? SalesPrice07 { get; set; }
        public virtual decimal? SalesPrice08 { get; set; }
        public virtual decimal? SalesPrice09 { get; set; }
        public virtual decimal? SalesPrice10 { get; set; }
        public virtual int? Online { get; set; }
        public virtual int? Minibar { get; set; }
        public virtual int? Laundry { get; set; }
        public virtual int? Quantity { get; set; }
        public virtual int? Lookup { get; set; }
    }
}

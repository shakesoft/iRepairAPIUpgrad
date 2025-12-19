using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("MTechnician")]
    public class MTechnician : Entity<int>, IMayHaveTenant
    {
        [Column("Seqno")]
        public override int Id { get; set; }
        public int? TenantId { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string Name { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string CompanyName { get; set; }
        [StringLength(1, MinimumLength = 0)]
        public virtual string Contractor { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string OPhone { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string MPhone { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Fax { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Pager { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Email { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Note { get; set; }
        public virtual Guid? TechnicianKey { get; set; }
        public virtual int Active { get; set; }
        public virtual Guid? CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual Guid? ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
    }
}

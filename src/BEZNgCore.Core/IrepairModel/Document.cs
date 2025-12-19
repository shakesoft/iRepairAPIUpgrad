using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
   
    [Table("Document")]
    public class Document : Entity<Guid>, IMayHaveTenant
    {
        [Column("DocumentKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid? GuestKey { get; set; }
        public virtual Guid? CompanyKey { get; set; }
        public virtual Guid? LastModifiedStaff { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual byte[] DocumentStore { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string Description { get; set; }
        [Column("Document")]
        [StringLength(100, MinimumLength = 0)]
        public virtual string Document1 { get; set; }
        public virtual Guid? ReservationKey { get; set; }
        public virtual byte[] Signature { get; set; }

        //public virtual Company Company { get; set; }
        //public virtual Guest Guest { get; set; }
    }
}

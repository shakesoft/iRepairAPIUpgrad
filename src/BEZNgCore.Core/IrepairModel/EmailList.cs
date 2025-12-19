using Abp.Domain.Entities;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairModel
{
    [Table("EmailList")]
    public class EmailList : Entity<Guid>, IMayHaveTenant
    {
        [Column("mail_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string mac_id { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string batch_id { get; set; }
        [StringLength(254, MinimumLength = 0)]
        public virtual string mail_from { get; set; }
        [StringLength(254, MinimumLength = 0)]
        public virtual string mail_to { get; set; }
        [StringLength(4000, MinimumLength = 0)]
        public virtual string mail_subject { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 0)]
        public virtual string mail_body { get; set; }
        public virtual int mail_status { get; set; }
        public virtual int mail_retry { get; set; }
        public virtual int sent_count { get; set; }
        public virtual int sent_repeat { get; set; }
        public virtual int mail_priority { get; set; }
        public virtual DateTime date_tosend { get; set; }
        public virtual DateTime? date_sent { get; set; }
        public virtual DateTime date_created { get; set; }
        public virtual DateTime? date_modified { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string user_created { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string user_modified { get; set; }
        [StringLength(4000, MinimumLength = 0)]
        public virtual string error_msg { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string doc_no { get; set; }
        public virtual Guid? sourcekey { get; set; }
        public virtual Guid? reportschedulekey { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string module { get; set; }
    }
}

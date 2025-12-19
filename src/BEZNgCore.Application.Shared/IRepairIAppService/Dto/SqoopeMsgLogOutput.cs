using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class SqoopeMsgLogOutput
    {
        public Guid Id { get; set; }
        // public int? TenantId { get; set; }
        //  public  Guid? SqoopeMessageKey { get; set; }
        //  public  int? FromContactId { get; set; }
        //  public  int? ToContactId { get; set; }
        public string Msg { get; set; }
        //  public  int? SqoopeMsgId { get; set; }
        //  public  string SqoopeMsgCreatedTS { get; set; }
        //  public  DateTime? SqoopeMsgCreatedOn { get; set; }
        //  public  string SqoopeMsgResCode { get; set; }
        public Guid? FromStaffKey { get; set; }//CreatedBy
        public DateTime? CreatedOn { get; set; }
        // public  Guid? ModifiedBy { get; set; }
        //  public  DateTime? ModifiedOn { get; set; }
        //  public  int Seq { get; set; }
        public bool Read { get; set; }
        public bool Send { get; set; }
        public Guid? ToStaffKey { get; set; }
    }
}

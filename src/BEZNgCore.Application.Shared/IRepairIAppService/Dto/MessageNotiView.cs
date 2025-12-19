using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class MessageNotiView
    {
        public MessageNotiView()
        {
            ToStaffList = new HashSet<ToStaffList>();
        }
        public ICollection<ToStaffList> ToStaffList { get; set; }
        public string Message { get; set; }
    }
    public class ToStaffList
    {
        public string to { get; set; }
    }
}

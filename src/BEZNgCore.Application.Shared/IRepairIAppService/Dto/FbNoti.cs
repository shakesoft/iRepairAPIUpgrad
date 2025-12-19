using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class FbNoti
    {
        public FbNoti()
        {
            priority = "high";
        }

        public string[] registration_ids { get; set; }
        public string priority { get; set; }
        public Notification notification { get; set; }

    }
    public class Notification
    {
        public string body { get; set; }
        public string title { get; set; }
    }
}

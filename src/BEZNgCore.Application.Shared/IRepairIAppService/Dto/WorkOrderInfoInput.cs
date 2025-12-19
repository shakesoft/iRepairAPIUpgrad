using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class WorkOrderInfoInput
    {
      
            public WorkOrderInfoInput()
            {
               
                MArea = "-1";
                Technician = "-1";
                Priority = "99";
                RoomKey = Guid.Empty.ToString();
                MWorkType = "-1";
                WorkOrderStatus = "-1";
                mode = "noreq";

        }
        public string seqno { get; set; }//
        public string MWorkOrderKey { get; set; }//
        public string WorkOrderStatus { get; set; }//
        public string WorkOrderStatusDesc { get; set; }//
        public string Notes { get; set; }//
        public string RoomKey { get; set; }//
        public string Unit { get; set; }//
        public string MArea { get; set; }//
        public string MAreaDes { get; set; }//
        public string MWorkType { get; set; }//
        public string MWorkTypeDes { get; set; }//
        public string ReportedByKey { get; set; }//
        public string ReportedByName { get; set; }//
        public DateTime ReportedDate { get; set; }//
        public string Priority { get; set; }//
        public string PriorityDesc { get; set; }//
        public string WorkDescription { get; set; }//
        public string Technician { get; set; }//
        public string TechnicianName { get; set; }//
        public DateTime? ScheduledStart { get; set; }//
        public DateTime? ScheduledEnd { get; set; }//
        public string mode { get; set; }//

    }

    public class WorkOrderInfoImgInput
    {

        public WorkOrderInfoImgInput()
        {

            MArea = "-1";
            Technician = "-1";
            Priority = "99";
            RoomKey = Guid.Empty.ToString();
            MWorkType = "-1";
            WorkOrderStatus = "-1";
            mode = "noreq";
            imglst = new HashSet<FWOImage>();

        }
        public string seqno { get; set; }//
        public string MWorkOrderKey { get; set; }//
        public string WorkOrderStatus { get; set; }//
        public string WorkOrderStatusDesc { get; set; }//
        public string Notes { get; set; }//
        public string RoomKey { get; set; }//
        public string Unit { get; set; }//
        public string MArea { get; set; }//
        public string MAreaDes { get; set; }//
        public string MWorkType { get; set; }//
        public string MWorkTypeDes { get; set; }//
        public string ReportedByKey { get; set; }//
        public string ReportedByName { get; set; }//
        public DateTime ReportedDate { get; set; }//
        public string Priority { get; set; }//
        public string PriorityDesc { get; set; }//
        public string WorkDescription { get; set; }//
        public string Technician { get; set; }//
        public string TechnicianName { get; set; }//
        public DateTime? ScheduledStart { get; set; }//
        public DateTime? ScheduledEnd { get; set; }//
        public string mode { get; set; }//
        public ICollection<FWOImage> imglst { get; set; }

    }
}

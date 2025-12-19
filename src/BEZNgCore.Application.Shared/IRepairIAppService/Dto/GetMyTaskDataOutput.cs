using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetMyTaskDataOutput
    {
        
        public  Guid? MWorkOrderKey { get; set; }
        public int Id { get; set; }
        public  string Description { get; set; }
        public string WorkOrderStatus { get; set; }//
        public  string Notes { get; set; }
        public  string Room { get; set; }
        public string RoomStatus { get; set; }//
        public  Guid? RoomKey { get; set; }
        public string Area { get; set; }//
        public string WorkType { get; set; }//
        public  string StaffName { get; set; }
        public  DateTime ReportedOn { get; set; }
        public string Priority { get; set; }

        #region Mobileui
        public string WorkOrderStatusColor { get; set; }
        public string PriorityColor { get; set; }
        public string WOStatusButton { get; set; }
        public string BlockRoomButton { get; set; }
        public string TStartOrEndTaskButton { get; set; }
        public string DescriptionByLength { get; set; }
        public string WOMaidStatus { get; set; }
        public string ColorRoomStatus { get; set; }
        public string DateToDisplay { get; set; }

        
        #endregion
    }
}

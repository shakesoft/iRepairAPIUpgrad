using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class PopupWorkOrderStatusViewData
    {
        public PopupWorkOrderStatusViewData()
        {

            WorkOrderStatus = new HashSet<DDLWorkStatusOutput>();
            listWO = new HashSet<string>();
        }
        public ICollection<DDLWorkStatusOutput> WorkOrderStatus { get; set; }
        public ICollection<string> listWO { get; set; }
        public string ddlWorkOrderStatusSelectedValue { get; set; }
    }

}

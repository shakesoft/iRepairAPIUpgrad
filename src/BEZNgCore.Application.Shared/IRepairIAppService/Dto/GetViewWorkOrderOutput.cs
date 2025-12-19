using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class GetViewWorkOrderOutput
    {
        public GetViewWorkOrderOutput()
        {

            vWorkOrderOutputList = new HashSet<VWorkOrderOutput>();
        }
        public ICollection<VWorkOrderOutput> vWorkOrderOutputList { get; set; }
        public string WorkOrderStatus { get; set; }
    }
    public class VWorkOrderOutput
    {
        public Guid? MWorkOrderKey { get; set; }
        public int Seqno { get; set; }
        public string Description { get; set; }
        public string WorkOrderStatus { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Room { get; set; }
        public string Area { get; set; }
        public string WorkType { get; set; }
        public string StaffName { get; set; }
        public string ReportedOn { get; set; }
    }
}

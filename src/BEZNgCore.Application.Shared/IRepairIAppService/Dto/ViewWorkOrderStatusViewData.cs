using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    
    public class ViewWorkOrderStatusViewData
    {
        public ViewWorkOrderStatusViewData()
        {
            DDLTechnician = new HashSet<DDLTechnicianOutput>();
            DDLArea = new HashSet<DDLAreaOutput>();
            DDLRoom = new HashSet<DDLRoomOutput>();
            DDLWorkStatus= new HashSet<DDLWorkStatusOutput>();
            DDLWorkType = new HashSet<DDLWorkTypeOutput>();
            DDLCreatedBy= new HashSet<DDLCreatedByOutput>();
            DDLMPriority= new HashSet<DDLMPriorityOutput>();
        }
        public ICollection<DDLTechnicianOutput> DDLTechnician { get; set; }
        public ICollection<DDLAreaOutput> DDLArea { get; set; }
        public ICollection<DDLRoomOutput> DDLRoom { get; set; }
        public ICollection<DDLWorkStatusOutput> DDLWorkStatus { get; set; }
        public ICollection<DDLWorkTypeOutput> DDLWorkType { get; set; }
        public ICollection<DDLCreatedByOutput> DDLCreatedBy { get; set; }
        public ICollection<DDLMPriorityOutput> DDLMPriority { get; set; }
        
    }
    public class DDLWorkStatusOutput
    {
        public string Seqno { get; set; }
        public string Description { get; set; }
    }
    public class DDLCreatedByOutput
    {
        public Guid StaffKey { get; set; }
        public string UserName { get; set; }
    }


}

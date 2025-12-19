using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class SetupViewData
    {
        public SetupViewData()
        {
            Status = new HashSet<SetupTabOutput>();
            Type = new HashSet<SetupTabOutput>();
            Area = new HashSet<SetupTabOutput>();
            Template = new HashSet<SetupTabOutput>();
            Priority = new HashSet<MPriorityOutput>();
            Technician = new HashSet<MTechnicianOutput>();
        }
        public ICollection<SetupTabOutput> Status { get; set; }
        public ICollection<SetupTabOutput> Type { get; set; }
        public ICollection<SetupTabOutput> Area { get; set; }
        public ICollection<SetupTabOutput> Template { get; set; }
        public ICollection<MPriorityOutput> Priority { get; set; }
        public ICollection<MTechnicianOutput> Technician { get; set; }
    }
    public class DDLMPriorityOutput
    {
        public string strPriorityStatus { get; set; }
        public string strPriorityDesc { get; set; }
      
    }
    public class MPriorityOutput
    {
        public int PriorityID { get; set; }
        public  string Priority { get; set; }
        public  int? Sort { get; set; }
        public bool ActiveStatus { get; set; }
    }
    public class ExternalAccessOutput
    {
        public Guid? ExternalAccessKey { get; set; }
        public string Start { get; set; }
        public string END { get; set; }

    }
    public class MPriorityModel
    {
        public  int PriorityID { get; set; }
        public int? TenantId { get; set; }
        public string Priority { get; set; }
        public int? Sort { get; set; }
        public int Active { get; set; }
    }
    public class MTechnicianOutput
    {
        public int Seqno { get; set; }
        public  string Name { get; set; }
        public string MPhone { get; set; }
        public string Email { get; set; }
        public  string CompanyName { get; set; }
        public  bool ContractorStatus { get; set; }
        public  string OPhone { get; set; }
        public  string Fax { get; set; }
        public bool ActiveStatus{ get; set; }
        public  string Note { get; set; }
    }
   

}

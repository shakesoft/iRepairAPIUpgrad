using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class ViewBlockRoomViewData
    {

        public ViewBlockRoomViewData()
        {

            DDLTechnician = new HashSet<DDLTechnicianOutput>();
            DDLRoom = new HashSet<DDLRoomOutput>();
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;

        }
        public ICollection<DDLTechnicianOutput> DDLTechnician { get; set; }
        public ICollection<DDLRoomOutput> DDLRoom { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
    public class DDLTechnicianOutput
    {
        public string Seqno { get; set; }
        public string Name { get; set; }
    }
    public class ViewSystemDtae
    {

        public ViewSystemDtae()
        {

           
            SystemDate = DateTime.Now;

        }
       
        public DateTime SystemDate { get; set; }

    }
}

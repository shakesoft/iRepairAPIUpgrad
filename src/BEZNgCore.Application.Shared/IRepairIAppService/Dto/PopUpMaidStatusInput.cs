using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class PopUpMaidStatusInput
    {
        public PopUpMaidStatusInput()
        {
            Note = "";
            phy = "";
        }
        public string strMode { get; set; }
        public string strRoomNo { get; set; }
        public string Note { get; set; }
        public string phy { get; set; }
        public string roomKey { get; set; }
    }
}

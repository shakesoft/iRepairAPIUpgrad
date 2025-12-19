using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class SalesPrice
    {
        public SalesPrice()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public double salesPrice { get; set; }
        public double salesTax1 { get; set; }
        public double salesTax2 { get; set; }
        public double salesTax3 { get; set; }
        public double salesTotal { get; set; }

    }
}

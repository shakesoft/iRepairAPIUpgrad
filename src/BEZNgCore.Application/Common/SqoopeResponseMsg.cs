using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.Common
{
    [Serializable]
    public class SqoopeResponseMsg
    {
        #region Constructor
        public SqoopeResponseMsg() { }
        #endregion

        #region Members

        #endregion

        #region Properties
        public string mid { get; set; }

        public string created { get; set; }

        #endregion

    }

    public class SqoopeResponse
    {
        public string resp_code { get; set; }
        public SqoopeResponseMsg sqoope_msg { get; set; }
    }
}

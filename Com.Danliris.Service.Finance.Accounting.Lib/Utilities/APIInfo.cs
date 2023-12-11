using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Utilities
{
    public class APIInfo
    {
        public int count { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public int size { get; set; }
        public dynamic order { get; set; }
    }
}

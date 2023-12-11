using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VbNonPORequest
{
    public class CurrencyVBRequest
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
    }
}

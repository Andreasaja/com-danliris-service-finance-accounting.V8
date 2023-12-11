using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO
{
    public class CurrencyViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Rate { get; set; }
        public string Symbol { get; set; }
    }
}

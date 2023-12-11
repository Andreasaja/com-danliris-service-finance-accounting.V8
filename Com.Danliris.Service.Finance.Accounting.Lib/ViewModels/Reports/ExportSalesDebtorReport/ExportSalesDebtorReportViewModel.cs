using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Rreports.ExportSalesDebtorReportController
{
    public class ExportSalesDebtorReportViewModel
    {
        public string index { get; set; }
        public string buyerCode { get; set; }
        public string buyerName { get; set; }
        public string total { get; set; }
        public decimal beginingBalance { get; set; }
        public double sales { get; set; }
        public double receipt { get; set; }
        public double endBalance { get; set; }
        public double normal { get; set; }
        public double oneThirty { get; set; }
        public double thirtySixty { get; set; }
        public double sixtyNinety { get; set; }
        public double moreThanNinety { get; set; }
        public TimeSpan timeSpan { get; set; }

    }
}

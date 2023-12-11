using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel
{
    public class BankCashReceiptTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int COAId { get; set; }
        public string COACode { get; set; }
        public string COAName { get; set; }
    }
}

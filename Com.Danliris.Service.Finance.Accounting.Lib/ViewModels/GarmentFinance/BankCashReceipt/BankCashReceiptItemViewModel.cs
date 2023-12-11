using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceipt
{
    public class BankCashReceiptItemViewModel : BaseViewModel
    {
        public ChartOfAccountViewModel AccNumber { get; set; }
        public ChartOfAccountViewModel AccSub { get; set; }
        //public ChartOfAccountViewModel AccUnit { get; set; }
        //public ChartOfAccountViewModel AccAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal Summary { get; set; }
        //public decimal C2A { get; set; }
        //public decimal C2B { get; set; }
        //public decimal C2C { get; set; }
        //public decimal C1A { get; set; }
        //public decimal C1B { get; set; }
        public string NoteNumber { get; set; }
        public string Remarks { get; set; }
    }
}

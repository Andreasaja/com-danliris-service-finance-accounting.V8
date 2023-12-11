using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.MemorialDetail
{
    public class GarmentFinanceMemorialDetailItemViewModel
    {
        public virtual int MemorialDetailId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }

        public BuyerViewModel Buyer { get; set; }

        public CurrencyViewModel Currency { get; set; }

        public double Amount { get; set; }
    }
}

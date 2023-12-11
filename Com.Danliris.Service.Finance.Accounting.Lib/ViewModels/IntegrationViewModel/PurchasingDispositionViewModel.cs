using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel
{
    public class PurchasingDispositionViewModel : BaseViewModel
    {
        public string DispositionNo { get; set; }
        public SupplierViewModel Supplier { get; set; }
        public string Bank { get; set; }
        public string ConfirmationOrderNo { get; set; }
        public string InvoiceNo { get; set; }
        public string PaymentMethod { get; set; }
        public string Calculation { get; set; }
        public string Remark { get; set; }
        public string Investation { get; set; }
        public string ProformaNo { get; set; }
        public double Amount { get; set; }
        public CurrencyViewModel Currency { get; set; }
        public DateTimeOffset PaymentDueDate { get; set; }
        public int Position { get; set; }
        public virtual List<PurchasingDispositionItemViewModel> Items { get; set; }
    }
}

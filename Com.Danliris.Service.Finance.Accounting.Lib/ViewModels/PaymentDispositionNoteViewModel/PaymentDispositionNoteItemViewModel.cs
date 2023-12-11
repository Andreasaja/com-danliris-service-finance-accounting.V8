using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel
{
    public class PaymentDispositionNoteItemViewModel : BaseViewModel
    {
        public int purchasingDispositionExpeditionId { get; set; }
        public DivisionViewModel division { get; set; }
        public string proformaNo { get; set; }
        public double totalPaid { get; set; }
        public string dispositionId { get; set; }
        public DateTimeOffset dispositionDate { get; set; }
        public DateTimeOffset paymentDueDate { get; set; }
        public string dispositionNo { get; set; }
        public double dpp { get; set; }
        public double vatValue { get; set; }
        public double incomeTaxValue { get; set; }
        public CategoryViewModel category { get; set; }
        public double payToSupplier { get; set; }
        public double AmountPaid { get; set; }
        public double SupplierPayment { get; set; }
        public List<PaymentDispositionNoteDetailViewModel> Details { get; set; }
    }
}

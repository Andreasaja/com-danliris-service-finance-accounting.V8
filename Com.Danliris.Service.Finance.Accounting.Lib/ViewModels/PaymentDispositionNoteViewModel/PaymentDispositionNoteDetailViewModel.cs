using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel
{
    public class PaymentDispositionNoteDetailViewModel : BaseViewModel
    {
        public string epoId { get; set; }
        public double price { get; set; }
        public ProductViewModel product { get; set; }
        public double quantity { get; set; }
        public UnitViewModel unit { get; set; }
        public UomViewModel uom { get; set; }
        public int purchasingDispositionDetailId { get; set; }
        public int purchasingDispositionExpeditionItemId { get; set; }
    }
}

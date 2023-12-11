using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceiptDetailLocal
{
    public class GarmentFinanceBankCashReceiptDetailLocalItemViewModel : BaseViewModel
    {
        public int LocalSalesNoteId { get; set; }
        public string LocalSalesNoteNo { get; set; }
        public BuyerViewModel Buyer { get; set; }
        public CurrencyViewModel Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
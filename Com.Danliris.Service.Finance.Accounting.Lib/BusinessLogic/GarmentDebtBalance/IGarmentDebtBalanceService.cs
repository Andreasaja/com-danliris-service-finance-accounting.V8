using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance
{
    public interface IGarmentDebtBalanceService
    {
        List<GarmentDebtBalanceCardDto> GetDebtBalanceCardDto(int supplierId, int month, int year);
        int CreateFromCustoms(CustomsFormDto form);
        int UpdateFromInternalNote(int deliveryOrderId, InternalNoteFormDto form);
        int UpdateFromInvoice(int deliveryOrderId, InvoiceFormDto form);
        int UpdateFromBankExpenditureNote(int deliveryOrderId, BankExpenditureNoteFormDto form);
        int RemoveBalance(int deliveryOrderId);
        int EmptyInternalNoteValue(int deliveryOrderId);
        int EmptyInvoiceValue(int deliveryOrderId);
        int EmptyBankExpenditureNoteValue(int deliveryOrderId);
        GarmentDebtBalanceIndexDto GetDebtBalanceCardIndex(int supplierId, int month, int year);
        List<GarmentDebtBalanceSummaryDto> GetDebtBalanceSummary(int supplierId, int month, int year, bool isForeignCurrency, bool supplierIsImport);
        List<GarmentDebtBalanceDetailDto> GetDebtBalanceDetail(DateTimeOffset arrivalDate, GarmentDebtBalanceDetailFilterEnum supplierTypeFilter, int supplierId, int currencyId, string paymentType);
        GarmentDebtBalanceIndexDto GetDebtBalanceCardWithBalanceBeforeIndex(int supplierId, int month, int year);
        GarmentDebtBalanceSummaryAndTotalCurrencyDto GetDebtBalanceSummaryAndTotalCurrency(int supplierId, int month, int year, bool isForeignCurrency, bool supplierIsImport);
        List<GarmentDebtBalanceCardDto> GetDebtBalanceCardWithBeforeBalanceAndSaldoAkhirDto(int supplierId, int month, int year);
        GarmentDebtBalanceIndexDto GetDebtBalanceCardWithBalanceBeforeAndRemainBalanceIndex(int supplierId, int month, int year);
        GarmentDebtBalanceIndexDto GetDebtBalanceCardWithBalanceBeforeAndRemainBalanceIndex(string searchingType, int page = 1, int size = 25, string order = "{}", List<string> select = null, string keyword = null, string filter = "{}");
        int UpdateFromMemo(int deliveryOrderId, int memoDetailId, string memoNo, double memoAmount, double paymentRate);
    }
}

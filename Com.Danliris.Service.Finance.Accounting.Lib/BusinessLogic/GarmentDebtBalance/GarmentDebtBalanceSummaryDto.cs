using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance
{
    public class GarmentDebtBalanceSummaryDto
    {
        public GarmentDebtBalanceSummaryDto(int supplierId, string supplierCode, string supplierName, bool supplierIsImport, int currencyId, string currencyCode, double initialBalance, double purchaseAmount, double paymentAmount, double currentBalance, double currencyInitialBalance, double currencyPurchaseAmount, double currencyPaymentAmount, double currencyCurrentBalance)
        {
            SupplierId = supplierId;
            SupplierCode = supplierCode;
            SupplierName = supplierName;
            SupplierIsImport = supplierIsImport;
            CurrencyId = currencyId;
            CurrencyCode = currencyCode;
            InitialBalance = initialBalance;
            PurchaseAmount = purchaseAmount;
            PaymentAmount = paymentAmount;
            CurrentBalance = currentBalance;
            CurrencyInitialBalance = currencyInitialBalance;
            CurrencyPurchaseAmount = currencyPurchaseAmount;
            CurrencyPaymentAmount = currencyPaymentAmount;
            CurrencyCurrentBalance = currencyCurrentBalance;
        }

        public int SupplierId { get; private set; }
        public string SupplierCode { get; private set; }
        public string SupplierName { get; private set; }
        public bool SupplierIsImport { get; private set; }
        public int CurrencyId { get; private set; }
        public string CurrencyCode { get; private set; }
        public double InitialBalance { get; private set; }
        public double PurchaseAmount { get; private set; }
        public double PaymentAmount { get; private set; }
        public double CurrentBalance { get; private set; }
        public double CurrencyInitialBalance { get; private set; }
        public double CurrencyPurchaseAmount { get; private set; }
        public double CurrencyPaymentAmount { get; private set; }
        public double CurrencyCurrentBalance { get; private set; }

        public void SetTotal()
        {
            SupplierName = "TOTAL";
        }
    }
}
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePurchasingDisposition
{
    public class GarmentInvoicePurchasingDispositionModel: StandardEntity
    {
        /// <summary>
        /// as Expenditure No
        /// </summary>
        public string? InvoiceNo { get; set; }
        /// <summary>
        /// as Expenditure Date
        /// </summary>
        public DateTimeOffset InvoiceDate { get; set; }
        public int BankId { get;  set; }
        public string? BankName { get;  set; }
        public string? BankCode { get;  set; }
        public int BankCurrencyId { get; set; }
        public string? BankCurrencyCode { get; set; }
        public string? BankAccountNo { get;  set; }
        public string? BankAccountName { get;  set; }
        public string? BankSwiftCode { get; set; }
        public int CurrencyId { get;  set; }
        public string? CurrencyCode { get;  set; }
        public double CurrencyRate { get;  set; }
        public string? CurrencySymbol { get; set; }
        public DateTimeOffset CurrencyDate { get;  set; }
        public int SupplierId { get;  set; }
        public string? SupplierName { get;  set; }
        public string? SupplierCode { get;  set; }
        public bool IsImportSupplier { get;  set; }
        public string? ChequeNo { get;  set; }
        public string? PaymentType { get;  set; }
        public bool IsPosted { get; set; }
        public virtual List<GarmentInvoicePurchasingDispositionItemModel> Items { get; set; }
        public void SetIsPosted(string username, string userAgent)
        {
            IsPosted = true;
            this.FlagForUpdate(username, userAgent);
        }

    }
}

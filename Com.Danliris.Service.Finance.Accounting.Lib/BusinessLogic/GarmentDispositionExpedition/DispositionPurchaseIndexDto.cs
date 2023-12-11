using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionExpedition
{
    public class DispositionPurchaseIndexDto
    {
        public int Id { get; set; }
        public string DispositionNo { get; set; }
        public string Category { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public bool SupplierIsImport { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public DateTimeOffset CurrencyDate { get; set; }
        public string Bank { get; set; }
        public string ConfirmationOrderNo { get; set; }
        public string PaymentType { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public string Description { get; set; }
        public string InvoiceProformaNo { get; set; }
        public double Dpp { get; set; }
        public double IncomeTax { get; set; }
        public double VAT { get; set; }
        public double OtherCost { get; set; }
        public double Amount { get; set; }
        public DateTimeOffset VerifiedDateReceive { get; set; }
        public DateTimeOffset VerifiedDateSend { get; set; }
        public bool IsPaymentPaid { get; set; }
        public string CreatedBy { get; set; }
    }
}

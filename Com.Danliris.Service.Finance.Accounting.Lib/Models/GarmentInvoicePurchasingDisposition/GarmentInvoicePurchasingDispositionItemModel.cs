using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePurchasingDisposition
{
    public class GarmentInvoicePurchasingDispositionItemModel : StandardEntity
    {

        public GarmentInvoicePurchasingDispositionItemModel()
        {

        }

        public GarmentInvoicePurchasingDispositionItemModel(double totalPaid, int expeditionId, string dispositionNo)
        {
            TotalPaid = totalPaid;
            PurchasingDispositionExpeditionId = expeditionId;
            DispositionNo = dispositionNo;
        }


        public int DispositionId { get; private set; }
        public string DispositionNo { get; private set; }
        public DateTimeOffset DispositionDate { get; private set; }
        public DateTimeOffset DipositionDueDate { get; private set; }
        public string ProformaNo { get; private set; }
        public string SupplierName { get; private set; }
        public string Category { get; private set; }
        public double VATAmount { get; private set; }
        public double TotalAmount { get; private set; }
        public double TotalPaid { get; private set; }
        /// <summary>
        /// Sum of Total Paid With same Disposistion ID
        /// </summary>
        public double TotalPaidBefore { get; private set; }
        public int CurrencyId { get; private set; }
        public string CurrencyCode { get; private set; }
        public double CurrencyRate { get; private set; }
        public int GarmentInvoicePurchasingDispositionId { get; set; }
        public int PurchasingDispositionExpeditionId { get; private set; }
        [ForeignKey("GarmentInvoicePurchasingDispositionId")]
        public virtual GarmentInvoicePurchasingDispositionModel GarmentInvoicePurchasingDisposition { get; set; }
        public void SetTotalPaid(double totalPaid)
        {
            TotalPaid = totalPaid;
        }
    }
}

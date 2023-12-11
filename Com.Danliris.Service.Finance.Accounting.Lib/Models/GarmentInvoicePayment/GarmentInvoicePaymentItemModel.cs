using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePayment
{
    public class GarmentInvoicePaymentItemModel : StandardEntity
    {
        public virtual int InvoicePaymentId { get; set; }
        [ForeignKey("InvoicePaymentId")]
        public virtual GarmentInvoicePaymentModel GarmentInvoicePaymentModel { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public decimal IDRAmount { get; set; }
    }
}

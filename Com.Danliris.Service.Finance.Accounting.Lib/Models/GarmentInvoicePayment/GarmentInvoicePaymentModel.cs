using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePayment
{
    public class GarmentInvoicePaymentModel : StandardEntity
    {
        [MaxLength(50)]
        public string? InvoicePaymentNo { get; set; }
        public DateTimeOffset PaymentDate { get; set; }

        [MaxLength(255)]
        public string? BuyerCode { get; set; }
        public int BuyerId { get; set; }
        [MaxLength(1000)]
        public string? BuyerName { get; set; }

        [MaxLength(100)]
        public string? BGNo { get; set; }

        [MaxLength(255)]
        public string? CurrencyCode { get; set; }
        public int CurrencyId { get; set; }
        public double CurrencyRate { get; set; }

        [MaxLength(4000)]
        public string? Remark { get; set; }

        public virtual ICollection<GarmentInvoicePaymentItemModel> Items { get; set; }

    }
}

using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetail
{
    public class GarmentFinanceMemorialDetailItemModel : StandardEntity
    {
        public virtual int MemorialDetailId { get; set; }
        [ForeignKey("MemorialDetailId")]
        public virtual GarmentFinanceMemorialDetailModel GarmentFinanceMemorialDetailModel { get; set; }

        public int InvoiceId { get; set; }
        [MaxLength(20)]
        public string InvoiceNo { get; set; }

        public int BuyerId { get; set; }
        [MaxLength(225)]
        public string BuyerName { get; set; }
        [MaxLength(20)]
        public string BuyerCode { get; set; }

        public int CurrencyId { get; set; }
        [MaxLength(20)]
        public string CurrencyCode { get; set; }
        public double CurrencyRate { get; set; }

        public double Amount { get; set; }
    }
}

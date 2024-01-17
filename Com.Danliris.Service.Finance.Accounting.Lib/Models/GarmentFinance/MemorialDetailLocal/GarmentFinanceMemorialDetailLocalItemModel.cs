using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetailLocal
{
    public class GarmentFinanceMemorialDetailLocalItemModel : StandardEntity
    {
        public virtual int MemorialDetailLocalId { get; set; }
        [ForeignKey("MemorialDetailLocalId")]
        public virtual GarmentFinanceMemorialDetailLocalModel GarmentFinanceMemorialDetailLocalModel { get; set; }

        public int LocalSalesNoteId { get; set; }
        [MaxLength(20)]
        public string? LocalSalesNoteNo { get; set; }

        public int BuyerId { get; set; }
        [MaxLength(225)]
        public string? BuyerName { get; set; }
        [MaxLength(20)]
        public string? BuyerCode { get; set; }

        public int CurrencyId { get; set; }
        [MaxLength(20)]
        public string? CurrencyCode { get; set; }
        public double CurrencyRate { get; set; }

        public decimal Amount { get; set; }
    }
}

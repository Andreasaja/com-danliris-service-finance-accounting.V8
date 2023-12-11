using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetailLocal
{
    public class GarmentFinanceMemorialDetailLocalOtherItemModel : StandardEntity
    {
        public int ChartOfAccountId { get; set; }
        [MaxLength(32)]
        public string ChartOfAccountCode { get; set; }
        [MaxLength(255)]
        public string ChartOfAccountName { get; set; }

        public int CurrencyId { get; set; }
        [MaxLength(32)]
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }

        public decimal Amount { get; set; }
        [MaxLength(32)]
        public string TypeAmount { get; set; }
        [MaxLength(1000)]
        public string Remarks { get; set; }

        public virtual int MemorialDetailLocalId { get; set; }
        [ForeignKey("MemorialDetailLocalId")]
        public virtual GarmentFinanceMemorialDetailLocalModel GarmentFinanceMemorialDetailLocalModel { get; set; }
    }
}

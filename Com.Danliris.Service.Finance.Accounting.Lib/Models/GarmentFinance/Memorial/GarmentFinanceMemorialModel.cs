using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Memorial
{
    public class GarmentFinanceMemorialModel : StandardEntity
    {
        [MaxLength(20)]
        public string? MemorialNo { get; set; }
        public DateTimeOffset Date { get; set; }

        public int AccountingBookId { get; set; }
        [MaxLength(10)]
        public string? AccountingBookCode { get; set; }
        [StringLength(255)]
        public string? AccountingBookType { get; set; }

        public int GarmentCurrencyId { get; set; }
        [MaxLength(20)]
        public string? GarmentCurrencyCode { get; set; }
        public double GarmentCurrencyRate { get; set; }

        [MaxLength(4000)]
        public string? Remark { get; set; }
        public bool IsUsed { get; set; }

        public virtual ICollection<GarmentFinanceMemorialItemModel> Items { get; set; }
    }
}

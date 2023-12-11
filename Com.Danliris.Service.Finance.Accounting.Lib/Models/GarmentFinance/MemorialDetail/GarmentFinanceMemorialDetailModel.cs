using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetail
{
    public class GarmentFinanceMemorialDetailModel : StandardEntity
    {
        [MaxLength(20)]
        public string MemorialNo { get; set; }
        public int MemorialId { get; set; }
        public DateTimeOffset MemorialDate { get; set; }
        public decimal Amount { get; set; }

        public int DebitCoaId { get; set; }
        [MaxLength(32)]
        public string DebitCoaCode { get; set; }
        [MaxLength(256)]
        public string DebitCoaName { get; set; }

        public int InvoiceCoaId { get; set; }
        [MaxLength(32)]
        public string InvoiceCoaCode { get; set; }
        [MaxLength(256)]
        public string InvoiceCoaName { get; set; }

        public virtual ICollection<GarmentFinanceMemorialDetailItemModel> Items { get; set; }
        public virtual ICollection<GarmentFinanceMemorialDetailOtherItemModel> OtherItems { get; set; }
        public virtual ICollection<GarmentFinanceMemorialDetailRupiahItemModel> RupiahItems { get; set; }

    }
}

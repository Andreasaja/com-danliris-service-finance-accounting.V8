using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetail
{
    public class GarmentFinanceMemorialDetailRupiahItemModel : StandardEntity
    {
        public int ChartOfAccountId { get; set; }
        [MaxLength(32)]
        public string ChartOfAccountCode { get; set; }
        [MaxLength(255)]
        public string ChartOfAccountName { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public virtual int MemorialDetailId { get; set; }
        [ForeignKey("MemorialDetailId")]
        public virtual GarmentFinanceMemorialDetailModel GarmentFinanceMemorialDetailModel { get; set; }
    }
}

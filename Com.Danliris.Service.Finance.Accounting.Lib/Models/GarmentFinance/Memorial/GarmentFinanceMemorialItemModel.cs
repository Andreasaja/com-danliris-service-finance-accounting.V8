using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Memorial
{
    public class GarmentFinanceMemorialItemModel : StandardEntity
    {
        public virtual int MemorialId { get; set; }
        [ForeignKey("MemorialId")]
        public virtual GarmentFinanceMemorialModel GarmentFinanceMemorialModel { get; set; }
        public int COAId { get; set; }
        [MaxLength(20)]
        public string COACode { get; set; }
        [MaxLength(100)]
        public string COAName { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
    }
}

using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Adjustment
{
    public class GarmentFinanceAdjustmentModel : StandardEntity
    {
        [MaxLength(20)]
        public string AdjustmentNo { get; set; }
        public DateTimeOffset Date { get; set; }

        public int GarmentCurrencyId { get; set; }
        [MaxLength(20)]
        public string GarmentCurrencyCode { get; set; }
        public double GarmentCurrencyRate { get; set; }

        public double Amount { get; set; }

        [MaxLength(4000)]
        public string Remark { get; set; }
        public bool IsUsed { get; set; }

        public virtual ICollection<GarmentFinanceAdjustmentItemModel> Items { get; set; }
    }
}

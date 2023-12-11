using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoGarmentPurchasing
{
    public class MemoGarmentPurchasingReportViewModel
    {
        public int Id { get; set; }
        public COAViewModel COA { get; set; }
        public int DebitNominal { get; set; }
        public int CreditNominal { get; set; }
        public virtual MemoGarmentPurchasingViewModel MemoGarmentPurchasing { get; set; }
    }
}

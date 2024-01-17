using Com.Moonlay.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing
{
    public class MemoGarmentPurchasingDetailModel : StandardEntity
    {
        public int MemoId { get; set; }
        public int COAId { get; set; }
        public string? COANo { get; set; }
        public string? COAName { get; set; }
        public double DebitNominal { get; set; }
        public double CreditNominal { get; set; }

        [ForeignKey("MemoId")]
        public virtual MemoGarmentPurchasingModel MemoGarmentPurchasing { get; set; }
    }
}

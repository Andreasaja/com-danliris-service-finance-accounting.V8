using Com.Moonlay.Models;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing
{
    public class MemoDetailGarmentPurchasingDispositionModel : StandardEntity
    {
        public int MemoDetailGarmentPurchasingId { get; set; }
        public int DispositionId { get; set; }
        [MaxLength(128)]
        public string DispositionNo { get; set; }
    }
}

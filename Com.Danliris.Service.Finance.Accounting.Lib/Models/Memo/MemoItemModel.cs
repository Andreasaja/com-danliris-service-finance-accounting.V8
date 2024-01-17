using Com.Moonlay.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.Memo
{
    public class MemoItemModel : StandardEntity
    {
        public int CurrencyId { get; set; }
        [MaxLength(64)]
        public string? CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal Interest { get; set; }
        public int MemoId { get; set; }
        [ForeignKey("MemoId")]
        public virtual MemoModel Memo { get; set; }
    }
}
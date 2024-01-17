using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.Memo
{
    public class MemoModel : StandardEntity
    {
        [MaxLength(64)]
        public string? DocumentNo { get; set; }
        public int SalesInvoiceId { get; set; }
        [MaxLength(64)]
        public string? SalesInvoiceNo { get; set; }
        [MaxLength(64)]
        public string? MemoType { get; set; }
        public DateTimeOffset Date { get; set; }
        public int BuyerId { get; set; }
        [MaxLength(512)]
        public string? BuyerName { get; set; }
        [MaxLength(64)]
        public string? BuyerCode { get; set; }
        public int UnitId { get; set; }
        [MaxLength(64)]
        public string? UnitName { get; set; }
        [MaxLength(64)]
        public string? UnitCode { get; set; }
        public string? Remark { get; set; }
        public ICollection<MemoItemModel> Items { get; set; }
    }
}

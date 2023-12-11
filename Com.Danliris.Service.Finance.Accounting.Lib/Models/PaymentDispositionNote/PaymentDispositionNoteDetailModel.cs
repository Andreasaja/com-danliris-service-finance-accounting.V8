using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote
{
    public class PaymentDispositionNoteDetailModel : StandardEntity
    {
        public string EPOId { get; set; }
        public virtual int PaymentDispositionNoteItemId { get; set; }
        [ForeignKey("PaymentDispositionNoteItemId")]
        public virtual PaymentDispositionNoteItemModel PaymentDispositionNoteItem { get; set; }
       
        public double Price { get; set; }
        [MaxLength(255)]
        public string ProductCode { get; set; }
        public int ProductId { get; set; }
        [MaxLength(255)]
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        [MaxLength(255)]
        public string UnitCode { get; set; }
        public int UnitId { get; set; }
        [MaxLength(255)]
        public string UnitName { get; set; }
        public int PurchasingDispositionExpeditionItemId { get; set; }
        public int PurchasingDispositionDetailId { get; set; }
        public int UomId { get; set; }
        [MaxLength(255)]
        public string UomUnit { get; set; }
        public double PaidPrice { get; set; }

    }
}

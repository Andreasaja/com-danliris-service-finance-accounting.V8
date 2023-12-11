using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote
{
    public class PaymentDispositionNoteItemModel : StandardEntity
    {
        public virtual int PaymentDispositionNoteId { get; set; }
        [ForeignKey("PaymentDispositionNoteId")]
        public virtual PaymentDispositionNoteModel PaymentDispositionNote { get; set; }
        public int DivisionId { get; set; }
        [MaxLength(255)]
        public string DivisionCode { get; set; }
        [MaxLength(500)]
        public string DivisionName { get; set; }
        [MaxLength(255)]
        public string ProformaNo { get; set; }
        public double TotalPaid { get; set; }
        public DateTimeOffset DispositionDate { get; set; }
        public DateTimeOffset PaymentDueDate { get; set; }
        public int DispositionId { get; set; }
        public int PurchasingDispositionExpeditionId { get; set; }
        [MaxLength(255)]
        public string DispositionNo { get; set; }
        public double DPP { get; set; }
        public double VatValue { get; set; }
        public double IncomeTaxValue { get; set; }
        public int CategoryId { get; set; }
        [MaxLength(255)]
        public string CategoryCode { get; set; }
        [MaxLength(1000)]
        public string CategoryName { get; set; }
        public double PayToSupplier { get; set; }
        public double AmountPaid { get; set; }
        public double SupplierPayment { get; set; }
        public virtual ICollection<PaymentDispositionNoteDetailModel> Details { get; set; }

    }
}

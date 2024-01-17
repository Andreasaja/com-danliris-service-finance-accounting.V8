using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.SalesReceipt
{
    public class SalesReceiptDetailModel : StandardEntity, IValidatableObject
    {
        #region SalesInvoice
        public int SalesInvoiceId { get; set; }
        [MaxLength(255)]
        public string SalesInvoiceNo { get; set; }
        #endregion

        #region Currency
        public int CurrencyId { get; set; }
        [MaxLength(255)]
        public string CurrencyCode { get; set; }
        [MaxLength(255)]
        public string CurrencySymbol { get; set; }
        public double CurrencyRate { get; set; }
        #endregion

        public DateTimeOffset DueDate { get; set; }
        [MaxLength(255)]
        public string? VatType { get; set; }
        public double Tempo { get; set; }

        //TotalPayment => jumlah yang ditangguhkan
        public double TotalPayment { get; set; }

        //TotalPaid => jumlah yang sudah dibayar
        public double TotalPaid { get; set; }

        //Paid => jumlah yang dibayarakan (TotalPaid + Nominal)
        public double Paid { get; set; }

        //Nominal => jumlah yang akan dibayar
        public double Nominal { get; set; }

        //Unpaid => jumlah yang belum dibayar (hutang)
        public double Unpaid { get; set; }

        //OverPaid => kelebihan bayar melebihi TotalPayment (bonus)
        public double OverPaid { get; set; }

        //IsPaidOff => status lunas/tidak
        public bool IsPaidOff { get; set; }

        public int SalesReceiptId { get; set; }

        [ForeignKey("SalesReceiptId")]
        public virtual SalesReceiptModel SalesReceiptModel { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}

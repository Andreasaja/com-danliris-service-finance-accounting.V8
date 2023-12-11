using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote
{
    public class PaymentDispositionNoteModel : StandardEntity, IValidatableObject
    {
        [MaxLength(255)]
        public string BGCheckNumber { get; set; }
        [MaxLength(1000)]
        public string BankAccountName { get; set; }
        [MaxLength(255)]
        public string BankAccountNumber { get; set; }
        [MaxLength(255)]
        public string BankCode { get; set; }
        public int BankId { get; set; }
        [MaxLength(1000)]
        public string BankName { get; set; }
        [MaxLength(255)]
        public string BankCurrencyCode { get; set; }
        public int BankCurrencyId { get; set; }
        public double BankCurrencyRate { get; set; }

        [MaxLength(255)]
        public string CurrencyCode { get; set; }
        public int CurrencyId { get; set; }
        public double CurrencyRate { get; set; }

        [MaxLength(255)]
        public string PaymentDispositionNo { get; set; }
        public double Amount { get; set; }
        [MaxLength(255)]
        public string SupplierCode { get; set; }
        public int SupplierId { get; set; }
        public bool SupplierImport { get; set; }
        [MaxLength(1000)]
        public string SupplierName { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
        public string BankAccountCOA { get; set; }
        [MaxLength(64)]
        public string TransactionType { get; set; }
        public virtual ICollection<PaymentDispositionNoteItemModel> Items { get; set; }
        [DefaultValue(false)]
        public bool IsPosted { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        public void SetIsPosted(string username, string userAgent)
        {
            IsPosted = true;
            this.FlagForUpdate(username, userAgent);
        }

        public void FixFailAutoMapper(string bankCode)
        {
            BankCode = bankCode;
        }
    }
}

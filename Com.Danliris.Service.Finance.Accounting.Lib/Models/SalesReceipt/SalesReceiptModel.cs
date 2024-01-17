using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.SalesReceipt
{
    public class SalesReceiptModel : StandardEntity, IValidatableObject
    {
        [MaxLength(255)]
        public string? Code { get; set; }
        public long AutoIncreament { get; set; }
        [MaxLength(255)]
        public string? SalesReceiptNo { get; set; }
        public DateTimeOffset SalesReceiptDate { get; set; }

        #region Unit
        public int UnitId { get; set; }
        [MaxLength(255)]
        public string? UnitName { get; set; }
        #endregion

        #region Buyer
        public int BuyerId { get; set; }
        [MaxLength(255)]
        public string? BuyerName { get; set; }
        [MaxLength(1000)]
        public string? BuyerAddress { get; set; }
        #endregion

        [MaxLength(255)]
        public string? OriginAccountNumber { get; set; }

        #region Currency
        public int CurrencyId { get; set; }
        [MaxLength(255)]
        public string? CurrencyCode { get; set; }
        [MaxLength(255)]
        public string? CurrencySymbol { get; set; }
        public double CurrencyRate { get; set; }
        #endregion

        #region Bank
        public int BankId { get; set; }
        [MaxLength(255)]
        public string? AccountName { get; set; }
        [MaxLength(255)]
        public string? AccountNumber { get; set; }
        [MaxLength(255)]
        public string? BankName { get; set; }
        [MaxLength(255)]
        public string? BankCode { get; set; }
        #endregion

        public double AdministrationFee { get; set; }
        public double TotalPaid { get; set; }

        public virtual ICollection<SalesReceiptDetailModel> SalesReceiptDetails { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}

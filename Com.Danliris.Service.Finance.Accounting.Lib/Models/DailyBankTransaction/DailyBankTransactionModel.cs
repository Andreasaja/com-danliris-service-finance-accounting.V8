using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction
{
    public class DailyBankTransactionModel : StandardEntity, IValidatableObject
    {
        //Bank
        //[MaxLength(50)]
        public int AccountBankId { get; set; }
        [MaxLength(25)]
        public string AccountBankCode { get; set; }
        [MaxLength(100)]
        public string AccountBankName { get; set; }
        [MaxLength(100)]
        public string AccountBankAccountName { get; set; }
        [MaxLength(100)]
        public string AccountBankAccountNumber { get; set; }
        [MaxLength(100)]
        public string AccountBankCurrencyCode { get; set; }
        [MaxLength(50)]
        public int AccountBankCurrencyId { get; set; }
        [MaxLength(100)]
        public string AccountBankCurrencySymbol { get; set; }

        //Destination Bank
        //[MaxLength(50)]
        public int DestinationBankId { get; set; }
        [MaxLength(25)]
        public string DestinationBankCode { get; set; }
        [MaxLength(100)]
        public string DestinationBankName { get; set; }
        [MaxLength(100)]
        public string DestinationBankAccountName { get; set; }
        [MaxLength(100)]
        public string DestinationBankAccountNumber { get; set; }
        [MaxLength(100)]
        public string DestinationBankCurrencyCode { get; set; }
        [MaxLength(50)]
        public int DestinationBankCurrencyId { get; set; }
        [MaxLength(100)]
        public string DestinationBankCurrencySymbol { get; set; }

        [MaxLength(25)]
        public string Code { get; set; }
        //Buyer
        [MaxLength(25)]
        public string BuyerCode { get; set; }
        //[MaxLength(50)]
        public int BuyerId { get; set; }
        [MaxLength(150)]
        public string BuyerName { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal Nominal { get; set; }
        public decimal NominalValas { get; set; }
        public decimal TransactionNominal { get; set; }
        [MaxLength(50)]
        public string ReferenceNo { get; set; }
        [MaxLength(50)]
        public string ReferenceType { get; set; }
        [MaxLength(500)]
        public string Remark { get; set; }
        [MaxLength(50)]
        public string SourceType { get; set; }
        [MaxLength(50)]
        public string Status { get; set; }
        //Supplier
        //[MaxLength(50)]
        public int SupplierId { get; set; }
        [MaxLength(100)]
        public string SupplierCode { get; set; }
        [MaxLength(250)]
        public string SupplierName { get; set; }

        //Receiver from supplier
        public string Receiver { get; set; }

        public decimal AfterNominal { get; set; }
        public decimal AfterNominalValas { get; set; }
        public decimal BeforeNominal { get; set; }
        public decimal BeforeNominalValas { get; set; }
        public decimal CurrencyRate { get; set; }
        public bool IsPosted { get; set; }
        [MaxLength(128)]
        public string FinancingSourceReferenceNo { get; set; }
        public int FinancingSourceReferenceId { get; set; }
        // public decimal NominalOut { get; set; }
        public decimal Rates { get; set; }
        public decimal BankCharges { get; set; }
        [MaxLength(50)]
        public string SourceFundingType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}

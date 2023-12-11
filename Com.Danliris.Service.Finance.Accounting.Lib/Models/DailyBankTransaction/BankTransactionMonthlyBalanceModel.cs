using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction
{
    public class BankTransactionMonthlyBalanceModel : StandardEntity, IValidatableObject
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public double InitialBalance { get; set; }
        public double InitialBalanceValas { get; set; }
        public double RemainingBalance { get; set; }
        public double RemainingBalanceValas { get; set; }
        [MaxLength(50)]
        public int AccountBankId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}

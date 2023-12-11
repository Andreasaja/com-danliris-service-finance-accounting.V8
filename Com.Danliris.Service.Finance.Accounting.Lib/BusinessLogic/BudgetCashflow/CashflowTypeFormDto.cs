using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class CashflowTypeFormDto : IValidatableObject
    {
        public string Name { get; set; }
        public int LayoutOrder { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Nama tidak boleh kosong", new List<string> { "Name" });
            }
        }
    }
}
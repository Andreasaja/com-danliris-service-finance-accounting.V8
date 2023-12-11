using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class CashflowCategoryFormDto : IValidatableObject
    {
        public string Name { get; set; }
        public CashType Type { get; set; }
        public int CashflowTypeId { get; set; }
        public int LayoutOrder { get; set; }
        public bool IsLabelOnly { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Nama tidak boleh kosong", new List<string> { "Name" });
            }

            if (Type <= 0)
            {
                yield return new ValidationResult("Cash In/Cash Out harus dipilih", new List<string> { "Type" });
            }

            if (CashflowTypeId <= 0)
            {
                yield return new ValidationResult("Jenis Cashflow harus diisi", new List<string> { "CashflowTypeId" });
            }
        }
    }
}
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MasterCOA
{
    public class COAViewModel : BaseViewModel, IValidatableObject
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string ReportType { get; set; }

        public string Nature { get; set; }

        public string CashAccount { get; set; }

        public decimal Balance { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Code))
                yield return new ValidationResult("Code harus diisi", new List<string> { "Code" });

            if (string.IsNullOrWhiteSpace(Name))
                yield return new ValidationResult("Name harus diisi", new List<string> { "Name" });
        }
    }
}

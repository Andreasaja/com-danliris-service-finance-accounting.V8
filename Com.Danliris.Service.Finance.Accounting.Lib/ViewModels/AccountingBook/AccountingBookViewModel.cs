using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.AccountingBook
{
    public class AccountingBookViewModel : BaseViewModel, IValidatableObject
    {

        public string Code { get; set; }
        public string Remarks { get; set; }
        public string AccountingBookType { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(AccountingBookType))
                yield return new ValidationResult("Name tidak boleh kosong", new List<string> { "AccountingBookType" });

            if (string.IsNullOrWhiteSpace(Code))
                yield return new ValidationResult("Kode tidak boleh kosong", new List<string> { "Code" });

            if (!string.IsNullOrWhiteSpace(Code))
            {
                if (Code.Length > 10)
                    yield return new ValidationResult("Kode tidak boleh lebih dari 10 karakter", new List<string> { "Code" });
            }
        }


    }
}

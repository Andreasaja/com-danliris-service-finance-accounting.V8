using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CashierApproval
{
    public class CashierApprovalViewModel : IValidatableObject
    {

        public string VBRequestCategory { get; set; }

        public List<CashierApprovalItemViewModel> CashierApproval { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.CashierApproval.Count.Equals(0))
            {
                yield return new ValidationResult("CashierApproval is required", new List<string> { "CashierApproval" });
            }
        }
    }
}

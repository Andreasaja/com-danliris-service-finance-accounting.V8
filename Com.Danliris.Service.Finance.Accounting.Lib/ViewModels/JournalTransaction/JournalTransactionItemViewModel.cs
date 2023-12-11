using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MasterCOA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction
{
    public class JournalTransactionItemViewModel : BaseViewModel, IValidatableObject
    {
        public COAViewModel COA { get; set; }
        public string Remark { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Com.Moonlay.NetCore.Lib.Service;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Utilities
{
    public class ServiceValidationException : ServiceValidationExeption
    {
        public ServiceValidationException(ValidationContext validationContext, IEnumerable<ValidationResult> validationResults) : base("Validation Error", validationContext, validationResults)
        {

        }
    }
}

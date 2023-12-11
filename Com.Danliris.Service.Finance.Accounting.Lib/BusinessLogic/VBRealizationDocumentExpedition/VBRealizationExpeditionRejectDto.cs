using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition
{
    public class VBRealizationExpeditionRejectDto : IValidatableObject
    {
        public string Reason { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Reason))
                yield return new ValidationResult("Alasan harus diisi", new List<string> { "Reason" });
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition
{
    public class VBRealizationIdListDto : IValidatableObject
    {
        public List<int> VBRealizationIds { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (VBRealizationIds == null || VBRealizationIds.Count <= 0)
                yield return new ValidationResult("Harap pilih VB Realisasi", new List<string> { "VBRealizations" });
        }
    }
}

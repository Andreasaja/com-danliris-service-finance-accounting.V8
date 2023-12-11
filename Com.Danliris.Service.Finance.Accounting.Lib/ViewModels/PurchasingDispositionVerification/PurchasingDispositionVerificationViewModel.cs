using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionVerification
{
    public class PurchasingDispositionVerificationViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public DateTimeOffset? VerifyDate { get; set; }
        public string DispositionNo { get; set; }
        public string Reason { get; set; }
        public ExpeditionPosition SubmitPosition { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.VerifyDate == null)
            {
                yield return new ValidationResult("Date is required", new List<string> { "VerifyDate" });
            }
            else if (this.VerifyDate > DateTimeOffset.UtcNow)
            {
                yield return new ValidationResult("Date must be lower or equal than today's date", new List<string> { "VerifyDate" });
            }

            if (string.IsNullOrWhiteSpace(this.DispositionNo))
                yield return new ValidationResult("Disposition No is required", new List<string> { "DispositionNo" });

            if (!Enum.IsDefined(typeof(ExpeditionPosition), SubmitPosition))
                yield return new ValidationResult("Submit Position is required", new List<string> { "SubmitPosition" });
        }
    }
}

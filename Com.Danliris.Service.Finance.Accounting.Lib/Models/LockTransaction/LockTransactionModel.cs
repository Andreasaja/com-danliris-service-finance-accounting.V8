using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.LockTransaction
{
    public class LockTransactionModel : StandardEntity, IValidatableObject
    {
        //public DateTimeOffset EndLockDate { get; set; }
        //public DateTimeOffset BeginLockDate { get; set; }
        public DateTimeOffset LockDate { get; set; }
        public string Description { get; set; }
        public bool IsActiveStatus { get; set; }
        public string Type { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}

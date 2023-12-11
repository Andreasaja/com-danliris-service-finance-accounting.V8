using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction
{
    public class JournalTransactionModel : StandardEntity, IValidatableObject
    {
        [MaxLength(50)]
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        [MaxLength(250)]
        public string ReferenceNo { get; set; }
        public bool IsReverser { get; set; }
        public bool IsReversed { get; set; }
        [MaxLength(25)]
        public string Status { get; set; }
        public string Remark { get; set; }
        public ICollection<JournalTransactionItemModel> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}

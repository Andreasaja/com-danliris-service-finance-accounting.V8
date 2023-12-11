using Com.Danliris.Service.Finance.Accounting.Lib.Models.MasterCOA;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction
{
    public class JournalTransactionItemModel : StandardEntity, IValidatableObject
    {
        public int COAId { get; set; }
        public virtual COAModel COA { get; set; }
        public string Remark { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credit { get; set; }
        public int JournalTransactionId { get; set; }
        [ForeignKey("JournalTransactionId")]
        public virtual JournalTransactionModel JournalTransaction { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}

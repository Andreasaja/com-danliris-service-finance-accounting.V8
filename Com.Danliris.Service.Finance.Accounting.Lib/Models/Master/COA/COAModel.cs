using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.MasterCOA
{
    public class COAModel : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }

        public string Code1 { get; set; }

        public string Code2 { get; set; }

        public string Code3 { get; set; }

        public string Code4 { get; set; }

        public string Header { get; set; }

        public string Subheader { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string ReportType { get; set; }

        public string Nature { get; set; }

        public string CashAccount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}

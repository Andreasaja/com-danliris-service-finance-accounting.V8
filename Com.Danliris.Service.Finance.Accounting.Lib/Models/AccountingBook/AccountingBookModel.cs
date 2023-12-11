using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.AccountingBook
{
    public class AccountingBookModel : StandardEntity
    {
        [StringLength(10)]
        [Required]
        public string Code { get; set; }
        [StringLength(255)]
        public string Remarks { get; set; }
        [StringLength(255)]
        [Required]
        public string AccountingBookType { get; set; }
    }
}

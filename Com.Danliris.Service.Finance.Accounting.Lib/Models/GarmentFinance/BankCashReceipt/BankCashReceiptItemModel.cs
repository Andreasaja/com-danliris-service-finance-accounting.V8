using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceipt
{
    public class BankCashReceiptItemModel : StandardEntity
    {
        public virtual int BankCashReceiptId { get; set; }
        [ForeignKey("BankCashReceiptId")]
        public virtual BankCashReceiptModel BankCashReceiptModel { get; set; }

        public int AccNumberCoaId { get; set; }
        [MaxLength(32)]
        public string AccNumberCoaCode { get; set; }
        [MaxLength(256)]
        public string AccNumberCoaName { get; set; }

        public int AccSubCoaId { get; set; }
        [MaxLength(32)]
        public string AccSubCoaCode { get; set; }
        [MaxLength(256)]
        public string AccSubCoaName { get; set; }

        //public int AccUnitCoaId { get; set; }
        //[MaxLength(32)]
        //public string AccUnitCoaCode { get; set; }
        //[MaxLength(256)]
        //public string AccUnitCoaName { get; set; }

        //public int AccAmountCoaId { get; set; }
        //[MaxLength(32)]
        //public string AccAmountCoaCode { get; set; }
        //[MaxLength(256)]
        //public string AccAmountCoaName { get; set; }

        public decimal Amount { get; set; }

        public decimal Summary { get; set; }

        //public decimal C2A { get; set; }
        //public decimal C2B { get; set; }
        //public decimal C2C { get; set; }
        //public decimal C1A { get; set; }
        //public decimal C1B { get; set; }

        [MaxLength(256)]
        public string NoteNumber { get; set; }
        [MaxLength(1024)]
        public string Remarks { get; set; }

    }
}

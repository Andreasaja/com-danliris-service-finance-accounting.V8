using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditorAccount
{
    public class CreditorAccountUnitReceiptNotePostedViewModel : CreditorAccountPostedViewModel
    {
        public decimal DPP { get; set; }

        public decimal PPN { get; set; }

        public string Currency { get; set; }

        public string Products { get; set; }

        public bool UseIncomeTax { get; set; }

        public int DivisionId { get; set; }

        public string DivisionCode { get; set; }

        public string DivisionName { get; set; }

        public int UnitId { get; set; }

        public string UnitCode { get; set; }

        public string UnitName { get; set; }
        public string ExternalPurchaseOrderNo { get; set; }
        public decimal VATAmount { get; set; }
        public decimal IncomeTaxAmount { get; set; }
        public string IncomeTaxNo { get; set; }
    }
}

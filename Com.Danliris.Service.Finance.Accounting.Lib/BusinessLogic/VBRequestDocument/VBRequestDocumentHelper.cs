using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public enum VBType
    {
        [Display(Name = "Dengan PO")]
        WithPO = 1,
        [Display(Name = "Tanpa PO")]
        NonPO
    }

    public enum RealizationDocumentType
    {
        WithVB = 1,
        NonVB
    }

    public enum ApprovalStatus
    {
        Draft = 1,
        Approved,
        Cancelled
    }
}

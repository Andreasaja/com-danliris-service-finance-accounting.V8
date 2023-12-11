using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditorAccount
{
    public class CreditorAccountMemoPostedViewModel : CreditorAccountPostedViewModel
    {
        public decimal DPP { get; set; }

        public decimal PPN { get; set; }
    }
}

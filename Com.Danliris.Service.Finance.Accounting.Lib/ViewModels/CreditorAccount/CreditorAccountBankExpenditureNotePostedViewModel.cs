using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditorAccount
{
    public class CreditorAccountBankExpenditureNotePostedViewModel : CreditorAccountPostedViewModel
    {
        public int Id { get; set; }

        public decimal Mutation { get; set; }
    }
}

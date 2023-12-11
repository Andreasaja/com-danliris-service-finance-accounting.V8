using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditorAccount
{
    public class CreditorAccountUnitPaymentOrderPostedViewModel
    {
        //from SPBNo / NI No
        public string MemoNo { get; set; }
        public string InvoiceNo { get; set; }

        public List<CreditorAccountPostedViewModel> CreditorAccounts { get; set; }
        public DateTimeOffset? MemoDate { get; set; }
        public string PaymentDuration { get; set; }
    }
}

using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public class VBAutoJournalFormDto
    {
        public DateTimeOffset? Date { get; set; }
        public string DocumentNo { get; set; }
        public List<long> EPOIds { get; set; }
        public List<UPOandAmountDto> UPOIds { get; set; }
        public AccountBankViewModel Bank { get; set; }
    }
}
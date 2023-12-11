using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition
{
    public class ClearanceFormDto
    {
        public List<ClearancePostId> ListIds { get; set; }
        public AccountBankViewModel Bank { get; set; }
    }
}
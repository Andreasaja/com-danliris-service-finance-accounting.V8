using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO
{
    public class DetailSPB
    {
        public bool IsSave { get; set; }
        public DateTimeOffset? date { get; set; }
        public string division { get; set; }
        public string no { get; set; }
        public SupplierViewModel supplier { get; set; }
        public CurrencyViewModel currency { get; set; }
        public ICollection<DetailItemSPB> item { get; set; }
    }
}
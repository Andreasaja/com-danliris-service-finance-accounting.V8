using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO
{
    public class DetailItemSPB
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DetailunitReceiptNote unitReceiptNote { get; set; }
    }
}
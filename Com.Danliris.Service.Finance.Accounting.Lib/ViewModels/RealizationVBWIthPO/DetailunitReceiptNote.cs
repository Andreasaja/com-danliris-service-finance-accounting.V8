using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO
{
    public class DetailunitReceiptNote
    {
        public string no { get; set; }
        public int _id { get; set; }

        public ICollection<DetailitemunitReceiptNote> items { get; set; }
    }
}
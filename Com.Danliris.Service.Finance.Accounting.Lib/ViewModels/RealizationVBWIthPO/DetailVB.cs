using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO
{
    public class DetailVB
    {
        public int? Id { get; set; }
        public decimal Amount { get; set; }
        public string VBNo { get; set; }
        public DateTimeOffset? DateEstimate { get; set; }
        public string CreateBy { get; set; }
        public int UnitId { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public ICollection<PODetail> PONo { get; set; }
    }
}
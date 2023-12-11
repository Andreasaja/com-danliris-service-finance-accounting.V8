using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Memo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.Reports.LocalSalesDebtorReport
{
    public class GarmentShippingLocalSalesNote
    {
        public IList<ShippingLocalSalesNoteDto> data { get; set; }
        public GarmentShippingLocalSalesNote()
        {
            data = new List<ShippingLocalSalesNoteDto>();
        }


        public class ShippingLocalSalesNoteDto
        {
            public string Id { get; set; }
            public string noteNo { get; set; }
            public Buyer buyer { get; set; }
            public double amount { get; set; }
            public DateTimeOffset date { get; set; }
            public int tempo { get; set; }
        }
    }
}

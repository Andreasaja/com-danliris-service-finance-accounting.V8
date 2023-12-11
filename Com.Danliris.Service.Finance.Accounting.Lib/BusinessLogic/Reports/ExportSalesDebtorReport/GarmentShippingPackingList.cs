using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Reports.ExportSalesDebtorReport
{
    public class GarmentShippingPackingList
    {
        public IList<ShippingPackingDto> data { get; set; }
        public GarmentShippingPackingList()
        {
            data = new List<ShippingPackingDto>();
        }


        public class ShippingPackingDto
        {
            public string invoiceId { get; set; }
            public string invoiceNo { get; set; }
            public string buyerAgentCode { get; set; }
            public string buyerAgentName { get; set; }
            public double amount { get; set; }
            public double rate { get; set; }
            public double balanceAmount { get; set; }
            public double balanceAmountIDR { get; set; }
            public DateTimeOffset truckingDate { get; set; }
            public DateTimeOffset pebDate { get; set; }
            public int paymentdue { get; set; }
        }
      
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.LocalSalesOutstanding
{
    public class GarmentFinanceLocalSalesListModel
    {
        public IList<GarmentFinanceLocalSalesModel> data { get; set; }
        public GarmentFinanceLocalSalesListModel()
        {
            data = new List<GarmentFinanceLocalSalesModel>();
        }
        public class GarmentFinanceLocalSalesModel
        {
            public int id { get; set; }
            public string noteNo { get; set; }
            public DateTimeOffset? date { get; set; }
            public Buyer buyer { get; set; }
            public bool useVat { get; set; }

            public ICollection<GarmentShippingLocalSalesNoteItemViewModel> items { get; set; }
        }

        public class Buyer
        {
            public int id { get; set; }
            public string code { get; set; }
            public string name { get; set; }
            public string npwp { get; set; }
        }
        public class GarmentShippingLocalSalesNoteItemViewModel
        {
            public double quantity { get; set; }
            public double price { get; set; }
        }
    }
}

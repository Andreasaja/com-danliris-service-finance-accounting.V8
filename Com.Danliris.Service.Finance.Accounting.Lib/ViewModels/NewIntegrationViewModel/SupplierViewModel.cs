using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel
{
    public class SupplierViewModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Import { get; set; }
        public string PIC { get; set; }
        public string Contact { get; set; }
    }

    public class NewSupplierViewModel
    {
        public int _id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public bool import { get; set; }
    }
}

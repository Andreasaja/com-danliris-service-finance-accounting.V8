using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public class VbWithPORequestDetailItemsViewModel
    {
        public int Id { get; set; }
        public decimal Conversion { get; set; }
        public decimal dealQuantity { get; set; }
        public dealUom dealUom { get; set; }
        public decimal defaultQuantity { get; set; }
        public defaultUom defaultUom { get; set; }
        public decimal priceBeforeTax { get; set; }
        public Product_VB product { get; set; }
        public string productRemark { get; set; }
        public bool includePpn { get; set; }
        public bool useVat { get; set; }
    }    
}
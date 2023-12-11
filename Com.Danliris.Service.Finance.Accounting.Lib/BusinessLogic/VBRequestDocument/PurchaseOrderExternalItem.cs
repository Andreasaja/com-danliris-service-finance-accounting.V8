namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public class PurchaseOrderExternalItem
    {
        public int? Id { get; set; }
        public Product Product { get; set; }
        public double? DefaultQuantity { get; set; }
        public double? DealQuantity { get; set; }
        public UnitOfMeasurement DealUOM { get; set; }
        public double? Conversion { get; set; }
        public double? Price { get; set; }
        public bool UseVat { get; set; }
        public IncomeTaxDto IncomeTax { get; set; }
        public VatTaxDto VatTax{ get; set; }
        public string IncomeTaxBy { get; set; }
        public UnitDto Unit { get; set; }
        public bool UseIncomeTax { get; set; }
    }
}
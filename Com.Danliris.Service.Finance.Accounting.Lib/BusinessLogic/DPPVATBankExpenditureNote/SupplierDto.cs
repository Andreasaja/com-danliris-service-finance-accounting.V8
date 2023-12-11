namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote
{
    public class SupplierDto
    {
        public SupplierDto()
        {

        }

        public SupplierDto(int supplierId, string supplierName, bool isImportSupplier, string supplierCode)
        {
            Id = supplierId;
            Name = supplierName;
            IsImport = isImportSupplier;
            Code = supplierCode;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsImport { get; set; }
    }
}
namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance
{
    public class SupplierIdCurrencyIdDto
    {
        public SupplierIdCurrencyIdDto(int supplierId, int currencyId)
        {
            SupplierId = supplierId;
            CurrencyId = currencyId;
        }

        public int SupplierId { get; private set; }
        public int CurrencyId { get; private set; }
    }
}
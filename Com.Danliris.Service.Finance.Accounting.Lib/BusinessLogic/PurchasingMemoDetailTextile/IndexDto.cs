using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoDetailTextile
{
    public class IndexDto
    {
        public IndexDto(int id, DateTimeOffset lastModifiedUtc, DateTimeOffset date, string divisionName, string currencyCode, bool supplierIsImport, string remark, string documentNo)
        {
            Id = id;
            LastModifiedUtc = lastModifiedUtc;
            Date = date;
            DivisionName = divisionName;
            CurrencyCode = currencyCode;
            SupplierIsImport = supplierIsImport;
            Remark = remark;
            DocumentNo = documentNo;
        }

        public int Id { get; private set; }
        public DateTimeOffset LastModifiedUtc { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public string DivisionName { get; private set; }
        public string CurrencyCode { get; private set; }
        public bool SupplierIsImport { get; private set; }
        public string Remark { get; private set; }
        public string DocumentNo { get; private set; }
    }
}
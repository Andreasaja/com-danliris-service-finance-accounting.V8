using Com.Danliris.Service.Finance.Accounting.Lib.Enums;
using Com.Moonlay.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingMemoDetailTextile
{
    public class PurchasingMemoDetailTextileModel : StandardEntity
    {
        public PurchasingMemoDetailTextileModel()
        {

        }

        public PurchasingMemoDetailTextileModel(DateTimeOffset date, int divisionId, string divisionCode, string divisionName, int currencyId, string currencyCode, double currencyRate, bool supplierIsImport, string remark, PurchasingMemoType type, string documentNo)
        {
            Date = date;
            DivisionId = divisionId;
            DivisionCode = divisionCode;
            DivisionName = divisionName;
            CurrencyId = currencyId;
            CurrencyCode = currencyCode;
            CurrencyRate = currencyRate;
            SupplierIsImport = supplierIsImport;
            Remark = remark;
            Type = type;
            DocumentNo = documentNo;
        }

        public DateTimeOffset Date { get; private set; }
        public int DivisionId { get; private set; }
        [MaxLength(64)]
        public string? DivisionCode { get; private set; }
        [MaxLength(128)]
        public string? DivisionName { get; private set; }
        public int CurrencyId { get; private set; }
        //public int GarmentCurrencyId { get; private set; }
        [MaxLength(64)]
        public string? CurrencyCode { get; private set; }
        public double CurrencyRate { get; private set; }
        public bool SupplierIsImport { get; private set; }
        public string? Remark { get; private set; }
        public PurchasingMemoType Type { get; private set; }
        [MaxLength(32)]
        public string? DocumentNo { get; private set; }
    }
}

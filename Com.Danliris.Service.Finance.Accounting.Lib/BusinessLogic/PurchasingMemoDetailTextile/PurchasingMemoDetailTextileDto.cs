using Com.Danliris.Service.Finance.Accounting.Lib.Enums;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoDetailTextile
{
    public class PurchasingMemoDetailTextileDto
    {
        public PurchasingMemoDetailTextileDto(DateTimeOffset date, DivisionDto division, CurrencyDto currency, bool supplierIsImport, PurchasingMemoType type, List<FormItemDto> items, List<FormDetailDto> details, string remark, int id, string documentNo, bool memoIsCreated)
        {
            Date = date;
            Division = division;
            Currency = currency;
            SupplierIsImport = supplierIsImport;
            Type = type;
            Items = items;
            Details = details;
            Remark = remark;
            Id = id;
            DocumentNo = documentNo;
            MemoIsCreated = memoIsCreated;
        }

        public int Id { get; set; }
        public DateTimeOffset Date { get; private set; }
        public DivisionDto Division { get; private set; }
        public CurrencyDto Currency { get; private set; }
        public bool SupplierIsImport { get; private set; }
        public PurchasingMemoType Type { get; private set; }
        public List<FormItemDto> Items { get; private set; }
        public List<FormDetailDto> Details { get; private set; }
        public string Remark { get; private set; }
        public string DocumentNo { get; private set; }
        public bool MemoIsCreated { get; private set; }
    }
}
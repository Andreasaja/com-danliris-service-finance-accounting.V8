using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoDetailTextile;
using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoTextile
{
    public class MemoDetailDto
    {
        public MemoDetailDto(int id, string documentNo, DateTimeOffset date, CurrencyDto currency)
        {
            Id = id;
            DocumentNo = documentNo;
            Date = date;
            Currency = currency;
        }

        public int Id { get; set; }
        public string DocumentNo { get; set; }
        public DateTimeOffset Date { get; set; }
        public CurrencyDto Currency { get; set; }
    }
}
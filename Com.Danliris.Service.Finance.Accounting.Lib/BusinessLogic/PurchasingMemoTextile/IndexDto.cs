using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoTextile
{
    public class IndexDto
    {
        public IndexDto(int id, string memoDetailDocumentNo, DateTimeOffset memoDetailDate, string accountingBookType, string currencyCode, string remark, bool isPosted, string type)
        {
            Id = id;
            MemoDetailDocumentNo = memoDetailDocumentNo;
            MemoDetailDate = memoDetailDate;
            AccountingBookType = accountingBookType;
            CurrencyCode = currencyCode;
            Remark = remark;
            IsPosted = isPosted;
            Type = type;
        }

        public int Id { get; set; }
        public string MemoDetailDocumentNo { get; set; }
        public DateTimeOffset MemoDetailDate { get; set; }
        public string AccountingBookType { get; set; }
        public string CurrencyCode { get; set; }
        public string Remark { get; set; }
        public bool IsPosted { get; set; }
        public string Type { get; set; }
    }
}
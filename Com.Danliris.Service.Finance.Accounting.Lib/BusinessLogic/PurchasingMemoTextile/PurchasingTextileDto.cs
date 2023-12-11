using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoTextile
{
    public class PurchasingTextileDto
    {
        public PurchasingTextileDto(int id, AccountingBookDto accountingBook, MemoDetailDto memoDetail, string remark, List<FormItemDto> items, string createdBy)
        {
            Id = id;
            AccountingBook = accountingBook;
            MemoDetail = memoDetail;
            Remark = remark;
            Items = items;
            CreatedBy = createdBy;
        }

        public string CreatedBy { get; set; }
        public int Id { get; set; }
        public AccountingBookDto AccountingBook { get; set; }
        public MemoDetailDto MemoDetail { get; set; }
        public string Remark { get; set; }
        public List<FormItemDto> Items { get; set; }
    }
}
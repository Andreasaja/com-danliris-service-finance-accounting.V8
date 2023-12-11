namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoTextile
{
    public class AccountingBookDto
    {
        public AccountingBookDto(int id, string code, string accountingBookType)
        {
            Id = id;
            Code = code;
            AccountingBookType = accountingBookType;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string AccountingBookType { get; set; }
    }
}
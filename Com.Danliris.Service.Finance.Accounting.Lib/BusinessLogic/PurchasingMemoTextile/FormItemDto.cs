namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoTextile
{
    public class FormItemDto
    {
        public FormItemDto(ChartOfAccountDto chartOfAccount, double debitAmount, double creditAmount)
        {
            ChartOfAccount = chartOfAccount;
            DebitAmount = debitAmount;
            CreditAmount = creditAmount;
        }

        public ChartOfAccountDto ChartOfAccount { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
    }
}
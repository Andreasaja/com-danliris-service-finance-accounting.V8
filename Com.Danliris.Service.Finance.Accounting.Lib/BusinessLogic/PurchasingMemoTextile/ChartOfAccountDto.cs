namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoTextile
{
    public class ChartOfAccountDto
    {
        public ChartOfAccountDto(int id, string code, string name)
        {
            Id = id;
            Code = code;
            Name = name;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
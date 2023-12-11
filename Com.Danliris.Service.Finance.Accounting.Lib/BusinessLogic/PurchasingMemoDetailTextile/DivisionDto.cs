namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoDetailTextile
{
    public class DivisionDto
    {
        public DivisionDto(int id, string code, string name)
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
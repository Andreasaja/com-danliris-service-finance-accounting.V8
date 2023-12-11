namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote
{
    public class CategoryDto
    {
        public CategoryDto(int categoryId, string categoryName)
        {
            Id = categoryId;
            Name = categoryName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels
{
    public class OthersExpenditureProofDocumentCreateUpdateItemViewModel
    {
        public int? Id { get; set; }
        public string Remark { get; set; }
        public int? COAId { get; set; }
        public decimal? Debit { get; set; }
    }
}

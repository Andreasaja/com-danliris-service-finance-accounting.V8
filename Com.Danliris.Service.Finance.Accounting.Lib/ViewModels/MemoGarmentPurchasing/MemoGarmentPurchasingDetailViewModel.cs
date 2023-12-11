
namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoGarmentPurchasing
{
    public class MemoGarmentPurchasingDetailViewModel
    {
        public int? Id { get; set; }
        public COAViewModel COA { get; set; }
        public double DebitNominal { get; set; }
        public double CreditNominal { get; set; }
    }
}

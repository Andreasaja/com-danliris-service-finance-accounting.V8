using Com.Moonlay.Models;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction
{
    public class JournalTransactionNumber : StandardEntity
    {
        public int Division { get; set; }
        public int Number { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}

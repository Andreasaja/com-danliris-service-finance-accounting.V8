using System.ComponentModel.DataAnnotations.Schema;
using Com.Moonlay.Models;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.OthersExpenditureProofDocument
{
    public class OthersExpenditureProofDocumentItemModel : StandardEntity
    {
        public int COAId { get; set; }
        public decimal Debit { get; set; }
        public string Remark { get; set; }
        public int OthersExpenditureProofDocumentId { get; set; }
        [NotMapped]
        public bool IsUpdated { get; set; }

        public void UpdateCOAId(int newCOAId)
        {
            if (newCOAId != COAId)
            {
                COAId = newCOAId;
                IsUpdated = true;
            }
        }

        public void UpdateDebit(decimal newDebit)
        {
            if (newDebit != Debit)
            {
                Debit = newDebit;
                IsUpdated = true;
            }
        }

        public void UpdateRemark(string newRemark)
        {
            if (Remark != newRemark)
            {
                Remark = newRemark;
                IsUpdated = true;
            }
        }
    }
}

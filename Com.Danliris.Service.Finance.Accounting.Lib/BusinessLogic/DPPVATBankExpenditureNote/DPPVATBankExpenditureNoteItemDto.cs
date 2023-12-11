using Com.Danliris.Service.Finance.Accounting.Lib.Models.DPPVATBankExpenditureNote;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote
{
    public class DPPVATBankExpenditureNoteItemDto
    {
        public DPPVATBankExpenditureNoteItemDto(DPPVATBankExpenditureNoteItemModel item, List<DPPVATBankExpenditureNoteDetailModel> details)
        {
            InternalNote = new InternalNoteDto(item, details);
            OutstandingAmount = item.OutstandingAmount;
            Id = item.Id;
            Select = true;
        }

        public int Id { get; set; }
        public bool Select { get; set; }
        public InternalNoteDto InternalNote { get; set; }
        public double OutstandingAmount { get; set; }
    }
}
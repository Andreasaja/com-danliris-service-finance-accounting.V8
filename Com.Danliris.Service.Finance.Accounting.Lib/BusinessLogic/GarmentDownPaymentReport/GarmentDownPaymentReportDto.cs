using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDownPaymentReport
{
    public class GarmentDownPaymentReportDto
    {
        public GarmentDownPaymentReportDto(int dispositionId, string dispositionNo, DateTimeOffset dispositionDueDate, List<int> dispositionPaymentIds)
        {
            DispositionId = dispositionId;
            DispositionNo = dispositionNo;
            DispositionDueDate = dispositionDueDate;
            DispositionPayments = new List<DispositionPaymentDto>();
            MemoDocuments = new List<MemoDocumentDto>();
            DispositionPaymentIds = dispositionPaymentIds;
        }

        public int DispositionId { get; private set; }
        public string DispositionNo { get; private set; }
        public DateTimeOffset DispositionDueDate { get; private set; }
        public List<DispositionPaymentDto> DispositionPayments { get; private set; }
        public List<MemoDocumentDto> MemoDocuments { get; private set; }
        public List<int> DispositionPaymentIds { get; private set; }
        public List<int> MemoIds { get; private set; }
    }
}
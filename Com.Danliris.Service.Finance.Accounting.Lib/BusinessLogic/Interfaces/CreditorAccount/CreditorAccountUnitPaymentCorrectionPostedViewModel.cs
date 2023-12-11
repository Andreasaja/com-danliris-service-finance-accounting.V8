using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.CreditorAccount
{
    public class CreditorAccountUnitPaymentCorrectionPostedViewModel
    {
        public int UnitPaymentCorrectionId { get; set; }
        public string UnitPaymentCorrectionNo { get; set; }
        public string UnitReceiptNoteNo { get; set; }
        public decimal UnitPaymentCorrectionDPP { get; set; }
        public decimal UnitPaymentCorrectionPPN { get; set; }
        public decimal UnitPaymentCorrectionMutation { get; set; }
        public DateTimeOffset UnitPaymentCorrectionDate { get; set; }
    }
}
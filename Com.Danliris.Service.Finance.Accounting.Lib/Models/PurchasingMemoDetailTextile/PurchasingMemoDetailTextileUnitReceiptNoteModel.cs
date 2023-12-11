using Com.Moonlay.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingMemoDetailTextile
{
    public class PurchasingMemoDetailTextileUnitReceiptNoteModel : StandardEntity
    {
        public PurchasingMemoDetailTextileUnitReceiptNoteModel()
        {

        }

        public PurchasingMemoDetailTextileUnitReceiptNoteModel(int purchasingMemoDetailTextileId, int purchasingMemoDetailTextileItemId, int purchasingMemoDetailTextileDetailId)
        {
            PurchasingMemoDetailTextileId = purchasingMemoDetailTextileId;
            PurchasingMemoDetailTextileItemId = purchasingMemoDetailTextileItemId;
            PurchasingMemoDetailTextileDetailId = purchasingMemoDetailTextileDetailId;
        }

        public PurchasingMemoDetailTextileUnitReceiptNoteModel(int purchasingMemoDetailTextileId, int purchasingMemoDetailTextileItemId, int purchasingMemoDetailTextileDetailId, int unitReceiptNoteId, string unitReceiptNoteNo, DateTimeOffset unitReceiptNoteDate) : this(purchasingMemoDetailTextileId, purchasingMemoDetailTextileItemId, purchasingMemoDetailTextileDetailId)
        {
            UnitReceiptNoteId = unitReceiptNoteId;
            UnitReceiptNoteNo = unitReceiptNoteNo;
            UnitReceiptNoteDate = unitReceiptNoteDate;
        }

        public int PurchasingMemoDetailTextileId { get; private set; }
        public int PurchasingMemoDetailTextileItemId { get; private set; }
        public int PurchasingMemoDetailTextileDetailId { get; private set; }
        public int UnitReceiptNoteId { get; private set; }
        [MaxLength(64)]
        public string UnitReceiptNoteNo { get; private set; }
        public DateTimeOffset UnitReceiptNoteDate { get; private set; }
    }
}

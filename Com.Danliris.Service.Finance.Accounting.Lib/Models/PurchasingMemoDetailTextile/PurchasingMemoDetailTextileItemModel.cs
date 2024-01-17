using Com.Moonlay.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingMemoDetailTextile
{
    public class PurchasingMemoDetailTextileItemModel : StandardEntity
    {
        public PurchasingMemoDetailTextileItemModel()
        {

        }

        public PurchasingMemoDetailTextileItemModel(int purchasingMemoDetailTextileId)
        {
            PurchasingMemoDetailTextileId = purchasingMemoDetailTextileId;
        }

        public PurchasingMemoDetailTextileItemModel(int dispositionId, string dispositionNo, DateTimeOffset dispositionDate, int purchasingMemoDetailTextileId) : this(purchasingMemoDetailTextileId)
        {
            DispositionId = dispositionId;
            DispositionNo = dispositionNo;
            DispositionDate = dispositionDate;
        }

        public int DispositionId { get; private set; }
        [MaxLength(64)]
        public string? DispositionNo { get; private set; }
        public DateTimeOffset DispositionDate { get; private set; }
        public int PurchasingMemoDetailTextileId { get; private set; }
    }
}

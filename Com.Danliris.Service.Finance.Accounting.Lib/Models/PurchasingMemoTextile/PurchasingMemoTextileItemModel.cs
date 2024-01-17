using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingMemoTextile
{
    public class PurchasingMemoTextileItemModel : StandardEntity
    {
        public PurchasingMemoTextileItemModel()
        {

        }

        public PurchasingMemoTextileItemModel(int purchasingMemoTextileId)
        {
            PurchasingMemoTextileId = purchasingMemoTextileId;
        }

        public PurchasingMemoTextileItemModel(int chartOfAccountId, string chartOfAccountCode, string chartOfAccountName, double debitAmount, double creditAmount, int purchasingMemoTextileId) : this(purchasingMemoTextileId)
        {
            ChartOfAccountId = chartOfAccountId;
            ChartOfAccountCode = chartOfAccountCode;
            ChartOfAccountName = chartOfAccountName;
            DebitAmount = debitAmount;
            CreditAmount = creditAmount;
        }

        public int ChartOfAccountId { get; private set; }
        public string? ChartOfAccountCode { get; private set; }
        public string? ChartOfAccountName { get; private set; }
        public double DebitAmount { get; private set; }
        public double CreditAmount { get; private set; }
        public int PurchasingMemoTextileId { get; private set; }

        
    }
}

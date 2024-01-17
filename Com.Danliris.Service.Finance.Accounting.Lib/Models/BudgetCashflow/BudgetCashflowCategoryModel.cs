using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow;
using Com.Moonlay.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow
{
    public class BudgetCashflowCategoryModel : StandardEntity
    {
        public BudgetCashflowCategoryModel()
        {

        }

        public BudgetCashflowCategoryModel(string name, CashType type, int cashflowTypeId, int layoutOrder, bool isLabelOnly)
        {
            Name = name;
            Type = type;
            CashflowTypeId = cashflowTypeId;
            LayoutOrder = layoutOrder;
            IsLabelOnly = isLabelOnly;
        }

        [MaxLength(512)]
        public string? Name { get; private set; }
        public int CashflowTypeId { get; private set; }
        public int LayoutOrder { get; private set; }
        public CashType Type { get; private set; }
        public bool IsLabelOnly { get; private set; }

        public void SetNewValue(string name, CashType type, int layoutOrder, int cashflowTypeId)
        {
            Name = name;
            Type = type;
            LayoutOrder = layoutOrder;
            CashflowTypeId = cashflowTypeId;
        }
    }
}

using Com.Moonlay.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow
{
    public class BudgetCashflowTypeModel : StandardEntity
    {
        public BudgetCashflowTypeModel()
        {

        }

        public BudgetCashflowTypeModel(string name, int layoutOrder)
        {
            Name = name;
            LayoutOrder = layoutOrder;
        }

        [MaxLength(512)]
        public string Name { get; private set; }
        public int LayoutOrder { get; private set; }

        public void SetNewNameAndLayoutOrder(string name, int layoutOrder)
        {
            Name = name;
            LayoutOrder = layoutOrder;
        }
    }
}

using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class BudgetCashflowSubCategoryTypeDto
    {
        public BudgetCashflowSubCategoryTypeDto(BudgetCashflowSubCategoryModel model)
        {
            Id = model.Id;
            Name = model.Name;
            PurchasingCategoryIds = JsonConvert.DeserializeObject<List<int>>(model.PurchasingCategoryIds);
            IsReadOnly = model.IsReadOnly;
            LayoutOrder = model.LayoutOrder;
            ReportType = model.ReportType;
            ReportTypeName = model.ReportType.ToDescriptionString();
            CashflowCategoryId = model.CashflowCategoryId;
            IsImport = model.IsImport;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> PurchasingCategoryIds { get; set; }
        public BudgetCashflowCategoryModel CashflowCategory { get; private set; }
        public int LayoutOrder { get; set; }
        public bool IsReadOnly { get; set; }
        public ReportType ReportType { get; set; }
        public string ReportTypeName { get; private set; }
        public int CashflowCategoryId { get; set; }
        public bool IsImport { get; private set; }
    }
}
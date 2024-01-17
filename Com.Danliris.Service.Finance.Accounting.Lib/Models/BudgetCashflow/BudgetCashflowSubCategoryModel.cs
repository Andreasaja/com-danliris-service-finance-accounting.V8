using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow;
using Com.Moonlay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow
{
    public class BudgetCashflowSubCategoryModel : StandardEntity
    {
        public BudgetCashflowSubCategoryModel()
        {

        }

        public BudgetCashflowSubCategoryModel(string name, int cashflowCategoryId, int layoutOrder, List<int> purchasingCategoryIds, bool isReadOnly, ReportType reportType, bool isImport)
        {
            Name = name;
            CashflowCategoryId = cashflowCategoryId;
            LayoutOrder = layoutOrder;

            if (purchasingCategoryIds == null)
                purchasingCategoryIds = new List<int>();

            PurchasingCategoryIds = JsonConvert.SerializeObject(purchasingCategoryIds);
            IsReadOnly = isReadOnly;
            ReportType = reportType;
            IsImport = isImport;
        }

        [MaxLength(512)]
        public string? Name { get; private set; }
        public int CashflowCategoryId { get; private set; }
        public int LayoutOrder { get; private set; }
        public string? PurchasingCategoryIds { get; private set; }
        public bool IsReadOnly { get; private set; }
        public ReportType ReportType { get; private set; }
        public bool IsImport { get; private set; }

        public void SetNewValue(int cashflowCategoryId, bool isReadOnly, int layoutOrder, string name, List<int> purchasingCategoryIds, ReportType reportType, bool isImport)
        {
            Name = name;
            CashflowCategoryId = cashflowCategoryId;
            LayoutOrder = layoutOrder;

            if (purchasingCategoryIds == null)
                purchasingCategoryIds = new List<int>();

            PurchasingCategoryIds = JsonConvert.SerializeObject(purchasingCategoryIds);
            IsReadOnly = isReadOnly;
            ReportType = reportType;
            IsImport = isImport;
        }
    }
}

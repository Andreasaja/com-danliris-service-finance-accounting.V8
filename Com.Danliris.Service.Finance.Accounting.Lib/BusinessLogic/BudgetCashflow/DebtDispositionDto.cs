using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class DebtDispositionDto
    {
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public double CurrencyRate { get; set; }
        public string CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string UnitId { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string DivisionId { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public bool IsImport { get; set; }
        public bool IsPaid { get; set; }
        public double DebtPrice { get; set; }
        public double DebtQuantity { get; set; }
        public double DispositionPrice { get; set; }
        public double DispositionQuantity { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public double Total { get; set; }
        public double DispositionTotal { get; set; }
        public double DebtTotal { get; set; }
        public string IncomeTaxBy { get; set; }
        public bool UseIncomeTax { get; set; }
        public string IncomeTaxRate { get; set; }
        public bool UseVat { get; set; }
        public int CategoryLayoutIndex { get; set; }
        public string AccountingUnitName { get; set; }
        public string AccountingUnitId { get; set; }

        public int GetCurrencyId()
        {
            return int.Parse(CurrencyId);
        }

        public int GetCategoryId()
        {
            return int.Parse(CategoryId);
        }

        public int GetUnitId()
        {
            //int.TryParse(UnitId, out var unitId);
            int.TryParse(AccountingUnitId, out var unitId);

            return unitId;
        }

        public int GetDivisionId()
        {
            int.TryParse(DivisionId, out var divisionId);
            return divisionId;
        }
    }
}
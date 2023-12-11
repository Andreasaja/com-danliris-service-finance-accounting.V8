using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;
using System.Collections.Generic;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class BudgetCashflowUnitItemDto
    {
        public BudgetCashflowUnitItemDto(BudgetCashflowUnitModel entity, List<CurrencyDto> currencies)
        {
            Currency = currencies.FirstOrDefault(currency => currency.Id == entity.CurrencyId);
            Nominal = entity.Nominal;
            CurrencyNominal = entity.CurrencyNominal;
            Total = entity.Total;
            IsIDR = Currency.Code == "IDR";
        }

        public BudgetCashflowUnitItemDto(RealCashBalanceModel entity, List<CurrencyDto> currencies)
        {
            Currency = currencies.FirstOrDefault(currency => currency.Id == entity.CurrencyId);
            Nominal = entity.Nominal;
            CurrencyNominal = entity.CurrencyNominal;
            Total = entity.Total;
            IsIDR = Currency.Code == "IDR";
        }

        public BudgetCashflowUnitItemDto(InitialCashBalanceModel entity, List<CurrencyDto> currencies)
        {
            Currency = currencies.FirstOrDefault(currency => currency.Id == entity.CurrencyId);
            Nominal = entity.Nominal;
            CurrencyNominal = entity.CurrencyNominal;
            Total = entity.Total;
            IsIDR = Currency.Code == "IDR";
        }

        public CurrencyDto Currency { get; private set; }
        public double Nominal { get; private set; }
        public double CurrencyNominal { get; private set; }
        public double Total { get; private set; }
        public bool IsIDR { get; private set; }
    }
}
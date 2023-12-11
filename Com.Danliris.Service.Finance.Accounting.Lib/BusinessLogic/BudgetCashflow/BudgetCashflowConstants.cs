using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public enum CashType
    {
        [Description("Cash In")]
        In = 1,
        [Description("Cash Out")]
        Out
    }

    public enum ReportType
    {
        [Description("Laporan Hutang dan Disposisi")]
        DebtAndDispositionSummary = 1,
        [Description("Laporan Pembelian")]
        PurchasingReport
    }

    public static class CashTypeEnumExtensions
    {
        public static string ToDescriptionString(this CashType me)
        {
            var enumType = me.GetType();
            var memberInfo = enumType.GetMember(me.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var _attr = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (_attr != null && _attr.Count() > 0)
                {
                    return ((DescriptionAttribute)_attr.ElementAt(0)).Description;
                }
            }
            return me.ToString();
        }
    }

    public static class ReportTypeEnumExtensions
    {
        public static string ToDescriptionString(this ReportType me)
        {
            var enumType = me.GetType();
            var memberInfo = enumType.GetMember(me.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var _attr = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (_attr != null && _attr.Count() > 0)
                {
                    return ((DescriptionAttribute)_attr.ElementAt(0)).Description;
                }
            }
            return me.ToString();
        }
    }
}

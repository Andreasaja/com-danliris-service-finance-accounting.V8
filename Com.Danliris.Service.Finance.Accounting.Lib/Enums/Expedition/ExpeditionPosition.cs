using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition
{
    public enum SupplierType
    {
        All = 1,
        Local,
        Import
    }

    public enum ExpeditionPosition
    {
        [Description("Bag. Pembelian")]
        PURCHASING_DIVISION = 1,
        [Description("Dikirim ke Bag. Verifikasi")]
        SEND_TO_VERIFICATION_DIVISION,
        [Description("Bag. Verifikasi")]
        VERIFICATION_DIVISION,
        [Description("Dikirim ke Bag. Kasir")]
        SEND_TO_CASHIER_DIVISION,
        [Description("Dikirim ke Bag. Keuangan")]
        SEND_TO_ACCOUNTING_DIVISION,
        [Description("Dikirim ke Bag. Pembelian")]
        SEND_TO_PURCHASING_DIVISION,
        [Description("Bag. Kasir")]
        CASHIER_DIVISION,
        [Description("Bag. Keuangan")]
        FINANCE_DIVISION
    }

    public enum GarmentPurchasingExpeditionPosition
    {
        [Description("")]
        Invalid = 0,
        [Description("Pembelian")]
        Purchasing,
        [Description("Kirim ke Verifikasi")]
        SendToVerification,
        [Description("Verifikasi (Diterima)")]
        VerificationAccepted,
        [Description("Kirim ke Kasir")]
        SendToCashier,
        [Description("Kasir (Diterima)")]
        CashierAccepted,
        [Description("Kirim ke Pembelian (Not Verified)")]
        SendToPurchasing,
        [Description("Kirim ke Accounting")]
        SendToAccounting,
        [Description("Accounting (Diterima)")]
        AccountingAccepted,
        [Description("Pembayaran Disposisi")]
        DispositionPayment
    }

    // Display Friendly Name for enum
    // source : https://www.codingame.com/playgrounds/2487/c---how-to-display-friendly-names-for-enumerations
    public static class ExpeditionPositionExtensions
    {
        public static string ToDescriptionString(this ExpeditionPosition me)
        {
            Type enumType = me.GetType();
            MemberInfo[] memberInfo = enumType.GetMember(me.ToString());
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

    public static class PurchasingGarmentExpeditionPositionEnumExtensions
    {
        public static string ToDescriptionString(this GarmentPurchasingExpeditionPosition me)
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

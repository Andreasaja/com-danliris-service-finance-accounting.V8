using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Enums.Master
{
    public enum COACategoryEnum
    {
        [Description("AKTIVA LANCAR")]
        AKTIVA_LANCAR = 1,
        [Description("KAS DAN SETARA KAS")]
        KAS_DAN_SETARA_KAS = 10,
        [Description("PIUTANG USAHA")]
        PIUTANG_USAHA = 11,
        [Description("PIUTANG WESEL")]
        PIUTANG_WESEL = 12,
        [Description("PIUTANG LAIN-LAIN")]
        PIUTANG_LAIN_LAIN = 13,
        [Description("PERSEDIAAN")]
        PERSEDIAAN = 14,
        [Description("PEMBAYARAN DIMUKA")]
        PEMBAYARAN_DIMUKA = 15,
        [Description("R/K UNIT")]
        RK_UNIT = 16,
        [Description("AKUN SEMENTARA")]
        AKUN_SEMENTARA = 18,

        [Description("AKTIVA TIDAK LANCAR")]
        AKTIVA_TIDAK_LANCAR = 2,
        [Description("INVESTASI JANGKA PANJANG")]
        INVESTASI_JANGKA_PANJANG = 20,
        [Description("AKTIVA TETAP - PEMILIKAN SENDIRI")]
        AKTIVA_TETAP_PEMILIKAN_SENDIRI = 21,
        [Description("AKTIVA TETAP SEWA GUNA USAHA")]
        AKTIVA_TETAP_SEWA_GUNA_USAHA = 22,
        [Description("AKTIVA LAIN-LAIN")]
        AKTIVA_LAIN_LAIN = 23,

        [Description("PASIVA")]
        PASIVA = 3,
        [Description("HUTANG USAHA")]
        HUTANG_USAHA = 30,
        [Description("PINJAMAN JANGKA PENDEK")]
        PINJAMAN_JANGKA_PENDEK = 31,
        [Description("HUTANG LAIN-LAIN")]
        HUTANG_LAIN_LAIN = 32,
        [Description("PAJAK YMG DIBAYAR")]
        PAJAK_YMH_DIBAYAR = 33,
        [Description("BEBAN YMG DIBAYAR")]
        BEBAN_YMH_DIBAYAR = 34,
        [Description("PINJAMAN JANGKA PANJANG")]
        PINJAMAN_JANGKA_PANJANG = 35,
        [Description("SELISIH KURS DITANGGUHKAN")]
        SELISIH_KURS_DITANGGUHKAN = 36,

        [Description("EKUITAS")]
        EKUITAS = 4,
        [Description("MODAL SAHAM")]
        MODAL_SAHAM = 40,
        [Description("SELISIH PENILAIAN AT")]
        SELISIH_PENILAIAN_AT = 41,
        [Description("MODAL LAIN-LAIN")]
        MODAL_LAIN_LAIN = 42,
        [Description("SALDO LABA (RUGI)")]
        SALDO_LABA_RUGI = 43,
        [Description("KOREKSI TAMBAHAN ASET TA")]
        KOREKSI_TAMBAHAN_ASET_TA = 44,

        [Description("PENJUALAN & BEBAN POKOK PENJUALAN")]
        PENJUALAN_BEBAN_POKOK_PENJUALAN = 5,
        [Description("PENJUALAN")]
        PENJUALAN = 50,
        [Description("RETUR PENJUALAN")]
        RETUR_PENJUALAN = 51,
        [Description("POTONGAN PENJUALAN")]
        POTONGAN_PENJUALAN = 52,
        [Description("BAHAN BAKU")]
        BAHAN_BAKU = 53,
        [Description("UPAH LANGSUNG")]
        UPAH_LANGSUNG = 54,
        [Description("BEBAN PRODUKSI TIDAK LANGSUNG")]
        BEBAN_PRODUKSI_TIDAK_LANGSUNG = 55,
        [Description("BEBAN PABRIKASI STANDAR")]
        BEBAN_PRABIKASI_STANDAR = 56,
        [Description("BEBAN POKOK PENJUALAN")]
        BEBAN_POKOK_PENJUALAN = 59,

        [Description("BEBAN USAHA")]
        BEBAN_USAHA_HEADER = 6,
        [Description("BEBAN USAHA")]
        BEBAN_USAHA_SUBHEADER = 60,

        [Description("PENGHASILAN & BEBAN DI LUAR USAHA")]
        PENGHASILAN_BEBAN_DI_LUAR_USAHA = 7,
        [Description("PENGHASILAN DI LUAR USAHA")]
        PENGHASILAN_DI_LUAR_USAHA = 70,
        [Description("BEBAN DI LUAR USAHA")]
        BEBAN_DI_LUAR_USAHA = 71,

        [Description("POS LUAR BIASA")]
        POS_LUAR_BIASA_HEADER = 8,
        [Description("POS LUAR BIASA")]
        POS_LUAR_BIASA_SUBHEADER = 80,

        [Description("PAJAK PENGHASILAN")]
        PAJAK_PENGHASILAN_HEADER = 9,
        [Description("PAJAK PENGHASILAN")]
        PAJAK_PENGHASILAN_SUBHEADER = 90,

    }

    // Display Friendly Name for enum
    // source : https://www.codingame.com/playgrounds/2487/c---how-to-display-friendly-names-for-enumerations
    public static class COACategoryEnumExtensions
    {
        public static string ToDescriptionString(this COACategoryEnum me)
        {
            Type enumType = me.GetType();
            MemberInfo[] memberInfo = enumType.GetMember(me.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var _attr = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if(_attr != null && _attr.Count() > 0)
                {
                    return ((DescriptionAttribute)_attr.ElementAt(0)).Description;
                }
            }
            return me.ToString();
        }
    }

}

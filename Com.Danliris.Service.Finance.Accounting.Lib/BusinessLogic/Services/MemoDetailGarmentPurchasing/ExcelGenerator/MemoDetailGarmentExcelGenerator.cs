using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.MemoDetailGarmentPurchasing.ExcelGenerator
{
    public static class MemoDetailGarmentExcelGenerator
    {
        private static readonly List<MonthName> _months = new List<MonthName>()
        {
            new MonthName(1, "Januari"),
            new MonthName(2, "Februari"),
            new MonthName(3, "Maret"),
            new MonthName(4, "April"),
            new MonthName(5, "Mei"),
            new MonthName(6, "Juni"),
            new MonthName(7, "Juli"),
            new MonthName(8, "Agustus"),
            new MonthName(9, "September"),
            new MonthName(10, "Oktober"),
            new MonthName(11, "November"),
            new MonthName(12, "Desember"),
        };

        public static MemoryStream GenerateExcel(ReadResponse<ReportRincian> data, string filename, int month, int year)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(filename);

                SetHeader(worksheet, filename);
                SetData(worksheet, data, month, year);

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream;
            }
        }

        private static void SetHeader(ExcelWorksheet worksheet, string filename)
        {
            var title = filename.ToUpper();

            worksheet.Cells["A1"].Value = title;
            worksheet.Cells["A1:C1"].Merge = true;
            worksheet.Cells["A1"].Style.Font.Size = 20;
        }

        private static void SetData(ExcelWorksheet worksheet, ReadResponse<ReportRincian> data, int month, int year)
        {
            var monthName = _months.FirstOrDefault(element => element.Key == month);
            worksheet.Cells["A2"].Value = $"Periode {monthName.Value} {year}";
            worksheet.Cells["A2"].Style.Font.Size = 18;
            worksheet.Cells["A2:B2"].Merge = true;

            worksheet.Cells["A4"].Value = "Nomor Memo";
            worksheet.Cells["A4"].Style.Font.Size = 14;
            worksheet.Cells["B4"].Value = "Tanggal Memo";
            worksheet.Cells["B4"].Style.Font.Size = 14;
            worksheet.Cells["C4"].Value = "No. BP Besar";
            worksheet.Cells["C4"].Style.Font.Size = 14;
            worksheet.Cells["D4"].Value = "No. BP Kecil";
            worksheet.Cells["D4"].Style.Font.Size = 14;
            worksheet.Cells["E4"].Value = "No. Nota Intern";
            worksheet.Cells["E4"].Style.Font.Size = 14;
            worksheet.Cells["F4"].Value = "No. Surat Jalan";
            worksheet.Cells["F4"].Style.Font.Size = 14;
            worksheet.Cells["G4"].Value = "Kode Supplier";
            worksheet.Cells["G4"].Style.Font.Size = 14;
            worksheet.Cells["H4"].Value = "Nama Supplier";
            worksheet.Cells["H4"].Style.Font.Size = 14;
            worksheet.Cells["I4"].Value = "Kurs";
            worksheet.Cells["I4"].Style.Font.Size = 14;
            worksheet.Cells["J4"].Value = "Kurs Pembayaran";
            worksheet.Cells["J4"].Style.Font.Size = 14;
            worksheet.Cells["K4"].Value = "Kurs Beli";
            worksheet.Cells["K4"].Style.Font.Size = 14;
            worksheet.Cells["L4"].Value = "Jumlah";
            worksheet.Cells["L4"].Style.Font.Size = 14;
            worksheet.Cells["M4"].Value = "Jumlah Dalam Rupiah";
            worksheet.Cells["M4"].Style.Font.Size = 14;

            if (data.Data.Count > 0)
            {
                var currentRow = 5;
                var total = 0;
                
                foreach (var item in data.Data)
                {
                    //DateTimeOffset date = (DateTimeOffset)item.MemoDate;

                    worksheet.Cells[$"A{currentRow}"].Value = item.MemoNo;
                    worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"B{currentRow}"].Value = item.MemoDate.Value.ToString("dd-MMM-yyyy");
                    worksheet.Cells[$"B{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"B{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[$"C{currentRow}"].Value = item.BillsNo;
                    worksheet.Cells[$"C{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"D{currentRow}"].Value = item.PaymentBills;
                    worksheet.Cells[$"D{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[$"E{currentRow}"].Value = item.InternalNoteNo;
                    worksheet.Cells[$"E{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"F{currentRow}"].Value = item.GarmentDeliveryOrderNo;
                    worksheet.Cells[$"F{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"G{currentRow}"].Value = item.SupplierCode != null ? item.SupplierCode : "";
                    worksheet.Cells[$"G{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"H{currentRow}"].Value = item.SupplierName != null ? item.SupplierName : "";
                    worksheet.Cells[$"H{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"I{currentRow}"].Value = item.CurrencyCode;
                    worksheet.Cells[$"I{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"J{currentRow}"].Value = item.PaymentRate;
                    worksheet.Cells[$"J{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"J{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[$"K{currentRow}"].Value = item.PurchasingRate;
                    worksheet.Cells[$"K{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"K{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[$"L{currentRow}"].Value = item.MemoAmount.ToString("N2");
                    worksheet.Cells[$"L{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"L{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[$"M{currentRow}"].Value = item.MemoIdrAmount.ToString("N2");
                    worksheet.Cells[$"M{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"M{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    total += item.MemoIdrAmount;
                    currentRow++;
                }

                worksheet.Cells[$"A4:M{currentRow - 1}"].AutoFitColumns();
                worksheet.Cells[$"A4:M{currentRow - 1}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[$"A4:M{currentRow - 1}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[$"A4:M{currentRow - 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[$"A4:M{currentRow - 1}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                worksheet.Cells[$"L{currentRow}"].Value = "Total";
                worksheet.Cells[$"L{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"L{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"L{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet.Cells[$"M{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[$"M{currentRow}"].Value = total.ToString("N2");
                worksheet.Cells[$"M{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"M{currentRow}"].Style.Font.Bold = true;

                worksheet.Cells[$"L{currentRow}:M{currentRow }"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[$"L{currentRow}:M{currentRow }"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[$"L{currentRow}:M{currentRow }"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[$"L{currentRow}:M{currentRow }"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            }
        }
    }

    public class MonthName
    {
        public MonthName(int key, string value)
        {
            Key = key;
            Value = value;
        }

        public int Key { get; private set; }
        public string Value { get; private set; }
    }
}

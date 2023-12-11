using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance.Excel
{
    public static class GarmentDebtBalanceValasLokalExcel
    {
        public static MemoryStream GenerateExcel(GarmentDebtBalanceSummaryAndTotalCurrencyDto data, int month, int year, string supplierName, bool isImport, int timeZone)
        {
            CultureInfo ci = new CultureInfo("en-us");
            var result = data;
            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Supplier", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Mata Uang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "<Currency>Saldo Awal", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "<Currency>Pembelian", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "<Currency>Pembayaran", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "<Currency>Saldo Akhir", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Saldo Awal", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Pembelian", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Pembayaran", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Saldo Akhir", DataType = typeof(string) });



            if (result.Data.Count > 0)
            {
                foreach (var report in result.Data)
                {
                    reportDataTable.Rows.Add(
                        report.SupplierName,
                        report.CurrencyCode,
                        report.CurrencyInitialBalance.ToString("N2", ci),
                        report.CurrencyPurchaseAmount.ToString("N2", ci),
                        report.CurrencyPaymentAmount.ToString("N2", ci),
                        report.CurrencyCurrentBalance.ToString("N2", ci),
                        report.InitialBalance.ToString("N2", ci),
                        report.PurchaseAmount.ToString("N2", ci),
                        report.PaymentAmount.ToString("N2", ci),
                        report.CurrentBalance.ToString("N2", ci)
                        );
                }

            }

            //adding total
            foreach (var summary in result.GroupTotalCurrency)
            {
                reportDataTable.Rows.Add(
                    "Total",
                    summary.CurrencyCode,
                    summary.TotalCurrencyInitialBalance.ToString("N2", ci),
                    summary.TotalCurrencyPurchase.ToString("N2", ci),
                    summary.TotalCurrencyPayment.ToString("N2", ci),
                    summary.TotalCurrencyCurrentBalance.ToString("N2", ci),
                    summary.TotalInitialBalance.ToString("N2", ci),
                    summary.TotalPurchase.ToString("N2", ci),
                    summary.TotalPayment.ToString("N2", ci),
                    summary.TotalCurrentBalance.ToString("N2", ci)
                    );
            }

            using (var package = new ExcelPackage())
            {
                var company = "PT DAN LIRIS";
                var title = "LEDGER HUTANG LOKAL VALAS";
                var monthYear = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                var monthYearStr = monthYear.ToString("dd MMMM yyyy");
                var period = monthYearStr;

                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells["A1"].Value = company;
                worksheet.Cells["A2"].Value = title;
                //worksheet.Cells["A3"].Value = supplierName;
                worksheet.Cells["A3"].Value = $"PER {period}";

                #region PrintHeaderExcel
                var rowStartHeader = 5;
                var colStartHeader = 1;
                foreach (var columns in reportDataTable.Columns)
                {
                    DataColumn column = (DataColumn)columns;
                    if (column.ColumnName == "Saldo Awal")
                    {
                        var rowStartHeaderSpan = rowStartHeader + 1;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Value = column.ColumnName;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.Font.Bold = true;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


                        worksheet.Cells[rowStartHeader, colStartHeader].Value = "Dalam Rupiah";
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 3].Merge = true;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 3].Style.Font.Bold = true;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    else if (column.ColumnName == "Pembelian" || column.ColumnName == "Pembayaran" || column.ColumnName == "Saldo Akhir")
                    {
                        var rowStartHeaderSpan = rowStartHeader + 1;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Value = column.ColumnName;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.Font.Bold = true;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    }
                    else
                    {
                        worksheet.Cells[rowStartHeader, colStartHeader].Value = column.ColumnName.Replace("<Currency>",string.Empty);
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader + 1, colStartHeader].Merge = true;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader + 1, colStartHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader + 1, colStartHeader].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader + 1, colStartHeader].Style.Font.Bold = true;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader + 1, colStartHeader].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    }
                    colStartHeader++;
                }
                #endregion
                worksheet.Cells["A7"].LoadFromDataTable(reportDataTable, false);
                for (int i = 7; i < result.Data.Count + 7; i++)
                {
                    for (int j = 1; j <= reportDataTable.Columns.Count; j++)
                    {
                        worksheet.Cells[i, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                }
                for(int i = 7 + result.Data.Count; i < result.GroupTotalCurrency.Count+7+result.Data.Count; i++)
                {
                    for (int j = 1; j <= reportDataTable.Columns.Count; j++)
                    {
                        worksheet.Cells[i, j].Style.Font.Bold = true;
                        worksheet.Cells[i, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                }
                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream;
            }
        }
    }
}

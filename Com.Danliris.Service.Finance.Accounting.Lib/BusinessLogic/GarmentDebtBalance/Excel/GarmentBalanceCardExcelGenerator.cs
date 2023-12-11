using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance.Excel
{
    public static class GarmentBalanceCardExcelGenerator
    {
        public static MemoryStream GenerateExcel(GarmentDebtBalanceIndexDto data,int month, int year, string supplierName,bool isImport,int timeZone)
        {
            CultureInfo ci = new CultureInfo("en-us");
            var result = data;
            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nama Barang", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Kategori Pembelian", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No BP Besar", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No BP Kecil", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No SJ", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Bukti Pengeluaran Bank", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No NI", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Invoice", DataType = typeof(string) });

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "DPP", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "DPP Valas", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PPN", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "PPH", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Total", DataType = typeof(string) });

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Pembelian", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Pembelian Valas", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Pembayaran", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Pembayaran Valas", DataType = typeof(string) });

            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Saldo Akhir", DataType = typeof(string) });

            var reportDataTableOnly = reportDataTable.Clone();

            if (result.Data.Count > 0)
            {
                foreach (var report in result.Data)
                {
                    reportDataTable.Rows.Add(
                        report.ArrivalDate != DateTimeOffset.MaxValue? report.ArrivalDate.AddHours(timeZone).ToString("dd/MM/yyyy"):report.ArrivalDate.ToString("dd/MM/yyyy"),
                        report.ProductNames,
                        report.PurchasingCategoryName,
                        report.BillsNo,
                        report.PaymentBills,
                        report.GarmentDeliveryOrderNo,
                        report.BankExpenditureNoteNo,
                        report.InternalNoteNo,
                        report.InvoiceNo,
                        report.DPPAmount.ToString("N2", ci),
                        report.CurrencyDPPAmount.ToString("N2", ci),
                        report.VATAmount.ToString("N2", ci),
                        report.IncomeTaxAmount.ToString("N2", ci),
                        report.TotalInvoice.ToString("N2", ci),
                        report.MutationPurchase.ToString("N2", ci),
                        report.CurrencyMutationPurchase.ToString("N2", ci),
                        report.MutationPayment.ToString("N2", ci),
                        report.CurrencyMutationPayment.ToString("N2", ci),
                        report.RemainBalance.ToString("N2", ci)
                        );

                    if(!report.IsInitialBalance && !report.IsTotalBalance)
                    {
                        reportDataTableOnly.Rows.Add(
                        report.ArrivalDate != DateTimeOffset.MaxValue ? report.ArrivalDate.AddHours(timeZone).ToString("dd/MM/yyyy") : report.ArrivalDate.ToString("dd/MM/yyyy"),
                        report.ProductNames,
                        report.PurchasingCategoryName,
                        report.BillsNo,
                        report.PaymentBills,
                        report.GarmentDeliveryOrderNo,
                        report.BankExpenditureNoteNo,
                        report.InternalNoteNo,
                        report.InvoiceNo,
                        report.DPPAmount.ToString("N2", ci),
                        report.CurrencyDPPAmount.ToString("N2", ci),
                        report.VATAmount.ToString("N2", ci),
                        report.IncomeTaxAmount.ToString("N2", ci),
                        report.TotalInvoice.ToString("N2", ci),
                        report.MutationPurchase.ToString("N2", ci),
                        report.CurrencyMutationPurchase.ToString("N2", ci),
                        report.MutationPayment.ToString("N2", ci),
                        report.CurrencyMutationPayment.ToString("N2", ci),
                        report.RemainBalance.ToString("N2", ci)
                        );
                    }
                }
                
            }

            using (var package = new ExcelPackage())
            {
                var company = "PT DAN LIRIS";
                var title = "KARTU HUTANG";
                var monthYear = new DateTime(year, month, DateTime.DaysInMonth(year,month));
                var monthYearStr = monthYear.ToString("dd MMMM yyyy");
                var period = monthYearStr;

                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells["A1"].Value = company;
                worksheet.Cells["A2"].Value = title;
                worksheet.Cells["A3"].Value = supplierName;
                worksheet.Cells["A4"].Value = $"Periode : {period}";

                #region PrintHeaderExcel
                var rowStartHeader = 5;
                var colStartHeader = 1;
                foreach (var columns in reportDataTable.Columns)
                {
                    DataColumn column = (DataColumn)columns;
                    if (column.ColumnName == "DPP")
                    {
                        var rowStartHeaderSpan = rowStartHeader + 1;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Value = column.ColumnName;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.Font.Bold = true;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


                        worksheet.Cells[rowStartHeader, colStartHeader].Value = "Nilai Invoice";
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 4].Merge = true;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 4].Style.Font.Bold = true;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    else if (column.ColumnName == "Pembelian")
                    {
                        var rowStartHeaderSpan = rowStartHeader + 1;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Value = column.ColumnName;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.Font.Bold = true;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowStartHeaderSpan, colStartHeader].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


                        worksheet.Cells[rowStartHeader, colStartHeader].Value = "Mutasi";
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 3].Merge = true;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 3].Style.Font.Bold = true;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader, colStartHeader + 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    else if (column.ColumnName == "DPP Valas" || column.ColumnName == "PPN" || column.ColumnName == "PPH" || column.ColumnName == "Total"|| column.ColumnName == "Pembayaran"||column.ColumnName == "Pembayaran Valas" || column.ColumnName == "Pembelian Valas")
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
                        worksheet.Cells[rowStartHeader, colStartHeader].Value = column.ColumnName;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader + 1, colStartHeader].Merge = true;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader + 1, colStartHeader].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader + 1, colStartHeader].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader + 1, colStartHeader].Style.Font.Bold = true;
                        worksheet.Cells[rowStartHeader, colStartHeader, rowStartHeader + 1, colStartHeader].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    }
                    colStartHeader++;
                }
                #endregion
                //worksheet.Cells["A7"].LoadFromDataTable(reportDataTable, false);
                bool isDataLoaded = false;
                for (int i = 7; i < result.Data.Count + 7; i++)
                {
                    for (int j = 1; j <= reportDataTable.Columns.Count; j++)
                    {
                        var item = result.Data[i-7];
                        var checkIfSaldoOrTotal = item.IsInitialBalance|| item.IsTotalBalance;
                        if (checkIfSaldoOrTotal)
                        {
                            var replaceCharacter = item.ProductNames.Replace("<", string.Empty).Replace(">", string.Empty);
                            var numberCellMerge = reportDataTable.Columns.Count - 1;
                            worksheet.Cells[i, j].Value = ci.TextInfo.ToTitleCase(replaceCharacter);
                            worksheet.Cells[i, j, i, numberCellMerge].Merge = true;
                            worksheet.Cells[i, j, i, numberCellMerge].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            worksheet.Cells[i, numberCellMerge + 1].Value = item.RemainBalance.ToString("N2",ci);
                            worksheet.Cells[i, numberCellMerge + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            j = reportDataTable.Columns.Count + 1; // exit column loop
                        }
                        else
                        {
                            if (!isDataLoaded)
                            {
                                worksheet.Cells[i,j].LoadFromDataTable(reportDataTableOnly, false);
                                isDataLoaded = true;
                            }
                            worksheet.Cells[i, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream;
            }
        }
    }
}

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow.ExcelGenerator
{
    public static class CashflowDivisionExcelGenerator
    {
        public static MemoryStream Generate(DivisionDto division, DateTimeOffset dueDate, int offset, BudgetCashflowDivision data)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                SetTitle(worksheet, division, dueDate);
                SetTableHeader(worksheet, data.Headers);
                SetData(worksheet, data);

                worksheet.Cells[worksheet.Cells.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream;
            }
        }

        private static void SetTableHeader(ExcelWorksheet worksheet, List<string> headers)
        {
            worksheet.Cells["A6"].Value = "KETERANGAN";
            worksheet.Cells["A6:D7"].Merge = true;
            worksheet.Cells["A6:D7"].Style.Font.Size = 14;
            worksheet.Cells["A6:D7"].Style.Font.Bold = true;
            worksheet.Cells["A6:D7"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["A6:D7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A6:D7"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            worksheet.Cells["A6:D7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A6:D7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Cells["E6:E7"].Value = "Mata Uang";
            worksheet.Cells["E6:E7"].Merge = true;
            worksheet.Cells["E6:E7"].Style.Font.Size = 14;
            worksheet.Cells["E6:E7"].Style.Font.Bold = true;
            worksheet.Cells["E6:E7"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["E6:E7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["E6:E7"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            worksheet.Cells["E6:E7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["E6:E7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            var writeableCol = 6;
            foreach (var header in headers)
            {
                worksheet.Cells[6, writeableCol, 6, writeableCol + 2].Value = header;
                worksheet.Cells[6, writeableCol, 6, writeableCol + 2].Merge = true;
                worksheet.Cells[6, writeableCol, 6, writeableCol + 2].Style.Font.Size = 14;
                worksheet.Cells[6, writeableCol, 6, writeableCol + 2].Style.Font.Bold = true;
                worksheet.Cells[6, writeableCol, 6, writeableCol + 2].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[6, writeableCol, 6, writeableCol + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[6, writeableCol, 6, writeableCol + 2].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                worksheet.Cells[6, writeableCol, 6, writeableCol + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[6, writeableCol, 6, writeableCol + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                var writeableSubCol = writeableCol;
                worksheet.Cells[7, writeableSubCol].Value = "Nominal Valas";
                worksheet.Cells[7, writeableSubCol].Merge = true;
                worksheet.Cells[7, writeableSubCol].Style.Font.Size = 14;
                worksheet.Cells[7, writeableSubCol].Style.Font.Bold = true;
                worksheet.Cells[7, writeableSubCol].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[7, writeableSubCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[7, writeableSubCol].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                worksheet.Cells[7, writeableSubCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[7, writeableSubCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                writeableSubCol++;

                worksheet.Cells[7, writeableSubCol].Value = "Nominal IDR";
                worksheet.Cells[7, writeableSubCol].Merge = true;
                worksheet.Cells[7, writeableSubCol].Style.Font.Size = 14;
                worksheet.Cells[7, writeableSubCol].Style.Font.Bold = true;
                worksheet.Cells[7, writeableSubCol].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[7, writeableSubCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[7, writeableSubCol].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                worksheet.Cells[7, writeableSubCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[7, writeableSubCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                writeableSubCol++;

                worksheet.Cells[7, writeableSubCol].Value = "Actual";
                worksheet.Cells[7, writeableSubCol].Merge = true;
                worksheet.Cells[7, writeableSubCol].Style.Font.Size = 14;
                worksheet.Cells[7, writeableSubCol].Style.Font.Bold = true;
                worksheet.Cells[7, writeableSubCol].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[7, writeableSubCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[7, writeableSubCol].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                worksheet.Cells[7, writeableSubCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[7, writeableSubCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                writeableCol += 3;
            }

            worksheet.Cells[6, writeableCol, 7, writeableCol].Value = "Total";
            worksheet.Cells[6, writeableCol, 7, writeableCol].Merge = true;
            worksheet.Cells[6, writeableCol, 7, writeableCol].Style.Font.Size = 14;
            worksheet.Cells[6, writeableCol, 7, writeableCol].Style.Font.Bold = true;
            worksheet.Cells[6, writeableCol, 7, writeableCol].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[6, writeableCol, 7, writeableCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[6, writeableCol, 7, writeableCol].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            worksheet.Cells[6, writeableCol, 7, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[6, writeableCol, 7, writeableCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        private static void SetTitle(ExcelWorksheet worksheet, DivisionDto division, DateTimeOffset dueDate)
        {
            var company = "PT DAN LIRIS";
            var title = "LAPORAN BUDGET CASH FLOW";
            var divisionName = "SEMUA DIVISI";
            if (division != null)
                divisionName = $"DIVISI: {division.Name}";

            var cultureInfo = new CultureInfo("id-ID");
            var date = $"PERIODE {dueDate.AddMonths(1).DateTime.ToString("MMMM yyyy", cultureInfo)}";

            worksheet.Cells["A1"].Value = company;
            worksheet.Cells["A1:H1"].Merge = true;
            worksheet.Cells["A1:H1"].Style.Font.Size = 20;
            worksheet.Cells["A1:H1"].Style.Font.Bold = true;
            worksheet.Cells["A2"].Value = title;
            worksheet.Cells["A2:H2"].Merge = true;
            worksheet.Cells["A2:H2"].Style.Font.Size = 20;
            worksheet.Cells["A2:H2"].Style.Font.Bold = true;
            worksheet.Cells["A3"].Value = divisionName;
            worksheet.Cells["A3:H3"].Merge = true;
            worksheet.Cells["A3:H3"].Style.Font.Size = 20;
            worksheet.Cells["A3:H3"].Style.Font.Bold = true;
            worksheet.Cells["A4"].Value = date;
            worksheet.Cells["A4:H4"].Merge = true;
            worksheet.Cells["A4:H4"].Style.Font.Size = 20;
            worksheet.Cells["A4:H4"].Style.Font.Bold = true;
        }

        private static void SetData(ExcelWorksheet worksheet, BudgetCashflowDivision data)
        {
            var currentRow = 8;
            var dynamicCol = data.Headers.Count * 3;
            foreach (var item in data.Items)
            {
                if (item.IsUseSection)
                {
                    var rowspan = item.SectionRows > 0 ? item.SectionRows : 1;
                    worksheet.Cells[$"A{currentRow}:A{currentRow + rowspan - 1}"].Value = item.CashflowType.Name;
                    worksheet.Cells[$"A{currentRow}:A{currentRow + rowspan - 1}"].Merge = true;
                    worksheet.Cells[$"A{currentRow}:A{currentRow + rowspan - 1}"].Style.Font.Bold = true;
                    worksheet.Cells[$"A{currentRow}:A{currentRow + rowspan - 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"A{currentRow}:A{currentRow + rowspan - 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[$"A{currentRow}:A{currentRow + rowspan - 1}"].Style.TextRotation = 90;
                }

                if (item.IsUseGroup)
                {
                    var rowspan = item.GroupRows > 0 ? item.GroupRows : 1;
                    worksheet.Cells[$"B{currentRow}:B{currentRow + rowspan - 1}"].Value = item.TypeName;
                    worksheet.Cells[$"B{currentRow}:B{currentRow + rowspan - 1}"].Merge = true;
                    worksheet.Cells[$"B{currentRow}:B{currentRow + rowspan - 1}"].Style.Font.Bold = true;
                    worksheet.Cells[$"B{currentRow}:B{currentRow + rowspan - 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"B{currentRow}:B{currentRow + rowspan - 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[$"B{currentRow}:B{currentRow + rowspan - 1}"].Style.TextRotation = 90;
                }

                if (item.IsLabelOnly)
                {
                    worksheet.Cells[currentRow, 3, currentRow, 6 + dynamicCol].Value = item.CashflowCategory.Name;
                    worksheet.Cells[currentRow, 3, currentRow, 6 + dynamicCol].Merge = true;
                    worksheet.Cells[currentRow, 3, currentRow, 6 + dynamicCol].Style.Font.Bold = true;
                    worksheet.Cells[currentRow, 3, currentRow, 6 + dynamicCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }

                if (item.IsSubCategory)
                {
                    if (item.IsShowSubCategoryLabel)
                    {
                        worksheet.Cells[$"D{currentRow}"].Value = item.CashflowSubCategory.Name;
                        worksheet.Cells[$"D{currentRow}"].Merge = true;
                        worksheet.Cells[$"D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    else
                    {
                        worksheet.Cells[$"D{currentRow}"].Value = "";
                        worksheet.Cells[$"D{currentRow}"].Merge = true;
                        worksheet.Cells[$"D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }

                    worksheet.Cells[$"E{currentRow}"].Value = item.Currency?.Code;
                    worksheet.Cells[$"E{currentRow}"].Merge = true;
                    worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var writeableCol = 6;
                    foreach (var subCategoryItem in item.Items)
                    {
                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.CurrencyNominal;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;

                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.Nominal;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;

                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.Actual;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;
                    }

                    worksheet.Cells[currentRow, writeableCol].Value = item.DivisionActualTotal;
                    worksheet.Cells[currentRow, writeableCol].Merge = true;
                    worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                if (item.IsGeneralSummary)
                {
                    if (item.IsShowGeneralSummaryLabel)
                    {
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Value = item.GeneralSummaryLabel;
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Merge = true;
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Style.Font.Bold = true;
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    else
                    {
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Value = "";
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Merge = true;
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }

                    worksheet.Cells[$"E{currentRow}"].Value = item.Currency?.Code;
                    worksheet.Cells[$"E{currentRow}"].Merge = true;
                    worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var writeableCol = 6;
                    foreach (var subCategoryItem in item.Items)
                    {
                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.CurrencyNominal;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;

                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.Nominal;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;

                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.Actual;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;
                    }

                    worksheet.Cells[currentRow, writeableCol].Value = item.DivisionActualTotal;
                    worksheet.Cells[currentRow, writeableCol].Merge = true;
                    worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                if (item.IsSummary)
                {
                    if (item.IsShowSummaryLabel)
                    {
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Value = item.SummaryLabel;
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Merge = true;
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Style.Font.Bold = true;
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    else
                    {
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Value = "";
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Merge = true;
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }

                    worksheet.Cells[$"E{currentRow}"].Value = item.Currency?.Code;
                    worksheet.Cells[$"E{currentRow}"].Merge = true;
                    worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var writeableCol = 6;
                    foreach (var subCategoryItem in item.Items)
                    {
                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.CurrencyNominal;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;

                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.Nominal;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;

                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.Actual;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;
                    }

                    worksheet.Cells[currentRow, writeableCol].Value = item.DivisionActualTotal;
                    worksheet.Cells[currentRow, writeableCol].Merge = true;
                    worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                if (item.IsDifference)
                {
                    if (item.IsShowDifferenceLabel)
                    {
                        worksheet.Cells[$"B{currentRow}:D{currentRow}"].Value = item.DifferenceLabel;
                        worksheet.Cells[$"B{currentRow}:D{currentRow}"].Merge = true;
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Style.Font.Bold = true;
                        worksheet.Cells[$"B{currentRow}:D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    else
                    {
                        worksheet.Cells[$"B{currentRow}:D{currentRow}"].Value = "";
                        worksheet.Cells[$"B{currentRow}:D{currentRow}"].Merge = true;
                        worksheet.Cells[$"B{currentRow}:D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }

                    worksheet.Cells[$"E{currentRow}"].Value = item.Currency?.Code;
                    worksheet.Cells[$"E{currentRow}"].Merge = true;
                    worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var writeableCol = 6;
                    foreach (var subCategoryItem in item.Items)
                    {
                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.CurrencyNominal;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;

                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.Nominal;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;

                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.Actual;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;
                    }

                    worksheet.Cells[currentRow, writeableCol].Value = item.DivisionActualTotal;
                    worksheet.Cells[currentRow, writeableCol].Merge = true;
                    worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                if (item.IsCurrencyRate)
                {
                    if (item.IsShowCurrencyLabel)
                    {
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Value = item.CurrencyRateLabel;
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Merge = true;
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Style.Font.Bold = true;
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    else
                    {
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Value = "";
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Merge = true;
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }

                    worksheet.Cells[$"E{currentRow}"].Value = item.Currency?.Code;
                    worksheet.Cells[$"E{currentRow}"].Merge = true;
                    worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[$"F{currentRow}"].Value = item.Currency == null ? 0 : item.Currency.Rate;
                    worksheet.Cells[$"F{currentRow}"].Merge = true;
                    worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[currentRow, 7, currentRow, 6 + dynamicCol].Value = "";
                    worksheet.Cells[currentRow, 7, currentRow, 6 + dynamicCol].Merge = true;
                    worksheet.Cells[currentRow, 7, currentRow, 6 + dynamicCol].Style.Font.Bold = true;
                    worksheet.Cells[currentRow, 7, currentRow, 6 + dynamicCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }

                if (item.IsGeneralSummary)
                {
                    if (item.IsShowGeneralSummaryLabel)
                    {
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Value = item.GeneralSummaryLabel;
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Merge = true;
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Style.Font.Bold = true;
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    else
                    {
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Value = "";
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Merge = true;
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }

                    worksheet.Cells[$"E{currentRow}"].Value = item.Currency?.Code;
                    worksheet.Cells[$"E{currentRow}"].Merge = true;
                    worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var writeableCol = 6;
                    foreach (var subCategoryItem in item.Items)
                    {
                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.CurrencyNominal;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;

                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.Nominal;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;

                        worksheet.Cells[currentRow, writeableCol].Value = subCategoryItem.Actual;
                        worksheet.Cells[currentRow, writeableCol].Merge = true;
                        worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        writeableCol++;
                    }

                    worksheet.Cells[currentRow, writeableCol].Value = item.DivisionActualTotal;
                    worksheet.Cells[currentRow, writeableCol].Merge = true;
                    worksheet.Cells[currentRow, writeableCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                if (item.IsEquivalent)
                {
                    worksheet.Cells[$"A{currentRow}:D{currentRow}"].Value = item.EquivalentLabel;
                    worksheet.Cells[$"A{currentRow}:D{currentRow}"].Merge = true;
                    worksheet.Cells[$"C{currentRow}:D{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"A{currentRow}:D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    worksheet.Cells[$"E{currentRow}"].Value = "IDR";
                    worksheet.Cells[$"E{currentRow}"].Merge = true;
                    worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //worksheet.Cells[$"F{currentRow}"].Value = item.Equivalent;
                    //worksheet.Cells[$"F{currentRow}"].Merge = true;
                    //worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[currentRow, 6, currentRow, 6 + (data.Headers.Count * 3) - 1].Value = "";
                    worksheet.Cells[currentRow, 6, currentRow, 6 + (data.Headers.Count * 3) - 1].Merge = true;
                    worksheet.Cells[currentRow, 6, currentRow, 6 + (data.Headers.Count * 3) - 1].Style.Font.Bold = true;
                    worksheet.Cells[currentRow, 6, currentRow, 6 + (data.Headers.Count * 3) - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    worksheet.Cells[currentRow, 6 + (data.Headers.Count * 3)].Value = item.Equivalent;
                    worksheet.Cells[currentRow, 6 + (data.Headers.Count * 3)].Merge = true;
                    worksheet.Cells[currentRow, 6 + (data.Headers.Count * 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                currentRow += 1;
            }
        }
    }
}

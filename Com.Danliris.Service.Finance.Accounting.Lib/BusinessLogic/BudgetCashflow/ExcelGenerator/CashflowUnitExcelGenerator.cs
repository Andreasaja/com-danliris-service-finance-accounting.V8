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
    public static class CashflowUnitExcelGenerator
    {
        public static MemoryStream Generate(UnitDto unit, DateTimeOffset dueDate, int offset, List<BudgetCashflowItemDto> data)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                SetTitle(worksheet, unit, dueDate);
                SetTableHeader(worksheet);
                SetData(worksheet, data);

                worksheet.Cells[worksheet.Cells.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream;
            }
        }

        private static void SetTableHeader(ExcelWorksheet worksheet)
        {
            worksheet.Cells["A6"].Value = "KETERANGAN";
            worksheet.Cells["A6:D6"].Merge = true;
            worksheet.Cells["A6:D6"].Style.Font.Size = 14;
            worksheet.Cells["A6:D6"].Style.Font.Bold = true;
            worksheet.Cells["A6:D6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["A6:D6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A6:D6"].Style.Fill.BackgroundColor.SetColor(Color.Orange);

            worksheet.Cells["E6"].Value = "Mata Uang";
            worksheet.Cells["E6"].Style.Font.Size = 14;
            worksheet.Cells["E6"].Style.Font.Bold = true;
            worksheet.Cells["E6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["E6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["E6"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            worksheet.Cells["F6"].Value = "Nominal Valas";
            worksheet.Cells["F6"].Style.Font.Size = 14;
            worksheet.Cells["F6"].Style.Font.Bold = true;
            worksheet.Cells["F6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["F6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["F6"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            worksheet.Cells["G6"].Value = "Nominal IDR";
            worksheet.Cells["G6"].Style.Font.Size = 14;
            worksheet.Cells["G6"].Style.Font.Bold = true;
            worksheet.Cells["G6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["G6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["G6"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            worksheet.Cells["H6"].Value = "Actual";
            worksheet.Cells["H6"].Style.Font.Size = 14;
            worksheet.Cells["H6"].Style.Font.Bold = true;
            worksheet.Cells["H6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["H6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["H6"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);

            worksheet.Cells["A6:H6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A6:H6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        private static void SetTitle(ExcelWorksheet worksheet, UnitDto unit, DateTimeOffset dueDate)
        {
            var company = "PT DAN LIRIS";
            var title = "LAPORAN BUDGET CASH FLOW";
            var unitName = "UNIT: ";
            if (unit != null)
                unitName += unit.Name;

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
            worksheet.Cells["A3"].Value = unitName;
            worksheet.Cells["A3:H3"].Merge = true;
            worksheet.Cells["A3:H3"].Style.Font.Size = 20;
            worksheet.Cells["A3:H3"].Style.Font.Bold = true;
            worksheet.Cells["A4"].Value = date;
            worksheet.Cells["A4:H4"].Merge = true;
            worksheet.Cells["A4:H4"].Style.Font.Size = 20;
            worksheet.Cells["A4:H4"].Style.Font.Bold = true;
        }

        private static void SetData(ExcelWorksheet worksheet, List<BudgetCashflowItemDto> data)
        {
            var currentRow = 7;
            foreach (var item in data)
            {
                if (item.IsUseSection)
                {
                    var rowspan = item.SectionRowSpan > 0 ? item.SectionRowSpan : 1;
                    worksheet.Cells[$"A{currentRow}:A{currentRow + rowspan - 1}"].Value = item.CashflowCategoryName;
                    worksheet.Cells[$"A{currentRow}:A{currentRow + rowspan - 1}"].Merge = true;
                    worksheet.Cells[$"A{currentRow}:A{currentRow + rowspan - 1}"].Style.Font.Bold = true;
                    worksheet.Cells[$"A{currentRow}:A{currentRow + rowspan - 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"A{currentRow}:A{currentRow + rowspan - 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[$"A{currentRow}:A{currentRow + rowspan - 1}"].Style.TextRotation = 90;
                }

                if (item.IsUseGroup)
                {
                    var rowspan = item.GroupRowSpan > 0 ? item.GroupRowSpan : 1;
                    worksheet.Cells[$"B{currentRow}:B{currentRow + rowspan - 1}"].Value = item.TypeName;
                    worksheet.Cells[$"B{currentRow}:B{currentRow + rowspan - 1}"].Merge = true;
                    worksheet.Cells[$"B{currentRow}:B{currentRow + rowspan - 1}"].Style.Font.Bold = true;
                    worksheet.Cells[$"B{currentRow}:B{currentRow + rowspan - 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"B{currentRow}:B{currentRow + rowspan - 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[$"B{currentRow}:B{currentRow + rowspan - 1}"].Style.TextRotation = 90;
                }

                if (item.CashflowCategoryId > 0)
                {
                    worksheet.Cells[$"C{currentRow}:H{currentRow}"].Value = item.CashflowCategoryName;
                    worksheet.Cells[$"C{currentRow}:H{currentRow}"].Merge = true;
                    worksheet.Cells[$"C{currentRow}:H{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"C{currentRow}:H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }

                if (item.SubCategoryId > 0)
                {
                    if (item.IsShowSubCategoryLabel)
                    {
                        worksheet.Cells[$"D{currentRow}"].Value = item.SubCategoryName;
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

                    worksheet.Cells[$"F{currentRow}"].Value = item.CurrencyNominal;
                    worksheet.Cells[$"F{currentRow}"].Merge = true;
                    worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"G{currentRow}"].Value = item.Nominal;
                    worksheet.Cells[$"G{currentRow}"].Merge = true;
                    worksheet.Cells[$"G{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"H{currentRow}"].Value = item.Total;
                    worksheet.Cells[$"H{currentRow}"].Merge = true;
                    worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                if (!string.IsNullOrWhiteSpace(item.TotalLabel))
                {
                    if (item.IsShowTotalLabel)
                    {
                        worksheet.Cells[$"C{currentRow}:D{currentRow}"].Value = item.TotalLabel;
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

                    worksheet.Cells[$"F{currentRow}"].Value = item.CurrencyNominal;
                    worksheet.Cells[$"F{currentRow}"].Merge = true;
                    worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"G{currentRow}"].Value = item.Nominal;
                    worksheet.Cells[$"G{currentRow}"].Merge = true;
                    worksheet.Cells[$"G{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"H{currentRow}"].Value = item.Total;
                    worksheet.Cells[$"H{currentRow}"].Merge = true;
                    worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                if (!string.IsNullOrWhiteSpace(item.DifferenceLabel))
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

                    worksheet.Cells[$"F{currentRow}"].Value = item.CurrencyNominal;
                    worksheet.Cells[$"F{currentRow}"].Merge = true;
                    worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"G{currentRow}"].Value = item.Nominal;
                    worksheet.Cells[$"G{currentRow}"].Merge = true;
                    worksheet.Cells[$"G{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"H{currentRow}"].Value = item.Total;
                    worksheet.Cells[$"H{currentRow}"].Merge = true;
                    worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                if (item.IsSummaryBalance)
                {
                    if (item.IsShowSummaryBalance)
                    {
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Value = item.SummaryBalanceLabel;
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

                    worksheet.Cells[$"F{currentRow}"].Value = item.CurrencyNominal;
                    worksheet.Cells[$"F{currentRow}"].Merge = true;
                    worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"G{currentRow}"].Value = item.Nominal;
                    worksheet.Cells[$"G{currentRow}"].Merge = true;
                    worksheet.Cells[$"G{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"H{currentRow}"].Value = item.Total;
                    worksheet.Cells[$"H{currentRow}"].Merge = true;
                    worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                if (item.IsShowSummary)
                {
                    if (item.IsShowSummaryLabel)
                    {
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Value = item.SummaryLabel;
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

                    worksheet.Cells[$"F{currentRow}"].Value = item.CurrencyNominal;
                    worksheet.Cells[$"F{currentRow}"].Merge = true;
                    worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"G{currentRow}"].Value = item.Nominal;
                    worksheet.Cells[$"G{currentRow}"].Merge = true;
                    worksheet.Cells[$"G{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"H{currentRow}"].Value = item.Total;
                    worksheet.Cells[$"H{currentRow}"].Merge = true;
                    worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                if (item.IsRealCashBalance)
                {
                    if (item.IsShowRealCashBalanceLabel)
                    {
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Value = "Saldo Real Kas";
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

                    worksheet.Cells[$"F{currentRow}"].Value = item.CurrencyNominal;
                    worksheet.Cells[$"F{currentRow}"].Merge = true;
                    worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"G{currentRow}"].Value = item.Nominal;
                    worksheet.Cells[$"G{currentRow}"].Merge = true;
                    worksheet.Cells[$"G{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"H{currentRow}"].Value = item.Total;
                    worksheet.Cells[$"H{currentRow}"].Merge = true;
                    worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                if (item.IsShowCurrencyRate)
                {

                    if (item.IsShowRealCashBalanceLabel)
                    {
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Value = "Rate";
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
                }

                if (item.IsShowRealCashDifference)
                {
                    if (item.IsShowRealCashDifferenceLabel)
                    {
                        worksheet.Cells[$"A{currentRow}:D{currentRow}"].Value = item.RealCashDifferenceLabel;
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

                    worksheet.Cells[$"F{currentRow}"].Value = item.CurrencyNominal;
                    worksheet.Cells[$"F{currentRow}"].Merge = true;
                    worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"G{currentRow}"].Value = item.Nominal;
                    worksheet.Cells[$"G{currentRow}"].Merge = true;
                    worksheet.Cells[$"G{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    worksheet.Cells[$"H{currentRow}"].Value = item.Total;
                    worksheet.Cells[$"H{currentRow}"].Merge = true;
                    worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                if (item.IsEquivalentDifference)
                {
                    worksheet.Cells[$"A{currentRow}:D{currentRow}"].Value = "Total Surplus (Defisit) Equivalent";
                    worksheet.Cells[$"A{currentRow}:D{currentRow}"].Merge = true;
                    worksheet.Cells[$"C{currentRow}:D{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"A{currentRow}:D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    worksheet.Cells[$"E{currentRow}"].Value = "IDR";
                    worksheet.Cells[$"E{currentRow}"].Merge = true;
                    worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[$"H{currentRow}"].Value = item.Total;
                    worksheet.Cells[$"H{currentRow}"].Merge = true;
                    worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                currentRow += 1;
            }
        }
    }
}

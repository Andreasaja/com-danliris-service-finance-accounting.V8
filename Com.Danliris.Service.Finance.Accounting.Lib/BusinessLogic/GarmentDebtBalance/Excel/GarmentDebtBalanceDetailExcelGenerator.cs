using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance.Excel
{
    public static class GarmentDebtBalanceDetailExcelGenerator
    {

        public static MemoryStream Generate(List<GarmentDebtBalanceDetailDto> data, DateTimeOffset arrivalDate, int timezoneOffset)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                SetTitle(worksheet, arrivalDate, timezoneOffset);
                SetTableHeader(worksheet);
                SetData(worksheet, data, timezoneOffset);

                worksheet.Cells[worksheet.Cells.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream;
            }
        }

        private static void SetData(ExcelWorksheet worksheet, List<GarmentDebtBalanceDetailDto> data, int timezoneOffset)
        {
            var currentRow = 5;

            foreach (var item in data)
            {
                worksheet.Cells[$"A{currentRow}"].Value = $"{item.SupplierCode} - {item.SupplierName}";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"B{currentRow}"].Value = item.BillNo;
                worksheet.Cells[$"B{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"C{currentRow}"].Value = item.PaymentBill;
                worksheet.Cells[$"C{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"D{currentRow}"].Value = item.DeliveryOrderNo;
                worksheet.Cells[$"D{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"E{currentRow}"].Value = item.PaymentType;
                worksheet.Cells[$"E{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"F{currentRow}"].Value = string.IsNullOrWhiteSpace(item.SupplierName) ? "" : item.ArrivalDate.GetValueOrDefault().AddHours(timezoneOffset).ToString("dd/MM/yyyy");
                worksheet.Cells[$"F{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"G{currentRow}"].Value = string.IsNullOrWhiteSpace(item.SupplierName) ? "" : item.DebtAging.ToString();
                worksheet.Cells[$"G{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"H{currentRow}"].Value = item.InternalNoteNo;
                worksheet.Cells[$"H{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"I{currentRow}"].Value = item.InvoiceNo;
                worksheet.Cells[$"I{currentRow}"].Style.Font.Size = 14;
                
                if (string.IsNullOrWhiteSpace(item.SupplierName))
                {
                    worksheet.Cells[$"J{currentRow}"].Value = item.VATNo;
                    worksheet.Cells[$"J{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"J{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"K{currentRow}"].Value = item.DPPAmount;
                    worksheet.Cells[$"K{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"K{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"K{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"L{currentRow}"].Value = item.CurrencyDPPAmount;
                    worksheet.Cells[$"L{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"L{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"L{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"M{currentRow}"].Value = item.CurrencyVATAmount == 0 ? item.VATAmount : item.CurrencyVATAmount;
                    worksheet.Cells[$"M{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"M{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"M{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"N{currentRow}"].Value = item.CurrencyIncomeTaxAmount == 0 ? item.IncomeTaxAmount : item.CurrencyIncomeTaxAmount;
                    worksheet.Cells[$"N{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"N{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"N{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"O{currentRow}"].Value = item.CurrencyTotal == 0 ? item.Total : item.CurrencyTotal;
                    worksheet.Cells[$"O{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"O{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"O{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"P{currentRow}"].Value = item.CurrencyCode;
                    worksheet.Cells[$"P{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"P{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"Q{currentRow}"].Value = item.CurrencyRate;
                    worksheet.Cells[$"Q{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"Q{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"Q{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"R{currentRow}"].Value = item.Total;
                    worksheet.Cells[$"R{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"R{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"R{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"S{currentRow}"].Value = item.CurrencyTotal;
                    worksheet.Cells[$"S{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"S{currentRow}"].Style.Font.Bold = true;
                }
                else
                {
                    worksheet.Cells[$"J{currentRow}"].Value = item.VATNo;
                    worksheet.Cells[$"J{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"K{currentRow}"].Value = item.DPPAmount;
                    worksheet.Cells[$"K{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"K{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"L{currentRow}"].Value = item.CurrencyDPPAmount;
                    worksheet.Cells[$"L{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"L{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"M{currentRow}"].Value = item.CurrencyVATAmount == 0 ? item.VATAmount : item.CurrencyVATAmount;
                    worksheet.Cells[$"M{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"M{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"N{currentRow}"].Value = item.CurrencyIncomeTaxAmount == 0 ? item.IncomeTaxAmount : item.CurrencyIncomeTaxAmount;
                    worksheet.Cells[$"N{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"N{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"O{currentRow}"].Value = item.CurrencyTotal == 0 ? item.Total : item.CurrencyTotal;
                    worksheet.Cells[$"O{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"O{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"P{currentRow}"].Value = item.CurrencyCode;
                    worksheet.Cells[$"P{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"Q{currentRow}"].Value = item.CurrencyRate;
                    worksheet.Cells[$"Q{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"Q{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"R{currentRow}"].Value = item.Total;
                    worksheet.Cells[$"R{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"R{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"S{currentRow}"].Value = item.CurrencyTotal;
                    worksheet.Cells[$"S{currentRow}"].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[$"S{currentRow}"].Style.Font.Size = 14;
                }

                currentRow++;
            }

            worksheet.Cells[$"A5:S{currentRow}"].AutoFitColumns();
            worksheet.Cells[$"A5:S{currentRow}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[$"A5:S{currentRow}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[$"A5:S{currentRow}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[$"A5:S{currentRow}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        }

        private static void SetTableHeader(ExcelWorksheet worksheet)
        {
            worksheet.Cells["A4"].Value = "SUPPLIER";
            worksheet.Cells["A4"].Style.Font.Size = 14;
            worksheet.Cells["A4"].Style.Font.Bold = true;
            worksheet.Cells["B4"].Value = "NO. BP BESAR";
            worksheet.Cells["B4"].Style.Font.Size = 14;
            worksheet.Cells["B4"].Style.Font.Bold = true;
            worksheet.Cells["C4"].Value = "NO. BP KECIL";
            worksheet.Cells["C4"].Style.Font.Size = 14;
            worksheet.Cells["C4"].Style.Font.Bold = true;
            worksheet.Cells["D4"].Value = "NO. SJ";
            worksheet.Cells["D4"].Style.Font.Size = 14;
            worksheet.Cells["D4"].Style.Font.Bold = true;
            worksheet.Cells["E4"].Value = "TIPE BAYAR";
            worksheet.Cells["E4"].Style.Font.Size = 14;
            worksheet.Cells["E4"].Style.Font.Bold = true;
            worksheet.Cells["F4"].Value = "TGL. NOTA";
            worksheet.Cells["F4"].Style.Font.Size = 14;
            worksheet.Cells["F4"].Style.Font.Bold = true;
            worksheet.Cells["G4"].Value = "UMUR HUTANG";
            worksheet.Cells["G4"].Style.Font.Size = 14;
            worksheet.Cells["G4"].Style.Font.Bold = true;
            worksheet.Cells["H4"].Value = "NOTA INTERN";
            worksheet.Cells["H4"].Style.Font.Size = 14;
            worksheet.Cells["H4"].Style.Font.Bold = true;
            worksheet.Cells["I4"].Value = "NO. INVOICE";
            worksheet.Cells["I4"].Style.Font.Size = 14;
            worksheet.Cells["I4"].Style.Font.Bold = true;
            worksheet.Cells["J4"].Value = "NO. FAKTUR";
            worksheet.Cells["J4"].Style.Font.Size = 14;
            worksheet.Cells["J4"].Style.Font.Bold = true;
            worksheet.Cells["K4"].Value = "DPP";
            worksheet.Cells["K4"].Style.Font.Size = 14;
            worksheet.Cells["K4"].Style.Font.Bold = true;
            worksheet.Cells["L4"].Value = "DPP Valas";
            worksheet.Cells["L4"].Style.Font.Size = 14;
            worksheet.Cells["L4"].Style.Font.Bold = true;
            worksheet.Cells["M4"].Value = "PPN";
            worksheet.Cells["M4"].Style.Font.Size = 14;
            worksheet.Cells["M4"].Style.Font.Bold = true;
            worksheet.Cells["N4"].Value = "PPh";
            worksheet.Cells["N4"].Style.Font.Size = 14;
            worksheet.Cells["N4"].Style.Font.Bold = true;
            worksheet.Cells["O4"].Value = "TOTAL (DPP + PPN - PPh)";
            worksheet.Cells["O4"].Style.Font.Size = 14;
            worksheet.Cells["O4"].Style.Font.Bold = true;
            worksheet.Cells["P4"].Value = "MATA UANG";
            worksheet.Cells["P4"].Style.Font.Size = 14;
            worksheet.Cells["P4"].Style.Font.Bold = true;
            worksheet.Cells["Q4"].Value = "RATE";
            worksheet.Cells["Q4"].Style.Font.Size = 14;
            worksheet.Cells["Q4"].Style.Font.Bold = true;
            worksheet.Cells["R4"].Value = "TOTAL (IDR)";
            worksheet.Cells["R4"].Style.Font.Size = 14;
            worksheet.Cells["R4"].Style.Font.Bold = true;
            worksheet.Cells["S4"].Value = "TOTAL (Valas)";
            worksheet.Cells["S4"].Style.Font.Size = 14;
            worksheet.Cells["S4"].Style.Font.Bold = true;

            worksheet.Cells["A4:S4"].AutoFitColumns();
            worksheet.Cells["A4:S4"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A4:S4"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A4:S4"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A4:S4"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        }

        private static void SetTitle(ExcelWorksheet worksheet, DateTimeOffset arrivalDate, int timezoneOffset)
        {
            var cultureInfo = new CultureInfo("id-ID");
            var period = $"PER {arrivalDate.AddHours(timezoneOffset).ToString("MMMM yyyy", cultureInfo)}";

            var company = "PT DAN LIRIS";

            var title = "LAPORAN RINCIAN HUTANG";

            worksheet.Cells["A1"].Value = company;
            worksheet.Cells["A1:S1"].Merge = true;
            worksheet.Cells["A1:S1"].Style.Font.Size = 20;
            worksheet.Cells["A1:S1"].Style.Font.Bold = true;
            worksheet.Cells["A2"].Value = title;
            worksheet.Cells["A2:S2"].Merge = true;
            worksheet.Cells["A2:S2"].Style.Font.Size = 20;
            worksheet.Cells["A2:S2"].Style.Font.Bold = true;
            worksheet.Cells["A3"].Value = period;
            worksheet.Cells["A3:S3"].Merge = true;
            worksheet.Cells["A3:S3"].Style.Font.Size = 20;
            worksheet.Cells["A3:S3"].Style.Font.Bold = true;
        }
    }
}

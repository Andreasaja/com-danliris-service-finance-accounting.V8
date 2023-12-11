using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote.Excel
{
    public class DPPVATBankExpenditureNoteExcelGenerator
    {
        public static MemoryStream Generate(List<ReportDto> data, DateTimeOffset startDate, DateTimeOffset endDate, int timezoneOffset)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                SetTitle(worksheet, startDate, endDate, timezoneOffset);
                SetTableHeader(worksheet, timezoneOffset);
                SetData(worksheet, data, timezoneOffset);

                worksheet.Cells[worksheet.Cells.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream;
            }
        }

        private static void SetData(ExcelWorksheet worksheet, List<ReportDto> data, int timezoneOffset)
        {
            CultureInfo cultureInfo = new CultureInfo("en-us");
            var currentRow = 6;
            var index = 1;
            foreach (var datum in data)
            {
                var totalDo = datum.DetailDO.Count;
                var indexDo = totalDo - 1 < 0?0: totalDo - 1 ;
                worksheet.Cells[$"A{currentRow}"].Value = index;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"A{currentRow}:A{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"B{currentRow}"].Value = datum.ExpenditureNoteNo;
                worksheet.Cells[$"B{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"B{currentRow}:B{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"C{currentRow}"].Value = datum.ExpenditureDate.AddHours(timezoneOffset).ToString("dd/MM/yyyy");
                worksheet.Cells[$"C{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"C{currentRow}:C{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"D{currentRow}"].Value = (datum.Amount* datum.CurrencyRate).ToString("N2", cultureInfo);
                worksheet.Cells[$"D{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[$"D{currentRow}:D{currentRow + indexDo}"].Merge = true;

                if (datum.CurrencyCode != "IDR")
                {
                    worksheet.Cells[$"E{currentRow}"].Value = datum.Amount.ToString("N2", cultureInfo);
                    worksheet.Cells[$"E{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[$"E{currentRow}:E{currentRow + indexDo}"].Merge = true;
                }
                else
                {
                    worksheet.Cells[$"E{currentRow}"].Value = 0.0.ToString("N2", cultureInfo);
                    worksheet.Cells[$"E{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[$"E{currentRow}:E{currentRow + indexDo}"].Merge = true;
                }

                worksheet.Cells[$"F{currentRow}"].Value = datum.CategoryName;
                worksheet.Cells[$"F{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"F{currentRow}:F{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"G{currentRow}"].Value = datum.PaymentMethod;
                worksheet.Cells[$"G{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"G{currentRow}:G{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"H{currentRow}"].Value = datum.InvoiceAmount.ToString("N2", cultureInfo);
                worksheet.Cells[$"H{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[$"H{currentRow}:H{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"I{currentRow}"].Value = (datum.InvoiceAmount * 0.1).ToString("N2", cultureInfo);
                worksheet.Cells[$"I{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"I{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[$"I{currentRow}:I{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"J{currentRow}"].Value = 0.0;
                worksheet.Cells[$"J{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"J{currentRow}:J{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"K{currentRow}"].Value = (datum.InvoiceAmount + (datum.InvoiceAmount * 0.1)).ToString("N2", cultureInfo);
                worksheet.Cells[$"K{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"K{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[$"K{currentRow}:K{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"L{currentRow}"].Value = datum.CurrencyCode;
                worksheet.Cells[$"L{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"L{currentRow}:L{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"M{currentRow}"].Value = datum.BankName;
                worksheet.Cells[$"M{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"M{currentRow}:M{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"N{currentRow}"].Value = datum.SupplierCode;
                worksheet.Cells[$"N{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"N{currentRow}:N{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"O{currentRow}"].Value = datum.SupplierName;
                worksheet.Cells[$"O{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"O{currentRow}:O{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"P{currentRow}"].Value = datum.InternalNoteNo;
                worksheet.Cells[$"P{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"P{currentRow}:P{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"Q{currentRow}"].Value = datum.InvoiceNo;
                worksheet.Cells[$"Q{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"Q{currentRow}:Q{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"R{currentRow}"].Value = datum.InvoiceAmount.ToString("N2", cultureInfo);
                worksheet.Cells[$"R{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"R{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[$"R{currentRow}:R{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"S{currentRow}"].Value = datum.InvoiceAmount.ToString("N2", cultureInfo);
                worksheet.Cells[$"S{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"S{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[$"S{currentRow}:S{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"T{currentRow}"].Value = datum.OutstandingAmount;
                worksheet.Cells[$"T{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"T{currentRow}:T{currentRow + indexDo}"].Merge = true;

                worksheet.Cells[$"U{currentRow}"].Value = datum.OutstandingAmount;
                worksheet.Cells[$"U{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"U{currentRow}:U{currentRow + indexDo}"].Merge = true;

                foreach (var dos in datum.DetailDO)
                {
                    worksheet.Cells[$"V{currentRow}"].Value = dos.DONo;
                    worksheet.Cells[$"V{currentRow}"].Style.Font.Size = 14;
                    //worksheet.Cells[$"U{currentRow}:U{currentRow + totalDo}"].Merge = true;

                    worksheet.Cells[$"W{currentRow}"].Value = dos.BillNo;
                    worksheet.Cells[$"W{currentRow}"].Style.Font.Size = 14;
                    //worksheet.Cells[$"V{currentRow}:V{currentRow + totalDo}"].Merge = true;

                    worksheet.Cells[$"X{currentRow}"].Value = dos.PaymentBill;
                    worksheet.Cells[$"X{currentRow}"].Style.Font.Size = 14;
                    //worksheet.Cells[$"W{currentRow}:W{currentRow + totalDo}"].Merge = true;

                    worksheet.Cells[$"Y{currentRow}"].Value = dos.TotalAmount.ToString("N2", cultureInfo);
                    worksheet.Cells[$"Y{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"Y{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //worksheet.Cells[$"X{currentRow}:X{currentRow + totalDo}"].Merge = true;

                    worksheet.Cells[$"Z{currentRow}"].Value = dos.CurrencyRate;
                    worksheet.Cells[$"Z{currentRow}"].Style.Font.Size = 14;
                    //worksheet.Cells[$"Y{currentRow}:Y{currentRow + totalDo}"].Merge = true;

                    worksheet.Cells[$"AA{currentRow}"].Value = dos.CurrencyRate;
                    worksheet.Cells[$"AA{currentRow}"].Style.Font.Size = 14;
                    //worksheet.Cells[$"Z{currentRow}:Z{currentRow + totalDo}"].Merge = true;

                    worksheet.Cells[$"AB{currentRow}"].Value = (dos.TotalAmount * dos.CurrencyRate).ToString("N2", cultureInfo);
                    worksheet.Cells[$"AB{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"AB{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //worksheet.Cells[$"AA{currentRow}:AA{currentRow + totalDo}"].Merge = true;

                    worksheet.Cells[$"AC{currentRow}"].Value = (dos.TotalAmount * dos.CurrencyRate).ToString("N2", cultureInfo);
                    worksheet.Cells[$"AC{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"AC{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //worksheet.Cells[$"AB{currentRow}:AB{currentRow + totalDo}"].Merge = true;

                    worksheet.Cells[$"AD{currentRow}"].Value = (dos.TotalAmount - (dos.TotalAmount * dos.CurrencyRate)).ToString("N2", cultureInfo);
                    worksheet.Cells[$"AD{currentRow}"].Style.Font.Size = 14;
                    worksheet.Cells[$"AD{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //worksheet.Cells[$"AC{currentRow}:AC{currentRow + totalDo}"].Merge = true;
                    currentRow++;

                }

                if(totalDo < 1)
                {
                    currentRow++;
                }

                index++;
                //currentRow+= currentRow + datum.DetailDO.Count;
            }

            worksheet.Cells[$"A6:AD{currentRow}"].AutoFitColumns();
            worksheet.Cells[$"A6:AD{currentRow}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[$"A6:AD{currentRow}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[$"A6:AD{currentRow}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[$"A6:AD{currentRow}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        private static void SetTableHeader(ExcelWorksheet worksheet, int timezoneOffset)
        {
            worksheet.Cells["A4"].Value = "No";
            worksheet.Cells["A4"].Style.Font.Size = 14;
            worksheet.Cells["A4"].Style.Font.Bold = true;
            worksheet.Cells["A4:A5"].Merge = true;

            worksheet.Cells["B4"].Value = "No Bukti Pengeluaran Bank";
            worksheet.Cells["B4"].Style.Font.Size = 14;
            worksheet.Cells["B4"].Style.Font.Bold = true;
            worksheet.Cells["B4:B5"].Merge = true;

            worksheet.Cells["C4"].Value = "Tanggal Bayar DPP+PPN";
            worksheet.Cells["C4"].Style.Font.Size = 14;
            worksheet.Cells["C4"].Style.Font.Bold = true;
            worksheet.Cells["C4:C5"].Merge = true;

            worksheet.Cells["D4"].Value = "Nilai Pembayaran Keluar";
            worksheet.Cells["D4"].Style.Font.Size = 14;
            worksheet.Cells["D4"].Style.Font.Bold = true;
            worksheet.Cells["D4:E4"].Merge= true;

            worksheet.Cells["D5"].Value = "Rupiah";
            worksheet.Cells["D5"].Style.Font.Size = 14;
            worksheet.Cells["D5"].Style.Font.Bold = true;

            worksheet.Cells["E5"].Value = "Valas";
            worksheet.Cells["E5"].Style.Font.Size = 14;
            worksheet.Cells["E5"].Style.Font.Bold = true;

            worksheet.Cells["F4"].Value = "Nama Barang";
            worksheet.Cells["F4"].Style.Font.Size = 14;
            worksheet.Cells["F4"].Style.Font.Bold = true;
            worksheet.Cells["F4:F5"].Merge= true;


            worksheet.Cells["G4"].Value = "Cara Pembayaran";
            worksheet.Cells["G4"].Style.Font.Size = 14;
            worksheet.Cells["G4"].Style.Font.Bold = true;
            worksheet.Cells["G4:G5"].Merge= true;


            worksheet.Cells["H4"].Value = "DPP";
            worksheet.Cells["H4"].Style.Font.Size = 14;
            worksheet.Cells["H4"].Style.Font.Bold = true;
            worksheet.Cells["H4:H5"].Merge = true;


            worksheet.Cells["I4"].Value = "PPN";
            worksheet.Cells["I4"].Style.Font.Size = 14;
            worksheet.Cells["I4"].Style.Font.Bold = true;
            worksheet.Cells["I4:I5"].Merge= true;


            worksheet.Cells["J4"].Value = "NK";
            worksheet.Cells["J4"].Style.Font.Size = 14;
            worksheet.Cells["J4"].Style.Font.Bold = true;
            worksheet.Cells["J4:J5"].Merge = true;


            worksheet.Cells["K4"].Value = "Total NI";
            worksheet.Cells["K4"].Style.Font.Size = 14;
            worksheet.Cells["K4"].Style.Font.Bold = true;
            worksheet.Cells["K4:K5"].Merge = true;


            worksheet.Cells["L4"].Value = "Mata Uang";
            worksheet.Cells["L4"].Style.Font.Size = 14;
            worksheet.Cells["L4"].Style.Font.Bold = true;
            worksheet.Cells["L4:L5"].Merge = true;


            worksheet.Cells["M4"].Value = "Bank Bayar PPh";
            worksheet.Cells["M4"].Style.Font.Size = 14;
            worksheet.Cells["M4"].Style.Font.Bold = true;
            worksheet.Cells["M4:M5"].Merge = true;


            worksheet.Cells["N4"].Value = "Kode Supplier";
            worksheet.Cells["N4"].Style.Font.Size = 14;
            worksheet.Cells["N4"].Style.Font.Bold = true;
            worksheet.Cells["N4:N5"].Merge = true;


            worksheet.Cells["O4"].Value = "Nama Supplier";
            worksheet.Cells["O4"].Style.Font.Size = 14;
            worksheet.Cells["O4"].Style.Font.Bold = true;
            worksheet.Cells["O4:O5"].Merge = true;


            worksheet.Cells["P4"].Value = "No NI";
            worksheet.Cells["P4"].Style.Font.Size = 14;
            worksheet.Cells["P4"].Style.Font.Bold = true;
            worksheet.Cells["P4:P5"].Merge = true;


            worksheet.Cells["Q4"].Value = "No Invoice";
            worksheet.Cells["Q4"].Style.Font.Size = 14;
            worksheet.Cells["Q4"].Style.Font.Bold = true;
            worksheet.Cells["Q4:Q5"].Merge = true;


            worksheet.Cells["R4"].Value = "Nilai Invoice";
            worksheet.Cells["R4"].Style.Font.Size = 14;
            worksheet.Cells["R4"].Style.Font.Bold = true;
            worksheet.Cells["R4:R5"].Merge= true;


            worksheet.Cells["S4"].Value = "Nilai Bayar";
            worksheet.Cells["S4"].Style.Font.Size = 14;
            worksheet.Cells["S4"].Style.Font.Bold = true;
            worksheet.Cells["S4:S5"].Merge = true;


            worksheet.Cells["T4"].Value = "Outstanding";
            worksheet.Cells["T4"].Style.Font.Size = 14;
            worksheet.Cells["T4"].Style.Font.Bold = true;
            worksheet.Cells["T4:T5"].Merge = true;

            worksheet.Cells["U4"].Value = "Selisih yang Dibayar";
            worksheet.Cells["U4"].Style.Font.Size = 14;
            worksheet.Cells["U4"].Style.Font.Bold = true;
            worksheet.Cells["U4:U5"].Merge = true;

            worksheet.Cells["V4"].Value = "No SJ";
            worksheet.Cells["V4"].Style.Font.Size = 14;
            worksheet.Cells["V4"].Style.Font.Bold = true;
            worksheet.Cells["V4:V5"].Merge = true;


            worksheet.Cells["W4"].Value = "No BP";
            worksheet.Cells["W4"].Style.Font.Size = 14;
            worksheet.Cells["W4"].Style.Font.Bold = true;
            worksheet.Cells["W4:W5"].Merge = true;


            worksheet.Cells["X4"].Value = "No BB";
            worksheet.Cells["X4"].Style.Font.Size = 14;
            worksheet.Cells["X4"].Style.Font.Bold = true;
            worksheet.Cells["X4:X5"].Merge = true;


            worksheet.Cells["Y4"].Value = "Nilai SJ";
            worksheet.Cells["Y4"].Style.Font.Size = 14;
            worksheet.Cells["Y4"].Style.Font.Bold = true;
            worksheet.Cells["Y4:Y5"].Merge = true;


            worksheet.Cells["Z4"].Value = "Rate Bayar Bulan Berjalan";
            worksheet.Cells["Z4"].Style.Font.Size = 14;
            worksheet.Cells["Z4"].Style.Font.Bold = true;
            worksheet.Cells["Z4:Z5"].Merge = true;


            worksheet.Cells["AA4"].Value = "Rate (disaat NOTA/BP diakui Hutang)";
            worksheet.Cells["AA4"].Style.Font.Size = 14;
            worksheet.Cells["AA4"].Style.Font.Bold = true;
            worksheet.Cells["AA4:AA5"].Merge = true;


            worksheet.Cells["AB4"].Value = "Nilai Bayar Rate Berjalan";
            worksheet.Cells["AB4"].Style.Font.Size = 14;
            worksheet.Cells["AB4"].Style.Font.Bold = true;
            worksheet.Cells["AB4:AB5"].Merge = true;


            worksheet.Cells["AC4"].Value = "Nilai Bayar Saat diakui Hutang";
            worksheet.Cells["AC4"].Style.Font.Size = 14;
            worksheet.Cells["AC4"].Style.Font.Bold = true;
            worksheet.Cells["AC4:AC5"].Merge = true;


            worksheet.Cells["AD4"].Value = "Selisih Kurs";
            worksheet.Cells["AD4"].Style.Font.Size = 14;
            worksheet.Cells["AD4"].Style.Font.Bold = true;
            worksheet.Cells["AD4:AD5"].Merge = true;


            worksheet.Cells["A4:AD5"].AutoFitColumns();
            worksheet.Cells["A4:AD5"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A4:AD5"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A4:AD5"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A4:AD5"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        private static void SetTitle(ExcelWorksheet worksheet, DateTimeOffset startDate, DateTimeOffset endDate, int timezoneOffset)
        {
            var company = "PT DAN LIRIS";
            var title = "LAPORAN BUKTI PENGELUARAN BANK DPP + PPN";

            var cultureInfo = new CultureInfo("id-ID");
            var date = $"PERIODE: {startDate.DateTime.AddHours(timezoneOffset).ToString("dd MMMM yyyy", cultureInfo)} sampai dengan {endDate.DateTime.AddHours(timezoneOffset).ToString("dd MMMM yyyy", cultureInfo)}";

            worksheet.Cells["A1"].Value = company;
            worksheet.Cells["A1:AD1"].Merge = true;
            worksheet.Cells["A1:AD1"].Style.Font.Size = 20;
            worksheet.Cells["A1:AD1"].Style.Font.Bold = true;
            worksheet.Cells["A2"].Value = title;
            worksheet.Cells["A2:AD2"].Merge = true;
            worksheet.Cells["A2:AD2"].Style.Font.Size = 20;
            worksheet.Cells["A2:AD2"].Style.Font.Bold = true;
            worksheet.Cells["A3"].Value = date;
            worksheet.Cells["A3:AD3"].Merge = true;
            worksheet.Cells["A3:AD3"].Style.Font.Size = 20;
            worksheet.Cells["A3:AD3"].Style.Font.Bold = true;
        }
    }
}

using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionPaymentReport.ExcelGenerator
{
    public static class DispositionPaymentReportExcelGenerator
    {
        public static MemoryStream GenerateExcel(List<GarmentDispositionPaymentReportDto> data, DateTimeOffset startDate, DateTimeOffset endDate, int timezoneOffset, GarmentPurchasingExpeditionPosition position)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Disposisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Disposisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Jatuh Tempo", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Proforma", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Supplier", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kurs", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "DPP", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "PPN", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "PPh", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Biaya Lain-lain", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Total", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kategori", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Posisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Alasan Retur", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Pembelian Kirim", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Keterangan", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Terima", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Kirim", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Verifikator", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Terima", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Bayar", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Bukti Pengeluaran Bank", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nominal yang Dibayar", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mata Uang", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "PO External", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Qty Disposisi", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Surat Jalan", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Qty Surat Jalan", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor SJ", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor BP Kecil", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor BP Besar", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor BeaCukai", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal BeaCukai", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Bon Terima", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Nota Intern", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Nota Intern", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Staff", DataType = typeof(string) });

            if (data.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", 0, 0, 0, 0, 0, "", "", "", "", "","", "", "", "", "", "", "", "", "", 0, "", "", "","", "", "", "", "", "", "", "");
            }
            else
            {
                data = data.OrderByDescending(s => s.DispositionNoteDate).ToList();
                foreach (var item in data)
                {
                    dt.Rows.Add(
                        item.DispositionNoteNo,
                        item.DispositionNoteDate.AddHours(timezoneOffset).ToString("dd/MM/yyyy"),
                        item.DispositionNoteDueDate.AddHours(timezoneOffset).ToString("dd/MM/yyyy"),
                        item.ProformaNo,
                        item.SupplierName,
                        item.CurrencyCode,
                        item.DPPAmount,
                        item.VATAmount,
                        item.IncomeTaxAmount,
                        item.OthersExpenditureAmount,
                        item.TotalAmount,
                        item.CategoryName,
                        item.PositionDescription,
                        item.SendToPurchasingRemark,
                        item.SendToVerificationDate != null ? item.SendToVerificationDate.GetValueOrDefault().AddHours(timezoneOffset).ToString("dd/MM/yyyy") : "",
                        item.Remark,
                        item.VerificationAcceptedDate != null ? item.VerificationAcceptedDate.GetValueOrDefault().AddHours(timezoneOffset).ToString("dd/MM/yyyy") : "",
                        item.VerifiedDate != null ? item.VerifiedDate.GetValueOrDefault().AddHours(timezoneOffset).ToString("dd/MM/yyyy") : "",
                        item.VerifiedBy,
                        item.CashierAcceptedDate != null ? item.CashierAcceptedDate.GetValueOrDefault().AddHours(timezoneOffset).ToString("dd/MM/yyyy") : "",
                        item.BankExpenditureNoteDate,
                        item.BankExpenditureNoteNo,
                        item.PaidAmount,
                        item.CurrencyCode,
                        item.ExternalPurchaseOrderNo,
                        item.DispositionQuantity,
                        item.DeliveryOrderNo,
                        item.DeliveryOrderQuantity,
                        item.DeliveryOrderNo,
                        item.BillsNo,
                        item.PaymentBillsNo,
                        item.CustomsNoteNo,
                        item.CustomsNoteDate != null ? item.CustomsNoteDate.GetValueOrDefault().AddHours(timezoneOffset).ToString("dd/MM/yyyy") : "",
                        item.UnitReceiptNoteNo,
                        item.InternalNoteNo,
                        item.InternalNoteDate != null ? item.InternalNoteDate.GetValueOrDefault().AddHours(timezoneOffset).ToString("dd/MM/yyyy") : "",
                        item.SendToVerificationby
                        );
                }
            }

            return CreateExcelDispositionReport(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Laporan Ekspedisi Garment") }, startDate, endDate, timezoneOffset, position, false);
        }

        public static MemoryStream GenerateExcel2(List<GarmentDispositionPaymentReportDto> data, DateTimeOffset startDate, DateTimeOffset endDate, int timezoneOffset, GarmentPurchasingExpeditionPosition position)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                SetTitle(worksheet, startDate, endDate, timezoneOffset, position);
                //SetTableHeader(worksheet, timezoneOffset);
                //SetData(worksheet, data, timezoneOffset);

                worksheet.Cells[worksheet.Cells.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream;
            }
        }

        private static void SetTitle(ExcelWorksheet worksheet, DateTimeOffset startDate, DateTimeOffset endDate, int timezoneOffset, GarmentPurchasingExpeditionPosition position)
        {
            var company = "PT DAN LIRIS";
            var title = "LAPORAN BUKTI PENGELUARAN BANK DPP + PPN";

            var cultureInfo = new CultureInfo("id-ID");
            var date = $"PERIODE: {startDate.DateTime.AddHours(timezoneOffset).ToString("dd MMMM yyyy", cultureInfo)} sampai dengan {endDate.DateTime.AddHours(timezoneOffset).ToString("dd MMMM yyyy", cultureInfo)}";

            worksheet.Cells["A1"].Value = company;
            worksheet.Cells["A1:AC1"].Merge = true;
            worksheet.Cells["A1:AC1"].Style.Font.Size = 20;
            worksheet.Cells["A1:AC1"].Style.Font.Bold = true;
            worksheet.Cells["A2"].Value = title;
            worksheet.Cells["A2:AC2"].Merge = true;
            worksheet.Cells["A2:AC2"].Style.Font.Size = 20;
            worksheet.Cells["A2:AC2"].Style.Font.Bold = true;
            worksheet.Cells["A3"].Value = date;
            worksheet.Cells["A3:AC3"].Merge = true;
            worksheet.Cells["A3:AC3"].Style.Font.Size = 20;
            worksheet.Cells["A3:AC3"].Style.Font.Bold = true;
        }

        public static MemoryStream CreateExcelDispositionReport(List<KeyValuePair<DataTable, string>> dtSourceList, DateTimeOffset startDate, DateTimeOffset endDate, int timezoneOffset, GarmentPurchasingExpeditionPosition position, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);

                sheet.Cells["A1"].Value = "PT.Dan Liris";

                sheet.Cells["A2"].Value = "LAPORAN EKSPEDISI BUKTI PEMBAYARAN DISPOSISI";

                var positionDescription = "SEMUA POSISI";
                if (position > GarmentPurchasingExpeditionPosition.Invalid)
                    positionDescription = position.ToDescriptionString();

                sheet.Cells["A3"].Value = $"{positionDescription}";

                sheet.Cells["A4"].Value = $"PERIODE : {startDate.AddHours(timezoneOffset).ToString("dd/MM/yyyy")} sampai dengan {endDate.AddHours(timezoneOffset).ToString("dd/MM/yyyy")}";

                sheet.Cells["A7"].LoadFromDataTable(item.Key, true);
                FixColumnCustom(sheet,item.Key);
                //sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                var startRow = 6;
                var startCol = 1;
                var maxROw = 7 + item.Key.Rows.Count;
                var maxColumnt = item.Key.Columns.Count;
                sheet.Cells[startRow,startCol,maxROw,maxColumnt].AutoFitColumns();
                sheet.Cells[startRow, startCol, maxROw, maxColumnt].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells[startRow, startCol, maxROw, maxColumnt].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells[startRow, startCol, maxROw, maxColumnt].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells[startRow, startCol, maxROw, maxColumnt].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        private static void FixColumnCustom(ExcelWorksheet sheet, DataTable data)
        {
            var row = 6;
            var col = 1;
            var totalColumn = data.Columns.Count;
            for (int indexColumn = 1; indexColumn <= totalColumn; indexColumn++)
            {
                if(indexColumn >5 && indexColumn < 11)
                {
                    sheet.Cells[row, indexColumn].Value = "Jumlah";
                    sheet.Cells[row, indexColumn, row, indexColumn + 4].Merge = true;
                    //col += 4;
                    indexColumn += 4;
                }
                else if(indexColumn >16 && indexColumn < 19)
                {
                    sheet.Cells[row, indexColumn].Value = "Verifikasi";
                    sheet.Cells[row, indexColumn, row, indexColumn + 1].Merge = true;
                    //col += 1;
                    indexColumn += 1;
                }
                else if (indexColumn > 19 && indexColumn < 25)
                {
                    sheet.Cells[row, indexColumn].Value = "Kasir";
                    sheet.Cells[row, indexColumn, row, indexColumn + 4].Merge = true;
                    //col += 4;
                    indexColumn += 4;
                }
                else
                {
                    var valueText = sheet.Cells[row + 1, indexColumn].Value.ToString();
                    sheet.Cells[row, indexColumn].Value = valueText;
                    sheet.Cells[row, indexColumn, row+1, indexColumn].Merge = true;
                    //col += 1;
                }
            }
        }
    }
}

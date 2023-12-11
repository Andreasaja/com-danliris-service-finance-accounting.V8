using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Helpers
{
    public static class Excel
    {
        /// <summary>
        /// Create an excel file using MemoryStream.
        /// File name is assigned later in Response.AddHeader() when you want to download.
        /// Each DataTable will be rendered in its own sheet, so you need to supply its sheet name as well.
        /// </summary>
        /// <param name="dtSourceList">A List of KeyValuePair of DataTable and its sheet name</param>
        /// <param name="styling">Default style is set to False</param>
        /// <returns>MemoryStream object to be written into Response.OutputStream</returns>
        public static MemoryStream CreateExcel(List<KeyValuePair<DataTable, string>> dtSourceList, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);
                sheet.Cells["A1"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
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

                sheet.Cells["A5"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcelWithTitle(List<KeyValuePair<DataTable, string>> dtSourceList, List<KeyValuePair<string, int>> sheetIndex, string title, string dateFrom, string dateTo, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);

                sheet.Cells["A1"].Value = "PT.Dan Liris";
                sheet.Cells["A1:D1"].Merge = true;

                sheet.Cells["A2"].Value = title;
                sheet.Cells["A2:D2"].Merge = true;

                sheet.Cells["A3"].Value = $"PERIODE : {dateFrom} sampai dengan {dateTo}";
                sheet.Cells["A3:D3"].Merge = true;

                sheet.Cells["A5"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                int index = sheetIndex.Find(x => x.Key == item.Value).Value;
                if (index > 0)
                {
                    int cells = 6;
                    if (title == "Laporan Ekspedisi Disposisi Pembayaran")
                    {
                        sheet.Cells[$"F{cells}:J{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        sheet.Cells[$"X{cells}:X{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    else if (title == "Histori Disposisi Not Verified" || title == "Laporan Disposisi Not Verified")
                        sheet.Cells[$"G{cells}:G{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    else if(title == "Laporan Saldo Bank Harian")
                    {
                        if (item.Value == "Saldo Harian")
                            sheet.Cells[$"D{cells}:F{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        else if (item.Value == "Saldo Harian Mata Uang")
                            sheet.Cells[$"B{cells}:D{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    else if(title == "Laporan Kwitansi")
                        sheet.Cells[$"C{cells}:C{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    else if (title == "Laporan Ekspedisi Realisasi VB")
                    {
                        sheet.Cells[$"J{cells}:J{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        sheet.Cells[$"L{cells}:L{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                }
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcelWithTitleNonDateFilter(List<KeyValuePair<DataTable, string>> dtSourceList, string title, string date, bool styling = false, int index = 0)
        {
            ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);

                sheet.Cells["A2"].Value = "PT. DANLIRIS";
                sheet.Cells["A2:D2"].Merge = true;

                sheet.Cells["A3"].Value = title;
                sheet.Cells["A3:D3"].Merge = true;

                sheet.Cells["A4"].Value = $"Per {date}";
                sheet.Cells["A4:D4"].Merge = true;

                sheet.Cells["A6"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                int cells = 7;
                sheet.Cells[$"G{cells}:L{(cells + index) - 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcelWithTitleNonDateFilterWithSupplierName(List<KeyValuePair<DataTable, string>> dtSourceList, string title, string suplierName, string date, bool styling = false, int index = 0)
        {
            ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);

                sheet.Cells["A2"].Value = "PT. DANLIRIS";
                sheet.Cells["A2:D2"].Merge = true;

                sheet.Cells["A3"].Value = title;
                sheet.Cells["A3:D3"].Merge = true;

                sheet.Cells["A4"].Value = suplierName;
                sheet.Cells["A4:D4"].Merge = true;

                sheet.Cells["A5"].Value = $"Per {date}";
                sheet.Cells["A5:D5"].Merge = true;

                sheet.Cells["A7"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                int cells = 7;
                sheet.Cells[$"G{cells}:L{(cells + index) - 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcelWithTitleNonDateFilterMemoReport(List<KeyValuePair<DataTable, string>> dtSourceList, string title, string date,string filter, bool styling = false, int index = 0)
        {
            ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);

                sheet.Cells["A2"].Value = "PT. DANLIRIS";
                sheet.Cells["A2:D2"].Merge = true;

                sheet.Cells["A3"].Value = title;
                sheet.Cells["A3:D3"].Merge = true;

                sheet.Cells["A4"].Value = $"Per {date}";
                sheet.Cells["A4:D4"].Merge = true;

                if (!string.IsNullOrEmpty(filter))
                {
                    sheet.Cells["F4"].Value = filter;
                    sheet.Cells["F4:G4"].Merge = true;
                }

                sheet.Cells["A6"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                int cells = 7;
                sheet.Cells[$"G{cells}:L{(cells + index) - 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream DailyMutationReportExcel(List<KeyValuePair<DataTable, string>> dtSourceList, string title, string bankAccount, string date, bool styling = false, int index = 0)
        {
            ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);

                sheet.Cells["A2"].Value = "PT. DANLIRIS";
                sheet.Cells["A2:D2"].Merge = true;

                sheet.Cells["A3"].Value = title;
                sheet.Cells["A3:D3"].Merge = true;

                sheet.Cells["A4"].Value = bankAccount;
                sheet.Cells["A4:D4"].Merge = true;

                sheet.Cells["A5"].Value = $"Per {date}";
                sheet.Cells["A5:D5"].Merge = true;

                sheet.Cells["A7"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                int cells = 8;
                sheet.Cells[$"F{cells}:I{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static ExcelPackage DailyMutationReportExcelPerSheet(ExcelPackage package, List<KeyValuePair<DataTable, string>> dtSourceList, string title, string bankAccount, string date, bool styling = false, int index = 0)
        {
            //ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);

                sheet.Cells["A2"].Value = "PT. DANLIRIS";
                sheet.Cells["A2:D2"].Merge = true;

                sheet.Cells["A3"].Value = title;
                sheet.Cells["A3:D3"].Merge = true;

                sheet.Cells["A4"].Value = bankAccount;
                sheet.Cells["A4:D4"].Merge = true;

                sheet.Cells["A5"].Value = $"Per {date}";
                sheet.Cells["A5:D5"].Merge = true;

                sheet.Cells["A7"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                int cells = 8;
                sheet.Cells[$"F{cells}:I{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            //MemoryStream stream = new MemoryStream();
            //package.SaveAs(stream);
            //return stream;
            return package;
        }

        public static MemoryStream DailyMutationReportExcel(List<KeyValuePair<DataTable, string>> dtSourceList, string title, string date, bool styling = false, int index = 0)
        {
            ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);

                sheet.Cells["A2"].Value = "PT. DANLIRIS";
                sheet.Cells["A2:D2"].Merge = true;

                sheet.Cells["A3"].Value = title;
                sheet.Cells["A3:D3"].Merge = true;

                sheet.Cells["A4"].Value = item.Key;
                sheet.Cells["A4:D4"].Merge = true;

                sheet.Cells["A5"].Value = $"Per {date}";
                sheet.Cells["A5:D5"].Merge = true;

                sheet.Cells["A7"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                int cells = 8;
                sheet.Cells[$"F{cells}:I{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static ExcelPackage DailyMutationReportExcelValasPerSheet(ExcelPackage package, List<KeyValuePair<DataTable, string>> dtSourceList, string title, string bankAccount, string date, double? rate, bool styling = false, int index = 0)
        {
            //ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var headers = new List<string> { "Tanggal", "Keterangan", "Nomor Referensi", "Jenis Referensi", "Currency", "Before", "DEBIT", "Debit2", "KREDIT", "Kredit2", "SALDO AKHIR", "After2" };
                var subHeaders = new List<string> { "Original Amount", "Equivalent", "Original Amount", "Equivalent", "Original Amount", "Equivalent" };

                //ExcelPackage package = new ExcelPackage();
                var sheet = package.Workbook.Worksheets.Add(item.Value);

                sheet.Cells["A2"].Value = "PT. DANLIRIS";
                sheet.Cells["A2:D2"].Merge = true;

                sheet.Cells["A3"].Value = title;
                sheet.Cells["A3:D3"].Merge = true;

                sheet.Cells["A4"].Value = bankAccount;
                sheet.Cells["A4:D4"].Merge = true;

                sheet.Cells["A5"].Value = $"Per {date}";
                sheet.Cells["A5:D5"].Merge = true;

                sheet.Cells["J6"].Value = "Kurs : Rp.";
                sheet.Cells["J6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                sheet.Cells["K6"].Value = $" {rate.GetValueOrDefault().ToString("#,##0.#0")}";

                sheet.Cells["G7"].Value = headers[6];
                sheet.Cells["G7:H7"].Merge = true;
                sheet.Cells["I7"].Value = headers[8];
                sheet.Cells["I7:J7"].Merge = true;
                sheet.Cells["K7"].Value = headers[10];
                sheet.Cells["K7:L7"].Merge = true;

                foreach (var i in Enumerable.Range(0, 6))
                {
                    var col = (char)('A' + i);
                    sheet.Cells[$"{col}7"].Value = headers[i];
                    sheet.Cells[$"{col}7:{col}8"].Merge = true;
                }

                foreach (var i in Enumerable.Range(0, 6))
                {
                    var col = (char)('G' + i);
                    sheet.Cells[$"{col}8"].Value = subHeaders[i];
                }

                sheet.Cells["A7:L8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A7:L8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells["A7:L8"].Style.Font.Bold = true;

                sheet.Cells["A9"].LoadFromDataTable(item.Key, false, OfficeOpenXml.Table.TableStyles.Light16);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                int cells = 9;
                sheet.Cells[$"F{cells}:L{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            }
            return package;
            //MemoryStream stream = new MemoryStream();
            //package.SaveAs(stream);
            //return stream;
        }


        public static MemoryStream DailyMutationReportExcelValas(List<KeyValuePair<DataTable, string>> dtSourceList, string title, string bankAccount, string date, double? rate,bool styling = false, int index = 0)
        {
            ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var headers = new List<string> { "Tanggal", "Keterangan", "Nomor Referensi", "Jenis Referensi", "Currency", "Before", "DEBIT", "Debit2", "KREDIT", "Kredit2", "SALDO AKHIR", "After2" };
                var subHeaders = new List<string> { "Original Amount", "Equivalent", "Original Amount", "Equivalent", "Original Amount", "Equivalent" };

                //ExcelPackage package = new ExcelPackage();
                var sheet = package.Workbook.Worksheets.Add(item.Value);

                sheet.Cells["A2"].Value = "PT. DANLIRIS";
                sheet.Cells["A2:D2"].Merge = true;

                sheet.Cells["A3"].Value = title;
                sheet.Cells["A3:D3"].Merge = true;

                sheet.Cells["A4"].Value = bankAccount;
                sheet.Cells["A4:D4"].Merge = true;

                sheet.Cells["A5"].Value = $"Per {date}";
                sheet.Cells["A5:D5"].Merge = true;

                sheet.Cells["J6"].Value = "Kurs : Rp.";
                sheet.Cells["J6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                sheet.Cells["K6"].Value = $" {rate.GetValueOrDefault().ToString("#,##0.#0")}";

                sheet.Cells["G7"].Value = headers[6];
                sheet.Cells["G7:H7"].Merge = true;
                sheet.Cells["I7"].Value = headers[8];
                sheet.Cells["I7:J7"].Merge = true;
                sheet.Cells["K7"].Value = headers[10];
                sheet.Cells["K7:L7"].Merge = true;

                foreach (var i in Enumerable.Range(0, 6))
                {
                    var col = (char)('A' + i);
                    sheet.Cells[$"{col}7"].Value = headers[i];
                    sheet.Cells[$"{col}7:{col}8"].Merge = true;
                }

                foreach (var i in Enumerable.Range(0, 6))
                {
                    var col = (char)('G' + i);
                    sheet.Cells[$"{col}8"].Value = subHeaders[i];
                }

                sheet.Cells["A7:L8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A7:L8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells["A7:L8"].Style.Font.Bold = true;

                sheet.Cells["A9"].LoadFromDataTable(item.Key, false, OfficeOpenXml.Table.TableStyles.Light16);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                int cells = 9;
                sheet.Cells[$"F{cells}:L{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                //MemoryStream stream = new MemoryStream();
                //package.SaveAs(stream);
                //return stream;
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream DailyMutationReportExcelValas(List<KeyValuePair<DataTable, string>> dtSourceList, string title, string date, bool styling = false, int index = 0)
        {
            ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);

                sheet.Cells["A2"].Value = "PT. DANLIRIS";
                sheet.Cells["A2:D2"].Merge = true;

                sheet.Cells["A3"].Value = title;
                sheet.Cells["A3:D3"].Merge = true;

                sheet.Cells["A4"].Value = item.Key;
                sheet.Cells["A4:D4"].Merge = true;

                sheet.Cells["A5"].Value = $"Per {date}";
                sheet.Cells["A5:D5"].Merge = true;

                sheet.Cells["A7"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                int cells = 8;
                sheet.Cells[$"F{cells}:I{cells + index}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcelVBStatusReport(KeyValuePair<DataTable, string> dataSource, KeyValuePair<DataTable, string> currencySource, DateTimeOffset requestDateFrom, DateTimeOffset requestDateTo, bool styling = false, double requestTotal = 0, double realizationTotal = 0)
        {
            ExcelPackage package = new ExcelPackage();
            //foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            //{
            var sheet = package.Workbook.Worksheets.Add(dataSource.Value);

            int totalRow = dataSource.Key.Rows.Count + 7;
            int period = 3;
            int from = dataSource.Key.Columns.IndexOf("No VB") + 2;
            int to = dataSource.Key.Columns.IndexOf("Aging (Hari)") + 2;
            sheet.Cells[totalRow, from, totalRow, to].Merge = true;
            sheet.Cells[period, from, period, to].Merge = true;

            sheet.Cells["L2"].Value = DateTimeOffset.Now.ToString("dd MMMM yyyy", new CultureInfo("id-ID"));
            sheet.Cells["B1"].Value = "PT.DAN LIRIS";
            sheet.Cells["B2"].Value = "LAPORAN STATUS VB";
            sheet.Cells["B6"].LoadFromDataTable(dataSource.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);

            //sheet.Cells[period, 2].Value = "PERIODE : " + item.Key.Compute("Min([Tanggal VB])", string.Empty) + " SAMPAI DENGAN " + item.Key.Compute("Max([Tanggal VB])", string.Empty);
            sheet.Cells[period, 2].Value = $"PERIODE : { requestDateFrom.Date.ToString("dd MMMM yyyy", new CultureInfo("id-ID"))} sampai dengan { requestDateTo.Date.ToString("dd MMMM yyyy", new CultureInfo("id-ID"))}";
            sheet.Cells[totalRow, 2].Value = "TOTAL";

            int jumlahVb = dataSource.Key.Columns.IndexOf("Jumlah VB") + 2;
            sheet.Cells[totalRow, jumlahVb].Value = requestTotal.ToString("#,##0.###0");

            int realisasi = dataSource.Key.Columns.IndexOf("Realisasi") + 2;
            sheet.Cells[totalRow, realisasi].Value = realizationTotal.ToString("#,##0.###0");

            int sisa = dataSource.Key.Columns.IndexOf("Sisa (Kurang/Lebih)") + 2;
            sheet.Cells[totalRow, sisa].Value = (requestTotal - realizationTotal).ToString("#,##0.###0");

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

            var sheet2 = package.Workbook.Worksheets.Add(currencySource.Value);
            sheet2.Cells["A1"].LoadFromDataTable(currencySource.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
            sheet2.Cells[sheet2.Dimension.Address].AutoFitColumns();

            //}

            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcelJournalTransaction(List<KeyValuePair<DataTable, string>> dtSourceList, DateTimeOffset dateFrom, DateTimeOffset dateTo, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();

            var sheet = package.Workbook.Worksheets.Add("Sheet 1");

            sheet.Cells["A1"].Value = "LAPORAN JURNAL TRANSAKSI";
            sheet.Cells["A1:C1"].Merge = true;

            sheet.Cells["A2"].Value = $"{dateFrom.Date} - {dateTo.Date}";
            sheet.Cells["B2:C2"].Merge = true;

            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                sheet.Cells["A4"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                var count = item.Key.Rows.Count;

                sheet.Cells[$"I5:J{4 + count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcelNoFilters(List<KeyValuePair<DataTable, string>> dtSourceList, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();
            int index = 1;
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);
                sheet.Cells["A1"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Tables[string.Format("Table{0}", index++)].ShowFilter = false;
                //sheet.Cells[sheet.Dimension.Address].Style.WrapText = true;

                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcel(List<(DataTable dataTable, string sheetName, List<(string cells, System.Enum hAlign, System.Enum vAlign)> mergeCells)> dtSourceList, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();
            foreach ((DataTable dataTable, string sheetName, List<(string, System.Enum, System.Enum)> mergeCells) in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(sheetName);
                sheet.Cells["A1"].LoadFromDataTable(dataTable, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                foreach ((string cells, System.Enum hAlign, System.Enum vAlign) in mergeCells)
                {
                    sheet.Cells[cells].Merge = true;
                    sheet.Cells[cells].Style.HorizontalAlignment = (OfficeOpenXml.Style.ExcelHorizontalAlignment)hAlign;
                    sheet.Cells[cells].Style.VerticalAlignment = (OfficeOpenXml.Style.ExcelVerticalAlignment)hAlign;
                }
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }
    }
}

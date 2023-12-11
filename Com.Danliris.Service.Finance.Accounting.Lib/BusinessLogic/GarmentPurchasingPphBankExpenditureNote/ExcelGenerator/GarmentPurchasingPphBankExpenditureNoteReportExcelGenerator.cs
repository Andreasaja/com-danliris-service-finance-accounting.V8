using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingPphBankExpenditureNote;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingPphBankExpenditureNote.ExcelGenerator
{
    public static class GarmentPurchasingPphBankExpenditureNoteReportExcelGenerator
    {
        public static MemoryStream Create(string filename, List<GarmentPurchasingPphBankExpenditureNoteModel> results, DateTimeOffset startDate, DateTimeOffset endDate, int timezoneOffset)
        {
            var memoryStream = new MemoryStream();

            using (var package = new ExcelPackage(memoryStream))
            {
                var firstelement = results.FirstOrDefault();
                var worksheet = package.Workbook.Worksheets.Add(filename);
                #region Header Title
                worksheet.Cells[2, 2].Value = "PT.Dan Liris";
                worksheet.Cells[2, 2].Style.Font.Bold = true;
                worksheet.Cells[2, 2].Style.Font.UnderLineType = ExcelUnderLineType.Single;
                worksheet.Cells[2, 2].Style.Font.UnderLine = true;
                worksheet.Cells[2, 2, 2, 16].Style.Font.Name = "Calibri";
                worksheet.Cells[2, 2, 2, 16].Style.Font.Size = 15f;
                worksheet.Cells[2, 2, 2, 16].Merge = true;

                worksheet.Cells[3, 2].Value = "Laporan Bukti Pengeluaran Bank PPH";
                worksheet.Cells[3, 2].Style.Font.Bold = true;
                worksheet.Cells[3, 2].Style.Font.UnderLineType = ExcelUnderLineType.Single;
                worksheet.Cells[3, 2].Style.Font.UnderLine = true;
                worksheet.Cells[3, 2, 3, 16].Style.Font.Name = "Calibri";
                worksheet.Cells[3, 2, 3, 16].Style.Font.Size = 15f;
                worksheet.Cells[3, 2, 3, 16].Merge = true;

                var startDateStr = startDate.Year == 1 ? "-" : startDate.ToString("dd-MM-yyyy");
                var endDateStr = endDate.Year == 1 ? "-" : endDate.ToString("dd-MM-yyyy");
                worksheet.Cells[4, 2].Value = String.Format("PERIODE : {0} sampai dengan {1}", startDateStr, endDateStr);
                worksheet.Cells[4, 2].Style.Font.Bold = true;
                worksheet.Cells[4, 2].Style.Font.UnderLineType = ExcelUnderLineType.Single;
                worksheet.Cells[4, 2].Style.Font.UnderLine = true;
                worksheet.Cells[4, 2, 4, 16].Style.Font.Name = "Calibri";
                worksheet.Cells[4, 2, 4, 16].Style.Font.Size = 15f;
                worksheet.Cells[4, 2, 4, 16].Merge = true;
                #endregion

                #region Table
                int row = 6;
                int col = 1;
                int maxDate = 1;
                double totalNilaiBayarPPH = 0;
                double totalNilaiNotaPPH = 0;
                #region HeaderTable
                var ListHeader = new List<string> {
                    "No",
                    "No Bukti Pengeluaran Bank",
                    "Tanggal Bayar PPH",
                    "Category",
                    "Nilai Bayar PPH",
                    "Mata Uang",
                    "Bank Bayar PPH",
                    "Kode Supplier",
                    "Supplier",
                    "No NI",
                    "No Invoice",
                    "No Nota Pajak PPH",
                    "Nilai Nota PPH",
                    "No SJ",
                    "No BP",
                    "No BB"
                };

                foreach (var i in ListHeader)
                {
                    worksheet.Cells[row, col].Value = i;
                    worksheet.Cells[row, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, col].Style.Font.Bold = true;
                    worksheet.Cells[row, col].Style.Font.Name = "Calibri";
                    worksheet.Cells[row, col].Style.Font.Size = 12f;
                    worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    worksheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    col++;
                }
                #endregion
                #region BodyData
                row += 1;
                col = 1;

                foreach (var item in results.Select((item, index) => new { Index = index, Data = item }))
                {
                    var dataCount = item.Data.Items.SelectMany(s=> s.GarmentPurchasingPphBankExpenditureNoteInvoices).Count()-1;
                    #region colNO
                    worksheet.Cells[row, col].Value = item.Index + 1;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, col, row + dataCount, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Name = "Calibri";
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Size = 11f;
                    //worksheet.Cells[row, col, row + 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;                 
                    worksheet.Cells[row, col, row + dataCount, col].Merge = true;
                    col++;
                    #endregion

                    #region NoBuktiPengeluaran
                    worksheet.Cells[row, col, row + dataCount, col].Value = item.Data.InvoiceOutNumber;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells[row, col, row dataCount 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, col, row + dataCount, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, col, row + dataCount, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Name = "Calibri";
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Size = 11f;
                    worksheet.Cells[row, col, row + dataCount, col].Style.WrapText = true;

                    worksheet.Cells[row, col, row + dataCount, col].Merge = true;
                    col++;
                    #endregion

                    #region TanggalBayarPPH
                    worksheet.Cells[row, col, row + dataCount, col].Value = item.Data.InvoiceOutDate.AddHours(7).ToString("dd-MM-yyy");
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells[row, col, row dataCount 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, col, row + dataCount, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, col, row + dataCount, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Name = "Calibri";
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Size = 11f;
                    worksheet.Cells[row, col, row + dataCount, col].Style.WrapText = true;

                    worksheet.Cells[row, col, row + dataCount, col].Merge = true;
                    col++;
                    #endregion

                    #region Category
                    worksheet.Cells[row, col, row + dataCount, col].Value = item.Data.Items.FirstOrDefault().GarmentPurchasingPphBankExpenditureNoteInvoices.FirstOrDefault().ProductCategory;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells[row, col, row dataCount 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, col, row + dataCount, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, col, row + dataCount, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Name = "Calibri";
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Size = 11f;
                    worksheet.Cells[row, col, row + dataCount, col].Style.WrapText = true;

                    worksheet.Cells[row, col, row + dataCount, col].Merge = true;
                    col++;
                    #endregion

                    #region NilaiBayarPPH
                    var nilaiBayarPph= item.Data.Items.Sum(s => (s.IncomeTaxTotal / 100) * s.TotalPaid);
                    worksheet.Cells[row, col, row + dataCount, col].Value = nilaiBayarPph.ToString("N2", new CultureInfo("en-US"));
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells[row, col, row dataCount 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, col, row + dataCount, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, col, row + dataCount, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Name = "Calibri";
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Size = 11f;
                    worksheet.Cells[row, col, row + dataCount, col].Style.WrapText = true;

                    worksheet.Cells[row, col, row + dataCount, col].Merge = true;

                    totalNilaiBayarPPH += nilaiBayarPph;
                    col++;
                    #endregion

                    #region MataUang
                    worksheet.Cells[row, col, row + dataCount, col].Value = item.Data.BankCurrencyCode;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells[row, col, row dataCount 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, col, row + dataCount, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, col, row + dataCount, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Name = "Calibri";
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Size = 11f;
                    worksheet.Cells[row, col, row + dataCount, col].Style.WrapText = true;

                    worksheet.Cells[row, col, row + dataCount, col].Merge = true;
                    col++;
                    #endregion

                    #region BankBayarPPH
                    worksheet.Cells[row, col, row + dataCount, col].Value = string.Format("{0} - {1} - {2} - {3}", item.Data.AccountBankName, item.Data.BankName, item.Data.AccountBankNumber, item.Data.BankCurrencyCode);
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells[row, col, row dataCount 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, col, row + dataCount, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, col, row + dataCount, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Name = "Calibri";
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Size = 11f;
                    worksheet.Cells[row, col, row + dataCount, col].Style.WrapText = true;

                    worksheet.Cells[row, col, row + dataCount, col].Merge = true;
                    col++;
                    #endregion

                    #region CodeSupplier
                    worksheet.Cells[row, col, row + dataCount, col].Value = item.Data.Items.FirstOrDefault().SupplierCode;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells[row, col, row dataCount 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, col, row + dataCount, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, col, row + dataCount, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Name = "Calibri";
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Size = 11f;
                    worksheet.Cells[row, col, row + dataCount, col].Style.WrapText = true;

                    worksheet.Cells[row, col, row + dataCount, col].Merge = true;
                    col++;
                    #endregion

                    #region SupplierName
                    worksheet.Cells[row, col, row + dataCount, col].Value = item.Data.Items.FirstOrDefault().SupplierName;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    //worksheet.Cells[row, col, row dataCount 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, col, row + dataCount, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, col, row + dataCount, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Name = "Calibri";
                    worksheet.Cells[row, col, row + dataCount, col].Style.Font.Size = 11f;
                    worksheet.Cells[row, col, row + dataCount, col].Style.WrapText = true;

                    worksheet.Cells[row, col, row + dataCount, col].Merge = true;
                    col++;
                    #endregion




                    foreach (var invoice in item.Data.Items)
                    {
                        var groupByInvoice = invoice.GarmentPurchasingPphBankExpenditureNoteInvoices.GroupBy(
                            invoiceKey => new { invoiceKey.InvoicesId, invoiceKey.InvoicesNo, invoiceKey.NPH },
                            grpInvoice => grpInvoice,
                            (invoiceKey, grpInvoice) => new { Key = invoiceKey, Grp = grpInvoice }
                            );
                        foreach (var invoiceGrp in groupByInvoice)
                        {
                            var dataCountNoDO = invoiceGrp.Grp.Count()-1;

                            #region NoNI
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Value = invoice.InternalNotesNo;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            //worksheet.Cells[row, col, row dataCountNoDO 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Font.Name = "Calibri";
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Font.Size = 11f;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.WrapText = true;

                            worksheet.Cells[row, col, row + dataCountNoDO, col].Merge = true;
                            col++;
                            #endregion

                            #region NoInvoice
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Value = invoiceGrp.Key.InvoicesNo;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            //worksheet.Cells[row, col, row dataCountNoDO 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Font.Name = "Calibri";
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Font.Size = 11f;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.WrapText = true;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Merge = true;
                            col++;
                            #endregion

                            #region NPH
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Value = invoiceGrp.Key.NPH ?? string.Empty;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            //worksheet.Cells[row, col, row dataCountNoDO 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Font.Name = "Calibri";
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Font.Size = 11f;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.WrapText = true;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Merge = true;
                            col++;
                            #endregion

                            #region Nilai NotaPPh
                            var nilaiNotaPPh = invoice.TotalPaid * (invoice.IncomeTaxTotal / 100);
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Value = nilaiNotaPPh.ToString("N2", new CultureInfo("en-US"));
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            //worksheet.Cells[row, col, row dataCountNoDO 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Font.Name = "Calibri";
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.Font.Size = 11f;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Style.WrapText = true;
                            worksheet.Cells[row, col, row + dataCountNoDO, col].Merge = true;
                            col++;
                            totalNilaiNotaPPH += nilaiNotaPPh;
                            #endregion

                            foreach (var deliveryOrderItems in invoiceGrp.Grp)
                            {
                                #region NoSJ
                                worksheet.Cells[row, col].Value = deliveryOrderItems.DoNo;
                                worksheet.Cells[row, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[row, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[row, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[row, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                //worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                worksheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                worksheet.Cells[row, col].Style.Font.Name = "Calibri";
                                worksheet.Cells[row, col].Style.Font.Size = 11f;
                                worksheet.Cells[row, col].Style.WrapText = true;
                                worksheet.Cells[row, col].Merge = true;
                                col++;
                                #endregion

                                #region NoBp
                                worksheet.Cells[row, col].Value = deliveryOrderItems.BillNo;
                                worksheet.Cells[row, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[row, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[row, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[row, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                //worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                worksheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                worksheet.Cells[row, col].Style.Font.Name = "Calibri";
                                worksheet.Cells[row, col].Style.Font.Size = 11f;
                                worksheet.Cells[row, col].Style.WrapText = true;
                                worksheet.Cells[row, col].Merge = true;
                                col++;
                                #endregion
                                #region NOBB
                                worksheet.Cells[row, col].Value = deliveryOrderItems.PaymentBill;
                                worksheet.Cells[row, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[row, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[row, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[row, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                //worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                worksheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                worksheet.Cells[row, col].Style.Font.Name = "Calibri";
                                worksheet.Cells[row, col].Style.Font.Size = 11f;
                                worksheet.Cells[row, col].Style.WrapText = true;
                                worksheet.Cells[row, col].Merge = true;
                                col -= 2;
                                #endregion
                                row++;
                            }
                            col -= 4;
                        }
                        //row -= 1;

                    }
                    //row += 1;
                    col = 1;
                }

                #endregion

                #region TotalSection
                foreach(var i in ListHeader)
                {
                    //worksheet.Cells[row, col].Value = i;
                    if(i == "Category")
                    {
                        worksheet.Cells[row, col].Value = "Total";
                    }else if(i == "Nilai Bayar PPH")
                    {
                        worksheet.Cells[row, col].Value = totalNilaiBayarPPH.ToString("N2",new CultureInfo("en-US"));
                    }else if (i == "No Nota Pajak PPH")
                    {
                        worksheet.Cells[row, col].Value = "Total";
                    }
                    else if (i == "Nilai Nota PPH")
                    {
                        worksheet.Cells[row, col].Value = totalNilaiNotaPPH.ToString("N2", new CultureInfo("en-US"));
                    }
                    else
                    {
                        worksheet.Cells[row, col].Value = "";
                    }
                    worksheet.Cells[row, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, col].Style.Font.Bold = true;
                    worksheet.Cells[row, col].Style.Font.Name = "Calibri";
                    worksheet.Cells[row, col].Style.Font.Size = 12f;
                    worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    col++;
                }
                #endregion
                #endregion
                worksheet.Cells.AutoFitColumns();
                package.Save();
            }

            memoryStream.Position = 0;
            return memoryStream;

        }
    }
}

using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates
{
    public static class DailyBankTransactionPDFTemplate
    {
        private static readonly Font _headerFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
        private static readonly Font _normalFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _normalBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);

        public static MemoryStream GeneratePdfTemplate(List<DailyBankTransactionModel> data, int month, int year, double beforeBalance, string dataAccountBank, int clientTimeZoneOffset)
        {
            var document = new Document(PageSize.A4.Rotate(), 25, 25, 25, 25);
            var stream = new MemoryStream();
            PdfWriter.GetInstance(document, stream);
            document.Open();

            SetHeader(document, month, year, dataAccountBank);

            SetReportTable(document, data, beforeBalance, clientTimeZoneOffset);

            //SetFooter(document);

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private static void SetHeader(Document document, int month, int year, string dataAccountBank)
        {
            var table = new PdfPTable(1)
            {
                WidthPercentage = 100
            };

            var cell = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            cell.Phrase = new Phrase("PT. DAN LIRIS", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Laporan Mutasi Bank Harian", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Per " + new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("dd MMMM yyyy"), _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase(dataAccountBank, _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("", _normalBoldFont);
            table.AddCell(cell);

            document.Add(table);
        }

        private static void SetReportTableHeader(PdfPTable table)
        {
            var cell = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            cell.Phrase = new Phrase("Tanggal", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Keterangan", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("No. Referensi", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Jenis Referensi", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Currency", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Debit", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Kredit", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Saldo", _smallBoldFont);
            table.AddCell(cell);
        }

        private static void SetReportTable(Document document, List<DailyBankTransactionModel> data, double beforeBalance, int clientTimeZoneOffset)
        {
            var table = new PdfPTable(8)
            {
                WidthPercentage = 100
            };
            table.SetWidths(new float[] { 10f, 15f, 10f, 10f, 6f, 15f, 15f, 15f });

            /*
            var widths = new List<int>();
            for (var i = 0; i < 9; i++)
                widths.Add(1);
            table.SetWidths(widths.ToArray());
            */

            SetReportTableHeader(table);

            foreach (var item in data)
            {
                var cell = new PdfPCell()
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_CENTER
                };

                var cellAlignLeft = new PdfPCell()
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER
                };

                var cellAlignRight = new PdfPCell()
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_CENTER
                };

                var afterBalance = beforeBalance + (item.Status.Equals("IN") ? (double)item.Nominal : (double)item.Nominal * -1);

                cell.Phrase = new Phrase(item.Date.ToOffset(new TimeSpan(clientTimeZoneOffset, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID")), _smallerFont);
                table.AddCell(cell);

                cellAlignLeft.Phrase = new Phrase(item.Remark, _smallerFont);
                table.AddCell(cellAlignLeft);

                cellAlignLeft.Phrase = new Phrase(item.ReferenceNo, _smallerFont);
                table.AddCell(cellAlignLeft);

                cellAlignLeft.Phrase = new Phrase(item.ReferenceType, _smallerFont);
                table.AddCell(cellAlignLeft);

                cell.Phrase = new Phrase(item.AccountBankCurrencyCode, _smallerFont);
                table.AddCell(cell);

                if (item.AccountBankCurrencyCode != "IDR")
                {
                    cellAlignRight.Phrase = new Phrase(item.Status.ToUpper().Equals("IN") ? item.NominalValas.ToString("#,##0.#0") : 0.ToString("#,##0.#0"), _smallerFont);
                    table.AddCell(cellAlignRight);

                    cellAlignRight.Phrase = new Phrase(item.Status.ToUpper().Equals("OUT") ? item.NominalValas.ToString("#,##0.#0") : 0.ToString("#,##0.#0"), _smallerFont);
                    table.AddCell(cellAlignRight);
                }
                else
                {
                    cellAlignRight.Phrase = new Phrase(item.Status.ToUpper().Equals("IN") ? item.Nominal.ToString("#,##0.#0") : 0.ToString("#,##0.#0"), _smallerFont);
                    table.AddCell(cellAlignRight);

                    cellAlignRight.Phrase = new Phrase(item.Status.ToUpper().Equals("OUT") ? item.Nominal.ToString("#,##0.#0") : 0.ToString("#,##0.#0"), _smallerFont);
                    table.AddCell(cellAlignRight);
                }
                
                cellAlignRight.Phrase = new Phrase(afterBalance.ToString("#,##0.#0"), _smallerFont);
                table.AddCell(cellAlignRight);
            }

            document.Add(table);
        }

        /*
        private static void SetFooter(Document document)
        {

        }
        */
    }
}
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditBalance;
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
    public static class LocalCreditBalanceReportPDFTemplate
    {
        private static readonly Font _headerFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
        private static readonly Font _normalFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _normalBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);

        public static MemoryStream GeneratePdfTemplate(List<CreditBalanceViewModel> data, int month, int year, int divisionId)
        {
            var document = new Document(PageSize.A4, 25, 25, 25, 25);
            var stream = new MemoryStream();
            PdfWriter.GetInstance(document, stream);
            document.Open();

            var divisionName = "SEMUA DIVISI";

            if (divisionId > 0)
            {
                var summary = data.FirstOrDefault();
                if (summary != null)
                {
                    divisionName = $"DIVISI {summary.DivisionName}";
                }
            }

            SetHeader(document, month, year, divisionName);

            SetReportTable(document, data);

            SetFooter(document, data);

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private static void SetHeader(Document document, int month, int year, string divisionName)
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

            cell.Phrase = new Phrase("LEDGER HUTANG LOKAL", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("PER " + new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("dd MMMM yyyy").ToUpper(), _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase(divisionName, _normalBoldFont);
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
                VerticalAlignment = Element.ALIGN_MIDDLE,
            };

            cell.Rowspan = 2;
            cell.Phrase = new Phrase("NO.", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("SUPPLIER", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("DIVISI", _normalBoldFont);
            table.AddCell(cell);

            cell.Rowspan = 1;
            cell.Phrase = new Phrase("SALDO AWAL", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("PEMBELIAN", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("PEMBAYARAN", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("PELUNASAN", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("SALDO AKHIR", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("IDR", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("IDR", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("IDR", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("IDR", _normalBoldFont);
            table.AddCell(cell);
        }

        private static void SetReportTable(Document document, List<CreditBalanceViewModel> data)
        {
            var table = new PdfPTable(8)
            {
                WidthPercentage = 100
            };
            table.SetWidths(new float[] { 5f, 15f, 15f, 15f, 15f, 15f, 15f, 15f });

            /*
            var widths = new List<int>();
            for (var i = 0; i < 6; i++)
                widths.Add(1);
            table.SetWidths(widths.ToArray());
            */

            SetReportTableHeader(table);

            int index = 1;
            foreach (var item in data)
            {
                var cell = new PdfPCell()
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };

                var cellAlignLeft = new PdfPCell()
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };

                var cellAlignRight = new PdfPCell()
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };

                cell.Phrase = new Phrase(index.ToString(), _normalFont);
                table.AddCell(cell);
                index++;

                cellAlignLeft.Phrase = new Phrase(item.SupplierName, _smallerFont);
                table.AddCell(cellAlignLeft);

                cellAlignLeft.Phrase = new Phrase(item.DivisionName, _smallerFont);
                table.AddCell(cellAlignLeft);

                cellAlignRight.Phrase = new Phrase(item.StartBalance.ToString("#,##0.#0"), _normalFont);
                table.AddCell(cellAlignRight);

                cellAlignRight.Phrase = new Phrase(item.Purchase.ToString("#,##0.#0"), _normalFont);
                table.AddCell(cellAlignRight);

                cellAlignRight.Phrase = new Phrase(item.Payment.ToString("#,##0.#0"), _normalFont);
                table.AddCell(cellAlignRight);

                cellAlignRight.Phrase = new Phrase(item.PaidAmount.ToString("#,##0.#0"), _normalFont);
                table.AddCell(cellAlignRight);

                cellAlignRight.Phrase = new Phrase(item.FinalBalance.ToString("#,##0.#0"), _normalFont);
                table.AddCell(cellAlignRight);
            }

            document.Add(table);
        }

        private static void SetFooter(Document document, List<CreditBalanceViewModel> data)
        {
            var table = new PdfPTable(8)
            {
                WidthPercentage = 100
            };
            table.SetWidths(new float[] { 5f, 15f, 15f, 15f, 15f, 15f, 15f, 15f });

            var cell = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            var cellAlignRight = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            decimal totalStartBalance = 0;
            decimal totalPurchase = 0;
            decimal totalPayment = 0;
            decimal totalFinalBalance = 0;
            foreach (var item in data)
            {
                totalStartBalance += item.StartBalance;
                totalPurchase += item.Purchase;
                totalPayment += item.Payment;
                totalFinalBalance += item.FinalBalance;
            }

            cell.Colspan = 3;
            cell.Phrase = new Phrase("TOTAL RUPIAH", _normalBoldFont);
            table.AddCell(cell);

            cellAlignRight.Colspan = 1;
            cellAlignRight.Phrase = new Phrase(totalStartBalance.ToString("#,##0.#0"), _normalBoldFont);
            table.AddCell(cellAlignRight);

            cellAlignRight.Phrase = new Phrase(totalPurchase.ToString("#,##0.#0"), _normalBoldFont);
            table.AddCell(cellAlignRight);

            cellAlignRight.Phrase = new Phrase(totalPayment.ToString("#,##0.#0"), _normalBoldFont);
            table.AddCell(cellAlignRight);

            cellAlignRight.Phrase = new Phrase("", _normalBoldFont);
            table.AddCell(cellAlignRight);

            cellAlignRight.Phrase = new Phrase(totalFinalBalance.ToString("#,##0.#0"), _normalBoldFont);
            table.AddCell(cellAlignRight);

            document.Add(table);
        }
    }
}
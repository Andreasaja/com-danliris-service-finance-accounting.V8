using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance.Pdf
{
    public static class GarmentDebtBalanceValasLokalPdf
    {
        private static readonly Font _headerFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 11);
        private static readonly Font _subHeaderFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
        private static readonly Font _normalFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _normalBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _normalBoldWhiteFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9, 0, BaseColor.White);
        private static readonly Font _smallBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _smallerBoldWhiteFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7, 0, BaseColor.White);

        public static MemoryStream Generate(GarmentDebtBalanceSummaryAndTotalCurrencyDto report, int month, int year, bool isImport, int timezoneOffset)
        {
            var document = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
            var stream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            SetTitle(document, month, year, isImport, timezoneOffset);
            SetTable(document, report, month, year, timezoneOffset);
            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }


        private static void SetTable(Document document, GarmentDebtBalanceSummaryAndTotalCurrencyDto report, int month, int year, int timezoneOffset)
        {
            var table = new PdfPTable(10)
            {
                WidthPercentage = 100,
                HorizontalAlignment = Element.ALIGN_LEFT
            };

            var cellCenter = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            var cellLeft = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            var cellRight = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            cellCenter.Rowspan = 2;
            cellCenter.Phrase = new Phrase("Supplier", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Mata Uang", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Saldo Awal", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Pembelian", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Pembayaran", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Saldo Akhir", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Rowspan = 1;
            cellCenter.Colspan = 4;
            cellCenter.Phrase = new Phrase("Dalam Rupiah", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Rowspan = 1;
            cellCenter.Colspan = 1;
            cellCenter.Phrase = new Phrase("Saldo Awal", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Pembelian", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Pembayaran", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Saldo Akhir", _subHeaderFont);
            table.AddCell(cellCenter);

            foreach (var item in report.Data)
            {

                    cellCenter.Rowspan = 1;
                    cellCenter.Colspan = 1;
                    cellCenter.Phrase = new Phrase(item.SupplierName, _normalFont);
                    table.AddCell(cellCenter);
                    cellLeft.Phrase = new Phrase(item.CurrencyCode, _normalFont);
                    table.AddCell(cellLeft);
                    cellCenter.Phrase = new Phrase(item.CurrencyInitialBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(item.CurrencyPurchaseAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(item.CurrencyPaymentAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(item.CurrencyCurrentBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(item.InitialBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(item.PurchaseAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(item.PaymentAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellCenter);
                    cellRight.Phrase = new Phrase(item.CurrentBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);

            }
            foreach (var item in report.GroupTotalCurrency)
            {

                cellCenter.Rowspan = 1;
                cellCenter.Colspan = 1;
                cellCenter.Phrase = new Phrase("Total", _normalBoldFont);
                table.AddCell(cellCenter);
                cellLeft.Phrase = new Phrase(item.CurrencyCode, _normalBoldFont);
                table.AddCell(cellLeft);
                cellCenter.Phrase = new Phrase(item.TotalCurrencyInitialBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                table.AddCell(cellCenter);          
                cellCenter.Phrase = new Phrase(item.TotalCurrencyPurchase.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                table.AddCell(cellCenter);          
                cellCenter.Phrase = new Phrase(item.TotalCurrencyPayment.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                table.AddCell(cellCenter);          
                cellCenter.Phrase = new Phrase(item.TotalCurrencyCurrentBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                table.AddCell(cellCenter);          
                cellCenter.Phrase = new Phrase(item.TotalInitialBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                table.AddCell(cellCenter);          
                cellCenter.Phrase = new Phrase(item.TotalPurchase.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                table.AddCell(cellCenter);          
                cellCenter.Phrase = new Phrase(item.TotalPayment.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                table.AddCell(cellCenter);
                cellRight.Phrase = new Phrase(item.TotalCurrentBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                table.AddCell(cellRight);

            }

            document.Add(table);

            document.Add(new Paragraph("\n"));

        }

        private static void SetTitle(Document document, int month, int year, bool isImport, int timezoneOffset)
        {
            var title = "Saldo Hutang Lokal Valas";

            var yearMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            var yearMonthStr = yearMonth.ToString("dd MMMM yyyy");

            var table = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            table.SetWidths(new float[] { 1f });

            var cellCenter = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            var cellLeft = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            cellCenter.Phrase = new Phrase("PT. DAN LIRIS", _headerFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase(title, _headerFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("PER " + yearMonthStr, _headerFont);
            table.AddCell(cellCenter);

            document.Add(table);
            document.Add(new Paragraph("\n"));
        }
    }
}

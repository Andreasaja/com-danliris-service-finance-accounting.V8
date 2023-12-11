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
    public class GarmentBalanceCardPDFGenerator
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

        public static MemoryStream Generate(GarmentDebtBalanceIndexDto data, int month, int year, bool isForeignCurrency, bool supplierIsImport, int timezoneOffset, string supplierName)
        {
            var document = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
            var stream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            SetTitle(document, month, year, supplierIsImport, timezoneOffset,supplierName);
            SetTable(document, data, month, year, timezoneOffset);
            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }


        private static void SetTable(Document document, GarmentDebtBalanceIndexDto report, int month, int year, int timezoneOffset)
        {
            var table = new PdfPTable(14)
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
            cellCenter.Phrase = new Phrase("Tanggal Bon", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Nama Barang", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Kategori Pembukuan", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("No. BP Besar", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("No. BP Kecil", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("No. SJ", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("No. Bukti Pengeluaran Bank", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("No. NI", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("No. Invoice", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Rowspan = 1;
            cellCenter.Colspan = 5;
            cellCenter.Phrase = new Phrase("Mutasi", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Rowspan = 1;
            cellCenter.Colspan = 1;
            cellCenter.Phrase = new Phrase("Pembelian", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Pembelian Valas", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Pembayaran", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Pembayaran Valas", _subHeaderFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Saldo", _subHeaderFont);
            table.AddCell(cellCenter);

            foreach (var item in report.Data)
            {
                if (item.ProductNames != "<<saldo awal>>" && item.ProductNames != "<<total>>")
                {
                    cellCenter.Rowspan = 1;
                    cellCenter.Colspan = 1;
                    cellCenter.Phrase = new Phrase(item.ArrivalDate.AddHours(timezoneOffset).ToString("dd/MM/yyyy"), _normalFont);
                    table.AddCell(cellCenter);
                    cellLeft.Phrase = new Phrase(item.ProductNames, _normalFont);
                    table.AddCell(cellLeft);
                    cellCenter.Phrase = new Phrase(item.PurchasingCategoryName, _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(item.BillsNo, _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(item.PaymentBills, _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(item.GarmentDeliveryOrderNo, _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(item.BankExpenditureNoteNo, _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(item.InternalNoteNo, _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(item.InvoiceNo, _normalFont);
                    table.AddCell(cellCenter);
                    cellRight.Phrase = new Phrase(item.MutationPurchase.ToString("N2", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.CurrencyMutationPurchase.ToString("N2", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.MutationPayment.ToString("N2", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.CurrencyMutationPayment.ToString("N2", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.RemainBalance.ToString("N2", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                }
                else if (item.ProductNames == "<<saldo awal>>")
                {
                    cellCenter.Rowspan = 1;
                    cellCenter.Colspan = 13;
                    cellCenter.Phrase = new Phrase("SALDO AWAL", _normalFont);
                    table.AddCell(cellCenter);
                    cellRight.Phrase = new Phrase(item.RemainBalance.ToString("N2", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                }
                else
                {
                    cellCenter.Rowspan = 1;
                    cellCenter.Colspan = 9;
                    cellCenter.Phrase = new Phrase("TOTAL", _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Rowspan = 1;
                    cellCenter.Colspan = 1;
                    cellRight.Phrase = new Phrase(report.Data.Sum(s => s.MutationPurchase).ToString("N2", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(report.Data.Sum(s => s.CurrencyMutationPurchase).ToString("N2", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(report.Data.Sum(s => s.MutationPayment).ToString("N2", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(report.Data.Sum(s => s.CurrencyMutationPayment).ToString("N2", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(report.Data.Sum(s => s.MutationPurchase).ToString("N2", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                }
            }

            document.Add(table);

            document.Add(new Paragraph("\n"));

        }

        private static void SetTitle(Document document, int month, int year, bool isImport, int timezoneOffset,string supplierName)
        {
            var title = "Kartu Hutang Lokal/Import";
            if (isImport)
                title = "Kartu Hutang Import";
            else
                title = "Kartu Hutang Lokal";

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
            cellCenter.Phrase = new Phrase(supplierName, _headerFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("PERIODE : " + yearMonthStr, _headerFont);
            table.AddCell(cellCenter);

            document.Add(table);
            document.Add(new Paragraph("\n"));
        }
    }
}

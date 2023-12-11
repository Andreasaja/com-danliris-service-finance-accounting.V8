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
    public static class GarmentDebtBalancePdfGenerator
    {
        private static readonly Font _headerFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _subHeaderFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _normalFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _smallFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 6);
        private static readonly Font _smallerFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 5);
        private static readonly Font _normalBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _normalBoldWhiteFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7, 0, BaseColor.White);
        private static readonly Font _smallBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 6);
        private static readonly Font _smallerBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 5);
        private static readonly Font _smallerBoldWhiteFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 5, 0, BaseColor.White);
        private static readonly List<string> _accountingCategories = new List<string>() { "BB", "BP", "BE", "PRC" };
        private static readonly List<MonthName> _months = new List<MonthName>()
        {
            new MonthName(1, "Januari"),
            new MonthName(2, "Februari"),
            new MonthName(3, "Maret"),
            new MonthName(4, "April"),
            new MonthName(5, "Mei"),
            new MonthName(6, "Juni"),
            new MonthName(7, "Juli"),
            new MonthName(8, "Agustus"),
            new MonthName(9, "September"),
            new MonthName(10, "Oktober"),
            new MonthName(11, "November"),
            new MonthName(12, "Desember"),
        };

        public static MemoryStream Generate(List<GarmentDebtBalanceSummaryDto> data, int month, int year, bool isForeignCurrency, bool supplierIsImport, int timezoneOffset)
        {
            var document = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
            var stream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            SetTitle(document, month, year, isForeignCurrency, supplierIsImport);
            SetTable(document, data, isForeignCurrency, supplierIsImport, timezoneOffset);
            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private static void SetTable(Document document, List<GarmentDebtBalanceSummaryDto> data, bool isForeignCurrency, bool supplierIsImport, int timezoneOffset)
        {
            if (supplierIsImport || isForeignCurrency)
            {
                SetTableImport(document, data, timezoneOffset);
            }
            else
            {
                SetTableLocal(document, data, timezoneOffset);
            }
        }

        private static void SetTableLocal(Document document, List<GarmentDebtBalanceSummaryDto> data, int timezoneOffset)
        {
            var table = new PdfPTable(6)
            {
                WidthPercentage = 100,
                HorizontalAlignment = Element.ALIGN_LEFT
            };

            var cellCenter = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            var cellCenterWithBackground = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_CENTER,
                BackgroundColor = BaseColor.LightGray
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

            cellCenterWithBackground.Phrase = new Phrase("SUPPLIER", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("MATA UANG", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("SALDO AWAL", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("PEMBELIAN", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("PEMBAYARAN", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("SALDO AKHIR", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);

            foreach (var datum in data)
            {
                cellLeft.Phrase = new Phrase(!string.IsNullOrWhiteSpace(datum.SupplierCode) ? $"{datum.SupplierCode} - {datum.SupplierName}" : "Total", _normalFont);
                table.AddCell(cellLeft);
                cellCenter.Phrase = new Phrase(datum.CurrencyCode, _normalFont);
                table.AddCell(cellCenter);
                cellRight.Phrase = new Phrase(datum.InitialBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                table.AddCell(cellRight);
                cellRight.Phrase = new Phrase(datum.PurchaseAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                table.AddCell(cellRight);
                cellRight.Phrase = new Phrase(datum.PaymentAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                table.AddCell(cellRight);
                cellRight.Phrase = new Phrase(datum.CurrentBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                table.AddCell(cellRight);
            }

            document.Add(table);
        }

        private static void SetTableImport(Document document, List<GarmentDebtBalanceSummaryDto> data, int timezoneOffset)
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

            var cellCenterWithBackground = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_CENTER,
                BackgroundColor = BaseColor.LightGray
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

            cellCenterWithBackground.Rowspan = 2;
            cellCenterWithBackground.Phrase = new Phrase("SUPPLIER", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("MATA UANG", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("SALDO AWAL", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("PEMBELIAN", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("PEMBAYARAN", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("SALDO AKHIR", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Rowspan = 1;
            cellCenterWithBackground.Colspan = 4;
            cellCenterWithBackground.Phrase = new Phrase("DALAM RUPIAH", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Colspan = 1;
            cellCenterWithBackground.Phrase = new Phrase("SALDO AWAL", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("PEMBELIAN", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("PEMBAYARAN", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("SALDO AKHIR", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);


            foreach (var datum in data)
            {
                cellLeft.Phrase = new Phrase(!string.IsNullOrWhiteSpace(datum.SupplierCode) ? $"{datum.SupplierCode} - {datum.SupplierName}" : "Total", _normalFont);
                table.AddCell(cellLeft);
                cellCenter.Phrase = new Phrase(datum.CurrencyCode, _normalFont);
                table.AddCell(cellCenter);
                cellRight.Phrase = new Phrase(datum.CurrencyInitialBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                table.AddCell(cellRight);
                cellRight.Phrase = new Phrase(datum.CurrencyPurchaseAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                table.AddCell(cellRight);
                cellRight.Phrase = new Phrase(datum.CurrencyPaymentAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                table.AddCell(cellRight);
                cellRight.Phrase = new Phrase(datum.CurrencyCurrentBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                table.AddCell(cellRight);
                cellRight.Phrase = new Phrase(datum.InitialBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                table.AddCell(cellRight);
                cellRight.Phrase = new Phrase(datum.PurchaseAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                table.AddCell(cellRight);
                cellRight.Phrase = new Phrase(datum.PaymentAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                table.AddCell(cellRight);
                cellRight.Phrase = new Phrase(datum.CurrentBalance.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                table.AddCell(cellRight);
            }

            document.Add(table);
        }

        private static void SetTitle(Document document, int month, int year, bool isForeignCurrency, bool supplierIsImport)
        {
            var title = "LEDGER HUTANG LOKAL";

            if (isForeignCurrency)
                title = "LEDGER HUTANG LOKAL VALAS";

            if (supplierIsImport)
                title = "LEDGER HUTANG IMPOR";

            var selectedMonth = _months.FirstOrDefault(_month => _month.Value == month);

            var table = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            table.SetWidths(new float[] { 1f });

            var cellLeft = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            cellLeft.Phrase = new Phrase("PT. DAN LIRIS", _headerFont);
            table.AddCell(cellLeft);
            cellLeft.Phrase = new Phrase(title, _headerFont);
            table.AddCell(cellLeft);
            cellLeft.Phrase = new Phrase($"PER {selectedMonth.Name.ToUpper()} {year}", _headerFont);
            table.AddCell(cellLeft);

            document.Add(table);
        }
    }

    public class MonthName
    {
        public MonthName(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public int Value { get; private set; }
        public string Name { get; private set; }
    }
}

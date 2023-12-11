using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance.Pdf
{
    public static class GarmentDebtBalanceDetailPDFGenerator
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

        public static MemoryStream Generate(List<GarmentDebtBalanceDetailDto> data, DateTimeOffset arrivalDate, int timezoneOffset)
        {
            var document = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
            var stream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            SetTitle(document, arrivalDate, timezoneOffset);
            SetTable(document, data, timezoneOffset);
            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private static void SetTable(Document document, List<GarmentDebtBalanceDetailDto> data, int timezoneOffset)
        {
            var table = new PdfPTable(19)
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
            cellCenterWithBackground.Phrase = new Phrase("NO. BP BESAR", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("NO. BP KECIL", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("NO. SJ", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("TIPE BAYAR", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("TGL NOTA", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("UMUR HUTANG", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("NOTA INTERN", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("NO. INVOICE", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("NO. FAKTUR", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("DPP", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("DPP Valas", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("PPN", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("PPh", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("TOTAL (DPP + PPN - PPh)", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("MATA UANG", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("RATE", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("TOTAL (Valas)", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);
            cellCenterWithBackground.Phrase = new Phrase("TOTAL (IDR)", _subHeaderFont);
            table.AddCell(cellCenterWithBackground);

            foreach (var datum in data)
            {
                cellLeft.Phrase = new Phrase($"{datum.SupplierCode} - {datum.SupplierName}", _normalFont);
                table.AddCell(cellLeft);
                cellCenter.Phrase = new Phrase(datum.BillNo, _normalFont);
                table.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(datum.PaymentBill, _normalFont);
                table.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(datum.DeliveryOrderNo, _normalFont);
                table.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(datum.PaymentType, _normalFont);
                table.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(string.IsNullOrWhiteSpace(datum.SupplierName) ? "" : datum.ArrivalDate.GetValueOrDefault().AddHours(timezoneOffset).ToString("dd/MM/yyyy"), _normalFont);
                table.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(string.IsNullOrWhiteSpace(datum.SupplierName) ? "" : datum.DebtAging.ToString(), _normalFont);
                table.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(datum.InternalNoteNo, _normalFont);
                table.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(datum.InvoiceNo, _normalFont);
                table.AddCell(cellCenter);
                
                if (string.IsNullOrWhiteSpace(datum.SupplierName))
                {
                    cellCenter.Phrase = new Phrase(datum.VATNo, _normalBoldFont);
                    table.AddCell(cellCenter);
                    cellRight.Phrase = new Phrase(datum.DPPAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(datum.CurrencyDPPAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(datum.CurrencyVATAmount == 0 ? datum.VATAmount.ToString("0,0.00", CultureInfo.InvariantCulture) : datum.CurrencyVATAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(datum.CurrencyIncomeTaxAmount == 0 ? datum.IncomeTaxAmount.ToString("0,0.00", CultureInfo.InvariantCulture) : datum.CurrencyIncomeTaxAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(datum.CurrencyTotal == 0 ? datum.Total.ToString("0,0.00", CultureInfo.InvariantCulture) : datum.Total.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                    table.AddCell(cellRight);
                    cellCenter.Phrase = new Phrase(datum.CurrencyCode, _normalBoldFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(datum.CurrencyRate.ToString(), _normalBoldFont);
                    table.AddCell(cellCenter);
                    cellRight.Phrase = new Phrase(datum.CurrencyTotal.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(datum.Total.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                    cellRight.Phrase = new Phrase(datum.Total.ToString("0,0.00", CultureInfo.InvariantCulture), _normalBoldFont);
                    table.AddCell(cellRight);
                }
                else
                {
                    cellCenter.Phrase = new Phrase(datum.VATNo, _normalFont);
                    table.AddCell(cellCenter);
                    cellRight.Phrase = new Phrase(datum.DPPAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(datum.CurrencyDPPAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(datum.CurrencyVATAmount == 0 ? datum.VATAmount.ToString("0,0.00", CultureInfo.InvariantCulture) : datum.CurrencyVATAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(datum.CurrencyIncomeTaxAmount == 0 ? datum.IncomeTaxAmount.ToString("0,0.00", CultureInfo.InvariantCulture) : datum.CurrencyIncomeTaxAmount.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(datum.CurrencyTotal == 0 ? datum.Total.ToString("0,0.00", CultureInfo.InvariantCulture) : datum.Total.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellCenter.Phrase = new Phrase(datum.CurrencyCode, _normalFont);
                    table.AddCell(cellCenter);
                    cellCenter.Phrase = new Phrase(datum.CurrencyRate.ToString(), _normalFont);
                    table.AddCell(cellCenter);
                    cellRight.Phrase = new Phrase(datum.CurrencyTotal.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(datum.Total.ToString("0,0.00", CultureInfo.InvariantCulture), _normalFont);
                    table.AddCell(cellRight);
                }
            }

            document.Add(table);
        }

        private static void SetTitle(Document document, DateTimeOffset arrivalDate, int timezoneOffset)
        {
            var cultureInfo = new CultureInfo("id-ID");
            var period = $"PER {arrivalDate.AddHours(timezoneOffset).ToString("MMMM yyyy", cultureInfo)}";
            var title = "LAPORAN RINCIAN HUTANG";

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
            cellLeft.Phrase = new Phrase(period, _headerFont);
            table.AddCell(cellLeft);

            document.Add(table);
        }
    }
}

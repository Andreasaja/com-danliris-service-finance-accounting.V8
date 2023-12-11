using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing;

namespace Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates
{
    public static class MemorialJobGarmentDetailPDFTemplate
    {
        private static readonly Font _headerFont = FontFactory.GetFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 16);
        private static readonly Font _header2Font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 16);
        private static readonly Font _header3Font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
        private static readonly Font _header3BoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
        private static readonly Font _normalFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _normalBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        
        public static MemoryStream GeneratePdfTemplate(List<ReportPDF> data, String month, int year)
        {
            var document = new Document(PageSize.A4.Rotate(), 25, 25, 25, 25);
            var stream = new MemoryStream();
            PdfWriter.GetInstance(document, stream);
            document.Open();

            SetHeader(document, month, year);

            SetContent(document, data);

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private static void SetHeader(Document document, String month, int year)
        {
            var table = new PdfPTable(2)
            {
                WidthPercentage = 100
            };

            var cellLeft = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            var cellRight = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            cellLeft.Phrase = new Phrase("PT. DAN LIRIS", _header2Font);
            cellLeft.PaddingLeft = 10;
            table.AddCell(cellLeft);

            cellRight.Phrase = new Phrase("LAPORAN DATA RINCIAN MEMORIAL LOKAL", _headerFont);
            cellRight.Rowspan = 3;
            table.AddCell(cellRight);

            cellLeft.Phrase = new Phrase("Kel. Banaran, Kec. Grogol, Kab. Sukoharjo 57193", _normalFont);
            cellLeft.PaddingLeft = 10;
            table.AddCell(cellLeft);

            cellLeft.Phrase = new Phrase("Jawa Tengah, Indonesia", _normalFont);
            cellLeft.PaddingLeft = 10;
            table.AddCell(cellLeft);

            cellLeft.Phrase = new Phrase("PERIODE  : " + month.ToUpper() + " " + year, _header3Font);
            cellLeft.Colspan = 2;
            cellLeft.PaddingLeft = 10;
            cellLeft.PaddingTop = 10;
            cellLeft.PaddingBottom = 5;
            table.AddCell(cellLeft);

            document.Add(table);
        }

        private static void SetContent(Document document, List<ReportPDF> data)
        {
            float[] widths = new float[] { 3, 10, 8, 10, 17, 7, 12, 9, 9, 9, 6 };

            var table = new PdfPTable(11)
            {
                WidthPercentage = 100
            };

            table.SetWidths(widths);

            var cell = new PdfPCell()
            {
                PaddingTop = 5,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            var cellItemCenter = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            var cellItemLeft = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            var cellItemRight = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            var cellColSpan8 = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Colspan = 8
            };

            cell.Phrase = new Phrase("No.", _header3Font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Nomor", _header3Font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Tanggal", _header3Font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("No. Nota", _header3Font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("No. Bon", _header3Font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Kode Supplier", _header3Font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Nama Supplier", _header3Font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Jml. Bayar (USD)", _header3Font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Jml. Bayar (IDR)", _header3Font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Jml. Beli (IDR)", _header3Font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Selisih (IDR)", _header3Font);
            table.AddCell(cell);

            var number = 1;
            var totalJmlBayarIDR = 0;
            var totalJmlBeli = 0.0;
            var totalSelisih = 0.0;

            foreach (var item in data)
            {
                cellItemRight.Phrase = new Phrase(number.ToString(), _header3Font);
                table.AddCell(cellItemRight);
                number++;

                cellItemCenter.Phrase = new Phrase(item.MemoNo.ToString(), _header3Font);
                table.AddCell(cellItemCenter);

                string memoDate = item.MemoDate.Value.ToString("dd / MM / yy");
                cellItemCenter.Phrase = new Phrase(memoDate, _header3Font);
                table.AddCell(cellItemCenter);

                cellItemCenter.Phrase = new Phrase(item.InternalNoteNo.ToString(), _header3Font);
                table.AddCell(cellItemCenter);

                cellItemCenter.Phrase = new Phrase(item.BillsNo.ToString(), _header3Font);
                table.AddCell(cellItemCenter);

                cellItemLeft.Phrase = new Phrase(item.SupplierCode != null ? item.SupplierCode : "", _header3Font);
                table.AddCell(cellItemLeft);

                cellItemLeft.Phrase = new Phrase(item.SupplierName != null ? item.SupplierName: "", _header3Font);
                table.AddCell(cellItemLeft);

                if (item.CurrencyCode != "IDR")
                {
                    cellItemRight.Phrase = new Phrase(item.MemoAmount.ToString("#,##0.#0"), _header3Font);
                    table.AddCell(cellItemRight);
                } else
                {
                    cellItemRight.Phrase = new Phrase("", _header3Font);
                    table.AddCell(cellItemRight);
                }

                cellItemRight.Phrase = new Phrase(item.MemoIdrAmount.ToString("#,##0.#0"), _header3Font);
                table.AddCell(cellItemRight);

                var buyTotal = item.MemoIdrAmount * item.PurchasingRate;
                cellItemRight.Phrase = new Phrase(buyTotal.ToString("#,##0.#0"), _header3Font);
                table.AddCell(cellItemRight);

                var differencTotal = item.MemoIdrAmount - buyTotal;

                if(differencTotal == 0)
                {
                    cellItemRight.Phrase = new Phrase("", _header3Font);
                    table.AddCell(cellItemRight);
                } else
                {
                    cellItemRight.Phrase = new Phrase(differencTotal.ToString("#,##0.#0"), _header3Font);
                    table.AddCell(cellItemRight);
                }

                totalJmlBayarIDR += item.MemoIdrAmount;
                totalJmlBeli += buyTotal;
                totalSelisih += differencTotal;
            }

            cellColSpan8.Phrase = new Phrase("Total ", _header3BoldFont);
            table.AddCell(cellColSpan8);

            cellItemRight.Phrase = new Phrase(totalJmlBayarIDR.ToString("#,##0.#0"), _header3BoldFont);
            table.AddCell(cellItemRight);

            cellItemRight.Phrase = new Phrase(totalJmlBeli.ToString("#,##0.#0"), _header3BoldFont);
            table.AddCell(cellItemRight);

            if (totalSelisih == 0)
            {
                cellItemRight.Phrase = new Phrase("", _header3Font);
                table.AddCell(cellItemRight);
            }
            else
            {
                cellItemRight.Phrase = new Phrase(totalSelisih.ToString("#,##0.#0"), _header3BoldFont);
                table.AddCell(cellItemRight);
            }

            document.Add(table);
        }
    }
}

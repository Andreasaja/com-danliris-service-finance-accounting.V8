using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditorAccount;
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
    public static class CreditorAccountPDFTemplate
    {
        private static readonly Font _headerFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
        private static readonly Font _normalFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _normalBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);

        public static MemoryStream GeneratePdfTemplate(List<CreditorAccountViewModel> data, string suplierName, int month, int year, int offSet, decimal? finalBalance)
        {
            var document = new Document(PageSize.A4.Rotate(), 25, 25, 25, 25);
            var stream = new MemoryStream();
            PdfWriter.GetInstance(document, stream);
            document.Open();

            SetHeader(document, suplierName, month, year);

            SetReportTable(document, data, suplierName, month, year, offSet, finalBalance);

            SetFooter(document, data, suplierName, month, year, offSet, finalBalance);

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private static void SetHeader(Document document, string suplierName,  int month, int year)
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

            cell.Phrase = new Phrase("Kartu Hutang Lokal", _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase(suplierName, _normalBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Periode: " + new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("dd MMMM yyyy"), _normalBoldFont);
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
            cell.Phrase = new Phrase("TANGGAL", _smallerBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("NAMA BARANG", _smallerBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("NO. BON PENERIMAAN", _smallerBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("NO. INVOICE", _smallerBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("NO. NI/SPB", _smallerBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("NO. VOUCHER", _smallerBoldFont);
            table.AddCell(cell);

            cell.Rowspan = 1;
            cell.Colspan = 3;
            cell.Phrase = new Phrase("MUTASI", _smallerBoldFont);
            table.AddCell(cell);

            cell.Colspan = 1;
            cell.Phrase = new Phrase("PEMBELIAN", _smallerBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("PEMBAYARAN", _smallerBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("SALDO", _smallerBoldFont);
            table.AddCell(cell);

        }

        private static void SetReportTable(Document document, List<CreditorAccountViewModel> data, string suplierName, int month, int year, int offSet, decimal? finalBalance)
        {
            var table = new PdfPTable(9)
            {
                WidthPercentage = 100
            };
            table.SetWidths(new float[] { 10f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f });

            SetReportTableHeader(table);

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

            cell.Colspan = 8;
            cell.Phrase = new Phrase("SALDO AWAL", _smallerFont);
            table.AddCell(cell);

            cellAlignRight.Phrase = new Phrase(finalBalance.GetValueOrDefault().ToString("#,##0.#0"), _smallerFont);
            table.AddCell(cellAlignRight);

            decimal eachBalance = 0;
            decimal tempBalance = finalBalance.GetValueOrDefault();
            foreach (var item in data)
            {
                decimal purchase = 0;
                decimal payment = 0;

                if (item.BankExpenditureNoteNo != null)
                {
                    purchase = item.Mutation;
                    payment = item.Mutation;
                } else
                {
                    purchase = item.Mutation;
                }

                eachBalance = purchase - payment;
                tempBalance += eachBalance;

                cell.Colspan = 1;
                cell.Phrase = new Phrase(item.Date.HasValue ? item.Date.Value.AddHours(offSet).ToString("dd-MMM-yyyy") : null, _smallerFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase(item.Products, _smallerFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase(item.UnitReceiptNoteNo, _smallerFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase(item.InvoiceNo, _smallerFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase(item.MemoNo, _smallerFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase(item.BankExpenditureNoteNo, _smallerFont);
                table.AddCell(cell);

                cellAlignRight.Phrase = new Phrase(purchase.ToString("#,##0.#0"), _smallerFont);
                table.AddCell(cellAlignRight);

                cellAlignRight.Phrase = new Phrase(payment.ToString("#,##0.#0"), _smallerFont);
                table.AddCell(cellAlignRight);

                //cellAlignRight.Phrase = new Phrase(item.BankExpenditureAmount.ToString("#,##0.#0"), _smallerFont);
                //table.AddCell(cellAlignRight);

                cellAlignRight.Phrase = new Phrase(tempBalance.ToString("#,##0.#0"), _smallerFont);
                table.AddCell(cellAlignRight);
            }

            document.Add(table);
        }

        private static void SetFooter(Document document, List<CreditorAccountViewModel> data, string suplierName, int month, int year, int offSet, decimal? finalBalance)
        {
            var table = new PdfPTable(9)
            {
                WidthPercentage = 100
            };
            table.SetWidths(new float[] { 10f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f });

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

            cell.Colspan = 6;
            cell.Phrase = new Phrase("TOTAL", _normalBoldFont);
            table.AddCell(cell);

            decimal totalPurchase = 0;
            decimal totalPayment = 0;
            decimal totalEachBalance = 0;
            foreach (var item in data)
            {
                decimal purchase = 0;
                decimal payment = 0;
                decimal eachBalance = 0;
                if (item.BankExpenditureNoteNo != null)
                {
                    purchase = item.Mutation;
                    payment = item.Mutation;
                }
                else
                {
                    purchase = item.Mutation;
                }
                eachBalance = purchase - payment;

                totalPurchase += purchase;
                totalPayment += payment;
                totalEachBalance += eachBalance;
            }

            totalEachBalance = finalBalance.GetValueOrDefault() + totalEachBalance;

            cellAlignRight.Colspan = 1;
            cellAlignRight.Phrase = new Phrase(totalPurchase.ToString("#,##0.#0"), _normalBoldFont);
            table.AddCell(cellAlignRight);

            cellAlignRight.Phrase = new Phrase(totalPayment.ToString("#,##0.#0"), _normalBoldFont);
            table.AddCell(cellAlignRight);

            //cellAlignRight.Phrase = new Phrase("", _normalBoldFont);
            //table.AddCell(cellAlignRight);

            cellAlignRight.Phrase = new Phrase(totalEachBalance.ToString("#,##0.#0"), _normalBoldFont);
            table.AddCell(cellAlignRight);

            document.Add(table);
        }
    }
}
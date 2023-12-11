using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates
{
    public class MemoGarmentPurchasingReportPdfTemplate
    {
        private static readonly Font _headerFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
        private static readonly Font _normalFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _normalBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);

        public static MemoryStream GeneratePdfTemplate(List<MemoGarmentPurchasingModel> data, string date,string filter)
        {
            var document = new Document(PageSize.A4, 25, 25, 25, 25);
            var stream = new MemoryStream();
            PdfWriter.GetInstance(document, stream);
            document.Open();

            SetHeader(document, date);
            if (!string.IsNullOrEmpty(filter))
            {
                SetFilterheader(document, filter);
            }

            foreach (var model in data)
                SetReportTable(document, model);

            //SetFooter(document, data, offSet);

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private static void SetHeader(Document document, string date)
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

            var cellAlignRight = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            cell.Phrase = new Phrase("PT. DANLIRIS", _headerFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Kel. Banaran, Kec. Grogol, Kab. Sukoharjo 57193", _smallFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Jawa Tengah Indonesia", _smallFont);
            table.AddCell(cell);

            document.Add(table);

            var table2 = new PdfPTable(2)
            {
                WidthPercentage = 100
            };
            table2.SetWidths(new float[] { 10f, 10f });

            cell.Phrase = new Phrase($" PERIODE : {date}", _normalFont);
            table2.AddCell(cell);

            cellAlignRight.Phrase = new Phrase("LAPORAN DATA MEMORIAL    ", _normalFont);
            table2.AddCell(cellAlignRight);
            
            document.Add(table2);


        }

        private static void SetFilterheader(Document document, string filter)
        {

            var table3 = new PdfPTable(2)
            {
                WidthPercentage = 100
            };

            var cell = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            var cellAlignRight = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            ;
            table3.SetWidths(new float[] { 10f, 10f });
            table3.AddCell(new PdfPCell() { Border = Rectangle.NO_BORDER });

            cellAlignRight.Phrase = new Phrase(filter, _normalBoldFont);
            table3.AddCell(cellAlignRight);

            document.Add(table3);

        }

        private static void SetReportTableHeader(PdfPTable table)
        {
            var cell = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            };

            cell.Rowspan = 2;
            cell.Phrase = new Phrase("No.", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("No. Memo", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Tanggal", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Nomor Akun", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Perkiraan", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Keterangan", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Debet", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Kredit", _smallBoldFont);
            table.AddCell(cell);

        }

        private static void SetReportTable(Document document, MemoGarmentPurchasingModel data)
        {
            var table = new PdfPTable(8)
            {
                WidthPercentage = 100
            };
            table.SetWidths(new float[] { 5f, 10f, 10f, 10f, 20f, 10f,10f, 10f });

            SetReportTableHeader(table);

            var cell = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            var cellColspan5 = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Colspan = 5
            };

            var cellAlignRight = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            var cellAlignLeft = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            double totalDebit = 0;
            double totalCredit = 0;
            int index = 1;

            cell.Phrase = new Phrase(index.ToString(), _smallerFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase(index.Equals(1) ? data.MemoNo : "", _smallerFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase(index.Equals(1) ? data.MemoDate.ToString("dd/MM/yy") : "", _smallerFont);
            table.AddCell(cell);

            bool isFirstDetail = false;
            foreach (var detail in data.MemoGarmentPurchasingDetails)
            {
                if (isFirstDetail)
                {
                    cell.Phrase = new Phrase(index.ToString(), _smallerFont);
                    table.AddCell(cell);
                    cell.Phrase = new Phrase();
                    table.AddCell(cell);
                    cell.Phrase = new Phrase();
                    table.AddCell(cell);
                }

                cell.Phrase = new Phrase(detail.COANo, _smallerFont);
                table.AddCell(cell);

                cellAlignLeft.Phrase = new Phrase(detail.COAName, _smallerFont);
                table.AddCell(cellAlignLeft);

                cellAlignLeft.Phrase = new Phrase(detail.MemoGarmentPurchasing.Remarks, _smallerFont);
                table.AddCell(cellAlignLeft);

                cellAlignRight.Phrase = new Phrase(detail.DebitNominal.ToString("#,##0.#0"), _smallerFont);
                table.AddCell(cellAlignRight);

                cellAlignRight.Phrase = new Phrase(detail.CreditNominal.ToString("#,##0.#0"), _smallerFont);
                table.AddCell(cellAlignRight);

                totalDebit += detail.DebitNominal;
                totalCredit += detail.CreditNominal;
                isFirstDetail = true;
                index++;
            }

            cellColspan5.Phrase = new Phrase("Jumlah ", _smallerBoldFont);
            table.AddCell(cellColspan5);

            cellAlignRight.Phrase = new Phrase(totalDebit.ToString("#,##0.#0"), _smallerBoldFont);
            table.AddCell(cellAlignRight);

            cellAlignRight.Phrase = new Phrase(totalCredit.ToString("#,##0.#0"), _smallerBoldFont);
            table.AddCell(cellAlignRight);

            cell.Phrase = new Phrase();
            table.AddCell(cell);

            document.Add(table);
        }

        private static void SetFooter(Document document, MemoGarmentPurchasingModel data, int offSet)
        {
            PdfPTable table = new PdfPTable(3)
            {
                WidthPercentage = 100
            };

            table.SetWidths(new float[] { 1f, 1f, 1f });

            PdfPCell cell = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            };

            PdfPCell cellColspan2 = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Colspan = 2
            };

            cellColspan2.Phrase = new Phrase($"Keterangan : {data.Remarks}", _smallFont);
            table.AddCell(cellColspan2);
            cell.Phrase = new Phrase();
            table.AddCell(cell);

            cellColspan2.Phrase = new Phrase("Nomor Memo Pusat :", _smallFont);
            table.AddCell(cellColspan2);
            cell.Phrase = new Phrase();
            table.AddCell(cell);

            cell.Phrase = new Phrase();
            table.AddCell(cell);
            cell.Phrase = new Phrase();
            table.AddCell(cell);
            cell.Phrase = new Phrase();
            table.AddCell(cell);

            cell.Phrase = new Phrase("Mengetahui", _smallFont);
            table.AddCell(cell);
            cell.Phrase = new Phrase();
            table.AddCell(cell);
            cell.Phrase = new Phrase($"Solo, {DateTime.UtcNow.AddHours(offSet).ToString("dd MMMM yyyy", new CultureInfo("id-ID"))}", _smallFont);
            table.AddCell(cell);
            cell.Phrase = new Phrase("Kepala Pembukuan", _smallFont);
            table.AddCell(cell);
            cell.Phrase = new Phrase();
            table.AddCell(cell);
            cell.Phrase = new Phrase("Yang Membuat", _smallFont);
            table.AddCell(cell);

            for (var i = 0; i < 4; i++)
            {
                cell.Phrase = new Phrase();
                table.AddCell(cell);
                cell.Phrase = new Phrase();
                table.AddCell(cell);
                cell.Phrase = new Phrase();
                table.AddCell(cell);
            }

            cell.Phrase = new Phrase("(..................)", _smallFont);
            table.AddCell(cell);
            cell.Phrase = new Phrase();
            table.AddCell(cell);
            cell.Phrase = new Phrase($"( {data.CreatedBy} )", _smallFont);
            table.AddCell(cell);

            document.Add(table);
        }
    }
}

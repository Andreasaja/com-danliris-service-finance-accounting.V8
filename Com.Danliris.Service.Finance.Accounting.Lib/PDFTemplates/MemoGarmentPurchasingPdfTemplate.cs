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
    public class MemoGarmentPurchasingPdfTemplate
    {
        private static readonly Font _headerFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
        private static readonly Font _normalFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _biggerFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 12);
        private static readonly Font _smallFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _normalBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);

        public static MemoryStream GeneratePdfTemplate(MemoGarmentPurchasingModel data, int offSet)
        {
            var document = new Document(PageSize.A4, 25, 25, 25, 25);
            var stream = new MemoryStream();
            PdfWriter.GetInstance(document, stream);
            document.Open();

            SetHeader(document, data);

            SetReportTable(document, data);

            SetFooter(document, data, offSet);

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private static void SetHeader(Document document, MemoGarmentPurchasingModel data)
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

            var rightCell = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            var centeredCell = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
                
            };

            cell.Phrase = new Phrase("PT. DANLIRIS", _headerFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Kel. Banaran (Sel. Laweyan) Telp. 714400", _smallFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("PO. Box. 166 Solo-57100 Indonesia", _smallFont);
            table.AddCell(cell);

            centeredCell.Phrase = new Phrase("BUKTI MEMORIAL", _biggerFont);
            centeredCell.PaddingTop = 5;
            table.AddCell(centeredCell);

            rightCell.Phrase = new Phrase($"No. Memo: {data.MemoNo}", _smallFont);
            rightCell.PaddingBottom = 10;
            table.AddCell(rightCell);

            document.Add(table);
        }

        private static void SetReportTableHeader(PdfPTable table)
        {
            var cell = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            };

            cell.PaddingTop = 10;
            cell.Rowspan = 2;
            cell.Phrase = new Phrase("No", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("No. Perk", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Perkiraan", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Debet", _smallBoldFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Kredit", _smallBoldFont);
            table.AddCell(cell);

        }

        private static void SetReportTable(Document document, MemoGarmentPurchasingModel data)
        {
            var table = new PdfPTable(5)
            {
                WidthPercentage = 100
            };
            table.SetWidths(new float[] { 5f, 12f, 23f, 15f, 15f });

            SetReportTableHeader(table);

            var cell = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            var cellColspan3 = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Colspan = 3
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
            int no = 1;

            foreach (var detail in data.MemoGarmentPurchasingDetails)
            {
                cell.Phrase = new Phrase(no+"", _smallerFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase(detail.COANo, _smallFont);
                table.AddCell(cell);

                cellAlignLeft.Phrase = new Phrase(detail.COAName, _smallFont);
                table.AddCell(cellAlignLeft);

                cellAlignRight.Phrase = new Phrase(detail.DebitNominal.ToString("#,##0.#0"), _smallFont);
                table.AddCell(cellAlignRight);

                cellAlignRight.Phrase = new Phrase(detail.CreditNominal.ToString("#,##0.#0"), _smallFont);
                table.AddCell(cellAlignRight);

                totalDebit += detail.DebitNominal;
                totalCredit += detail.CreditNominal;
                no++;
            }

            cellColspan3.Phrase = new Phrase("Jumlah Total", _smallBoldFont);
            table.AddCell(cellColspan3);

            cellAlignRight.Phrase = new Phrase(totalDebit.ToString("#,##0.#0"), _smallBoldFont);
            table.AddCell(cellAlignRight);

            cellAlignRight.Phrase = new Phrase(totalCredit.ToString("#,##0.#0"), _smallBoldFont);
            table.AddCell(cellAlignRight);

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
            cellColspan2.PaddingBottom = 10;
            table.AddCell(cellColspan2);
            cell.Phrase = new Phrase();
            table.AddCell(cell);

            //cellColspan2.Phrase = new Phrase("Nomor Memo Pusat :", _smallFont);
            //table.AddCell(cellColspan2);
            //cell.Phrase = new Phrase();
            //table.AddCell(cell);

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

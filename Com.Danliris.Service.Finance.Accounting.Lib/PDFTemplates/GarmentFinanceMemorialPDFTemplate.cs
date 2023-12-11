using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Memorial;
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
    public class GarmentFinanceMemorialPDFTemplate
    {
        public MemoryStream GeneratePdfTemplate(GarmentFinanceMemorialViewModel viewModel, int clientTimeZoneOffset)
        {
            const int MARGIN = 25;

            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 11);
            Font note_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_italic_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLDOBLIQUE, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 12);
            Font Title_bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 13);
            Font underlined_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
            underlined_font.SetStyle(Font.UNDERLINE);

            Document document = new Document(PageSize.A4, MARGIN, MARGIN, MARGIN, MARGIN);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            #region header
            Paragraph company = new Paragraph("PT. DAN LIRIS", Title_bold_font);
            Paragraph address = new Paragraph("Kel. Banaran, Kec. Grogol, Kab.Sukoharjo 57193 \nJawa Tengah Indonesia", note_font);
            document.Add(company);
            document.Add(address);

            PdfPTable headerTable = new PdfPTable(3);
            headerTable.SetWidths(new float[] { 2f,0.5f,3f });
            headerTable.WidthPercentage = 40;
            headerTable.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell cellHeader = new PdfPCell() { Border = Rectangle.NO_BORDER };

            cellHeader.Phrase = new Phrase("No. Bukti", normal_font);
            headerTable.AddCell(cellHeader);
            cellHeader.Phrase = new Phrase(":", normal_font);
            headerTable.AddCell(cellHeader);
            cellHeader.Phrase = new Phrase(viewModel.MemorialNo, normal_font);
            headerTable.AddCell(cellHeader);

            cellHeader.Phrase = new Phrase("Tanggal", normal_font);
            headerTable.AddCell(cellHeader);
            cellHeader.Phrase = new Phrase(":", normal_font);
            headerTable.AddCell(cellHeader);
            cellHeader.Phrase = new Phrase(viewModel.Date.GetValueOrDefault().AddHours(clientTimeZoneOffset).ToString("dd/MM/yy", new CultureInfo("id-ID")), normal_font);
            headerTable.AddCell(cellHeader);

            document.Add(headerTable);

            Paragraph title = new Paragraph("BUKTI MEMORIAL", header_font);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            #endregion

            #region body
            Paragraph Currency = new Paragraph("Kurs : " + viewModel.GarmentCurrency.Code + "      Rate : " + string.Format("{0:n2}", viewModel.GarmentCurrency.Rate), normal_font);
            Currency.Alignment = Element.ALIGN_RIGHT;
            document.Add(Currency);

            PdfPTable tableContent = new PdfPTable(4);
            tableContent.SetWidths(new float[] { 2f, 6f, 2.5f, 2.5f });
            tableContent.WidthPercentage = 100;
            tableContent.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cellCenter = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight=25f };
            PdfPCell cellRight = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = 25f };
            PdfPCell cellLeft = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = 25f };

            cellCenter.Phrase = new Phrase("Acc. No", normal_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Uraian", normal_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Debet", normal_font);
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Kredit", normal_font);
            tableContent.AddCell(cellCenter);

            foreach(var item in viewModel.Items)
            {
                string debit = item.Debit == 0 ? "" : string.Format("{0:n2}", item.Debit);
                string credit = item.Credit == 0 ? "" : string.Format("{0:n2}", item.Credit);
                cellCenter.Phrase = new Phrase(item.COA.Code, note_font);
                tableContent.AddCell(cellCenter);
                cellLeft.Phrase = new Phrase(item.COA.Name, note_font);
                tableContent.AddCell(cellLeft);
                cellRight.Phrase = new Phrase(debit, note_font);
                tableContent.AddCell(cellRight);
                cellRight.Phrase = new Phrase(credit, note_font);
                tableContent.AddCell(cellRight);
            }
        
            cellRight.Phrase = new Phrase("J U M L A H :", note_font);
            cellRight.Colspan = 2;
            tableContent.AddCell(cellRight);
            cellRight.Phrase = new Phrase(string.Format("{0:n2}", viewModel.Items.Sum(item=>item.Debit)), note_font);
            cellRight.Colspan = 1;
            tableContent.AddCell(cellRight);
            cellRight.Phrase = new Phrase(string.Format("{0:n2}", viewModel.Items.Sum(item => item.Credit)), note_font);
            tableContent.AddCell(cellRight);

            tableContent.SpacingBefore = 10f;
            tableContent.SpacingAfter = 20f;
            document.Add(tableContent);
            #endregion

            #region footer
            Paragraph remark = new Paragraph("Keterangan : " + viewModel.Remark, normal_font);
            document.Add(remark);

            PdfPTable footerTable = new PdfPTable(3);
            footerTable.SetWidths(new float[] { 1f ,2f, 1f });
            footerTable.WidthPercentage = 100;
            footerTable.HorizontalAlignment = Element.ALIGN_RIGHT;
            PdfPCell cellFooter = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };

            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("Solo, " + viewModel.Date.GetValueOrDefault().AddHours(clientTimeZoneOffset).ToString("dd MMMM yyyy", new CultureInfo("id-ID")), normal_font);
            footerTable.AddCell(cellFooter);

            cellFooter.Phrase = new Phrase("Kepala Pembukuan", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("Yg. Membuat", normal_font);
            footerTable.AddCell(cellFooter);

            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
          
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
          
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);

            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);

            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);

            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);

            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);

            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);

            cellFooter.Phrase = new Phrase("(                             )", underlined_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            cellFooter.Phrase = new Phrase("(                             )", underlined_font);
            footerTable.AddCell(cellFooter);

            document.Add(footerTable);
            #endregion

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}

using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DownPayment;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates
{
    public class DownPaymentPDFTemplate
    {
        public MemoryStream GeneratePdfTemplate(DownPaymentViewModel viewModel, int clientTimeZoneOffset)
        {
            const int MARGIN = 20;

            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 11);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 11);
            Font note_font = FontFactory.GetFont(BaseFont.HELVETICA_OBLIQUE, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_italic_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLDOBLIQUE, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 12);
            Font Title_bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 13);

            Document document = new Document(PageSize.A5.Rotate(), MARGIN, MARGIN, MARGIN, MARGIN);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            #region CustomModel

            double convertCurrency;


            if (viewModel.Currency.Code == "IDR")
            {
                convertCurrency = (double)viewModel.TotalPayment;
            }
            else
            {
                convertCurrency = (Math.Round((double)viewModel.TotalPayment * (double)viewModel.Currency.Rate));
            }

            string TotalPaidString = NumberToTextIDN.terbilang(convertCurrency);

            #endregion CustomModel

            #region Header

            PdfPTable headerTable_A = new PdfPTable(2);
            PdfPTable headerTable_B = new PdfPTable(1);
            PdfPTable headerTable1 = new PdfPTable(1);
            PdfPTable headerTable2 = new PdfPTable(1);
            PdfPTable headerTable3 = new PdfPTable(3);
            PdfPTable headerTable4 = new PdfPTable(2);
            headerTable_A.SetWidths(new float[] { 10f, 10f });
            headerTable_A.WidthPercentage = 100;
            headerTable3.SetWidths(new float[] { 40f, 4f, 100f });
            headerTable3.WidthPercentage = 100;
            headerTable4.SetWidths(new float[] { 10f, 40f });
            headerTable4.WidthPercentage = 100;

            PdfPCell cellHeader1 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader3 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader4 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody = new PdfPCell() { Border = Rectangle.NO_BORDER };

            cellHeaderBody.Phrase = new Phrase("PT. DAN LIRIS", Title_bold_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Head Office : Jl. Merapi No. 23 Banaran, Grogol", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Sukoharjo, 57552 Central Java, Indonesia", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Telp  :(+62 271) 740888, 714400", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Fax  :(+62 271) 740777, 735222", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("PO BOX 116 Solo, 57100", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Web: www.danliris.com", normal_font);
            headerTable1.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable1.AddCell(cellHeaderBody);

            cellHeader1.AddElement(headerTable1);
            headerTable_A.AddCell(cellHeader1);

            cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;

            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("BUKTI PENERIMAAN", header_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", header_font);
            headerTable2.AddCell(cellHeaderBody);

            cellHeader2.AddElement(headerTable2);
            headerTable_A.AddCell(cellHeader2);

            document.Add(headerTable_A);

            cellHeaderBody.HorizontalAlignment = Element.ALIGN_LEFT;

            cellHeaderBody.Phrase = new Phrase("Bank ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(":", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(viewModel.Bank.BankName + " " + viewModel.Bank.AccountNumber + " (" + viewModel.Bank.Currency.Code + ")", normal_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("Telah terima dari ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(":", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(viewModel.Buyer.Name, normal_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("Banyaknya uang ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(":", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("" + TotalPaidString + " Rupiah", bold_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("Untuk pembayaran ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(":", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(viewModel.Remark + "  ", normal_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("Terbilang", bold_italic_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(":", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Rp. " + convertCurrency.ToString("#,##0.00", new CultureInfo("id-ID")), bold_italic_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeader3.AddElement(headerTable3);
            headerTable_B.AddCell(cellHeader3);

            cellHeader4.AddElement(headerTable4);
            headerTable_B.AddCell(cellHeader4);

            document.Add(headerTable_B);
            #endregion

            #region Footer
            PdfPTable footerTable = new PdfPTable(2);
            PdfPTable footerTable1 = new PdfPTable(1);
            PdfPTable footerTable2 = new PdfPTable(2);

            footerTable.SetWidths(new float[] { 10f, 10f });
            footerTable.WidthPercentage = 100;
            footerTable1.WidthPercentage = 80;
            footerTable2.SetWidths(new float[] { 30f, 50f });
            footerTable2.WidthPercentage = 100;

            PdfPCell cellFooterLeft1 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellFooterLeft2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderFooter = new PdfPCell() { Border = Rectangle.NO_BORDER };


            cellHeaderFooter.HorizontalAlignment = Element.ALIGN_CENTER;

            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);

            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);

            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);

            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);

            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable1.AddCell(cellHeaderFooter);

            footerTable1.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", note_font);
            footerTable1.AddCell(cellHeaderFooter);

            cellFooterLeft1.AddElement(footerTable1);
            footerTable.AddCell(cellFooterLeft1);

            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("Sukoharjo, " + viewModel.DatePayment?.AddHours(clientTimeZoneOffset).ToString("dd MMMM yyyy", new CultureInfo("id-ID")), normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("(                             )", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);

            cellFooterLeft2.AddElement(footerTable2);
            footerTable.AddCell(cellFooterLeft2);

            document.Add(footerTable);

            #endregion Footer

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}

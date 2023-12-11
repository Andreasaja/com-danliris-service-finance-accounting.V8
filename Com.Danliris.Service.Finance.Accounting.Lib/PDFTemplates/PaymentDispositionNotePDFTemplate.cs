using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates
{
    public class PaymentDispositionNotePDFTemplate
    {
        //public MemoryStream GeneratePdfTemplate(PaymentDispositionNoteViewModel viewModel, int clientTimeZoneOffset)
        //{
        //    const int MARGIN = 15;

        //    Font header_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
        //    Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        //    Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

        //    Document document = new Document(PageSize.A4, MARGIN, MARGIN, MARGIN, MARGIN);
        //    MemoryStream stream = new MemoryStream();
        //    PdfWriter writer = PdfWriter.GetInstance(document, stream);
        //    document.Open();

        //    #region Header

        //    PdfPTable headerTable = new PdfPTable(2);
        //    headerTable.SetWidths(new float[] { 10f, 10f });
        //    headerTable.WidthPercentage = 100;
        //    PdfPTable headerTable1 = new PdfPTable(1);
        //    PdfPTable headerTable2 = new PdfPTable(2);
        //    headerTable2.SetWidths(new float[] { 15f, 40f });
        //    headerTable2.WidthPercentage = 100;

        //    PdfPCell cellHeader1 = new PdfPCell() { Border = Rectangle.NO_BORDER };
        //    PdfPCell cellHeader2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
        //    PdfPCell cellHeaderBody = new PdfPCell() { Border = Rectangle.NO_BORDER };

        //    PdfPCell cellHeaderCS2 = new PdfPCell() { Border = Rectangle.NO_BORDER, Colspan = 2 };


        //    cellHeaderCS2.Phrase = new Phrase("BUKTI PENGELUARAN BANK - DISPOSISI", bold_font);
        //    cellHeaderCS2.HorizontalAlignment = Element.ALIGN_CENTER;
        //    headerTable.AddCell(cellHeaderCS2);

        //    cellHeaderCS2.Phrase = new Phrase("", bold_font);
        //    cellHeaderCS2.HorizontalAlignment = Element.ALIGN_CENTER;
        //    headerTable.AddCell(cellHeaderCS2);

        //    cellHeaderBody.Phrase = new Phrase("PT. DANLIRIS", normal_font);
        //    headerTable1.AddCell(cellHeaderBody);
        //    cellHeaderBody.Phrase = new Phrase("Kel. Banaran, Kec. Grogol", normal_font);
        //    headerTable1.AddCell(cellHeaderBody);
        //    cellHeaderBody.Phrase = new Phrase("Sukoharjo - 57100", normal_font);
        //    headerTable1.AddCell(cellHeaderBody);

        //    cellHeader1.AddElement(headerTable1);
        //    headerTable.AddCell(cellHeader1);

        //    cellHeaderCS2.Phrase = new Phrase("", bold_font);
        //    headerTable2.AddCell(cellHeaderCS2);

        //    cellHeaderBody.Phrase = new Phrase("Tanggal", normal_font);
        //    headerTable2.AddCell(cellHeaderBody);
        //    cellHeaderBody.Phrase = new Phrase(": " + viewModel.PaymentDate.AddHours(clientTimeZoneOffset).ToString("dd MMMM yyyy", new CultureInfo("id-ID")), normal_font);
        //    headerTable2.AddCell(cellHeaderBody);

        //    cellHeaderBody.Phrase = new Phrase("NO", normal_font);
        //    headerTable2.AddCell(cellHeaderBody);
        //    cellHeaderBody.Phrase = new Phrase(": " + viewModel.PaymentDispositionNo, normal_font);
        //    headerTable2.AddCell(cellHeaderBody);

        //    //List<string> supplier = model.Details.Select(m => m.SupplierName).Distinct().ToList();
        //    cellHeaderBody.Phrase = new Phrase("Dibayarkan ke", normal_font);
        //    headerTable2.AddCell(cellHeaderBody);
        //    cellHeaderBody.Phrase = new Phrase(": " + viewModel.Supplier.Name, normal_font);
        //    headerTable2.AddCell(cellHeaderBody);


        //    cellHeaderBody.Phrase = new Phrase("Bank", normal_font);
        //    headerTable2.AddCell(cellHeaderBody);
        //    cellHeaderBody.Phrase = new Phrase(": " + viewModel.AccountBank.BankName + " - A/C : " + viewModel.AccountBank.AccountNumber, normal_font);
        //    headerTable2.AddCell(cellHeaderBody);

        //    cellHeader2.AddElement(headerTable2);
        //    headerTable.AddCell(cellHeader2);

        //    cellHeaderCS2.Phrase = new Phrase("", normal_font);
        //    headerTable.AddCell(cellHeaderCS2);

        //    document.Add(headerTable);

        //    #endregion Header

        //    Dictionary<string, double> units = new Dictionary<string, double>();
        //    Dictionary<string, double> percentageUnits = new Dictionary<string, double>();


        //    int index = 1;
        //    double total = 0;
        //    double totalPay = 0;

        //    if (viewModel.AccountBank.Currency.Code != "IDR" || viewModel.CurrencyCode == "IDR")
        //    {
        //        #region BodyNonIDR

        //        PdfPTable bodyNonIDRTable = new PdfPTable(6);
        //        PdfPCell bodyNonIDRCell = new PdfPCell();

        //        float[] widthsBodyNonIDR = new float[] { 5f, 10f, 10f, 10f, 7f, 15f };
        //        bodyNonIDRTable.SetWidths(widthsBodyNonIDR);
        //        bodyNonIDRTable.WidthPercentage = 100;

        //        bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        bodyNonIDRCell.Phrase = new Phrase("No.", bold_font);
        //        bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //        bodyNonIDRCell.Phrase = new Phrase("No. Disposisi", bold_font);
        //        bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //        bodyNonIDRCell.Phrase = new Phrase("Kategori Barang", bold_font);
        //        bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //        bodyNonIDRCell.Phrase = new Phrase("Divisi", bold_font);
        //        bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //        bodyNonIDRCell.Phrase = new Phrase("Mata Uang", bold_font);
        //        bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //        bodyNonIDRCell.Phrase = new Phrase("Jumlah", bold_font);
        //        bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //        foreach (PaymentDispositionNoteItemViewModel item in viewModel.Items)
        //        {
        //            var details = item.Details
        //                .GroupBy(m => new { m.unit.code, m.unit.name })
        //                .Select(s => new
        //                {
        //                    s.First().unit.code,
        //                    s.First().unit.name,
        //                    Total = s.Sum(d => d.price)
        //                });
        //            bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //            bodyNonIDRCell.VerticalAlignment = Element.ALIGN_TOP;
        //            bodyNonIDRCell.Phrase = new Phrase((index++).ToString(), normal_font);
        //            bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //            bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            bodyNonIDRCell.Phrase = new Phrase(item.dispositionNo, normal_font);
        //            bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //            bodyNonIDRCell.Phrase = new Phrase(item.category.name, normal_font);
        //            bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //            bodyNonIDRCell.Phrase = new Phrase(item.division.name, normal_font);
        //            bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //            bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //            bodyNonIDRCell.Phrase = new Phrase(viewModel.AccountBank.Currency.Code, normal_font);
        //            bodyNonIDRTable.AddCell(bodyNonIDRCell);


        //            bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            bodyNonIDRCell.Phrase = new Phrase(string.Format("{0:n4}", item.payToSupplier), normal_font);
        //            bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //            total += item.payToSupplier;

        //            foreach (var detail in details)
        //            {
        //                if (units.ContainsKey(detail.code))
        //                {
        //                    units[detail.code] += detail.Total;
        //                }
        //                else
        //                {
        //                    units.Add(detail.code, detail.Total);
        //                }

        //                totalPay += detail.Total;
        //            }
        //        }

        //        foreach (var un in units)
        //        {
        //            percentageUnits[un.Key] = un.Value * 100 / totalPay;
        //        }

        //        bodyNonIDRCell.Colspan = 3;
        //        bodyNonIDRCell.Border = Rectangle.NO_BORDER;
        //        bodyNonIDRCell.Phrase = new Phrase("", normal_font);
        //        bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //        bodyNonIDRCell.Colspan = 1;
        //        bodyNonIDRCell.Border = Rectangle.BOX;
        //        bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        bodyNonIDRCell.Phrase = new Phrase("Total", bold_font);
        //        bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //        bodyNonIDRCell.Colspan = 1;
        //        bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        bodyNonIDRCell.Phrase = new Phrase(viewModel.AccountBank.Currency.Code, bold_font);
        //        bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //        bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        bodyNonIDRCell.Phrase = new Phrase(string.Format("{0:n4}", total), bold_font);
        //        bodyNonIDRTable.AddCell(bodyNonIDRCell);

        //        document.Add(bodyNonIDRTable);

        //        #endregion BodyNonIDR
        //    }
        //    else
        //    {
        //        #region Body

        //        PdfPTable bodyTable = new PdfPTable(7);
        //        PdfPCell bodyCell = new PdfPCell();

        //        float[] widthsBody = new float[] { 5f, 10f, 10f, 10f, 7f, 10f, 10f };
        //        bodyTable.SetWidths(widthsBody);
        //        bodyTable.WidthPercentage = 100;

        //        bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        bodyCell.Phrase = new Phrase("No.", bold_font);
        //        bodyTable.AddCell(bodyCell);

        //        bodyCell.Phrase = new Phrase("No. Disposisi", bold_font);
        //        bodyTable.AddCell(bodyCell);

        //        bodyCell.Phrase = new Phrase("Kategori Barang", bold_font);
        //        bodyTable.AddCell(bodyCell);

        //        bodyCell.Phrase = new Phrase("Divisi", bold_font);
        //        bodyTable.AddCell(bodyCell);

        //        bodyCell.Phrase = new Phrase("Mata Uang", bold_font);
        //        bodyTable.AddCell(bodyCell);

        //        bodyCell.Phrase = new Phrase("Jumlah", bold_font);
        //        bodyTable.AddCell(bodyCell);

        //        bodyCell.Phrase = new Phrase("Jumlah (IDR)", bold_font);
        //        bodyTable.AddCell(bodyCell);

        //        foreach (PaymentDispositionNoteItemViewModel item in viewModel.Items)
        //        {
        //            var details = item.Details
        //                .GroupBy(m => new { m.unit.code, m.unit.name })
        //                .Select(s => new
        //                {
        //                    s.First().unit.code,
        //                    s.First().unit.name,
        //                    Total = s.Sum(d => d.price)
        //                });
        //            bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //            bodyCell.VerticalAlignment = Element.ALIGN_TOP;
        //            bodyCell.Phrase = new Phrase((index++).ToString(), normal_font);
        //            bodyTable.AddCell(bodyCell);

        //            bodyCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            bodyCell.Phrase = new Phrase(item.dispositionNo, normal_font);
        //            bodyTable.AddCell(bodyCell);

        //            bodyCell.Phrase = new Phrase(item.category.name, normal_font);
        //            bodyTable.AddCell(bodyCell);

        //            bodyCell.Phrase = new Phrase(item.division.name, normal_font);
        //            bodyTable.AddCell(bodyCell);

        //            bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //            bodyCell.Phrase = new Phrase(viewModel.CurrencyCode, normal_font);
        //            bodyTable.AddCell(bodyCell);


        //            bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            bodyCell.Phrase = new Phrase(string.Format("{0:n4}", item.payToSupplier), normal_font);
        //            bodyTable.AddCell(bodyCell);

        //            bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            bodyCell.Phrase = new Phrase(string.Format("{0:n4}", (item.payToSupplier * viewModel.CurrencyRate)), normal_font);
        //            bodyTable.AddCell(bodyCell);

        //            total += item.payToSupplier;

        //            foreach (var detail in details)
        //            {
        //                if (units.ContainsKey(detail.code))
        //                {
        //                    units[detail.code] += detail.Total;
        //                }
        //                else
        //                {
        //                    units.Add(detail.code, detail.Total);
        //                }

        //                totalPay += detail.Total;
        //            }
        //        }

        //        foreach (var un in units)
        //        {
        //            percentageUnits[un.Key] = (un.Value* viewModel.CurrencyRate) * 100 / (totalPay*viewModel.CurrencyRate);
        //        }

        //        bodyCell.Colspan = 3;
        //        bodyCell.Border = Rectangle.NO_BORDER;
        //        bodyCell.Phrase = new Phrase("", normal_font);
        //        bodyTable.AddCell(bodyCell);

        //        bodyCell.Colspan = 1;
        //        bodyCell.Border = Rectangle.BOX;
        //        bodyCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        bodyCell.Phrase = new Phrase("Total", bold_font);
        //        bodyTable.AddCell(bodyCell);

        //        bodyCell.Colspan = 1;
        //        bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        bodyCell.Phrase = new Phrase(viewModel.AccountBank.Currency.Code, bold_font);
        //        bodyTable.AddCell(bodyCell);

        //        bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        bodyCell.Phrase = new Phrase(string.Format("{0:n4}", total), bold_font);
        //        bodyTable.AddCell(bodyCell);

        //        bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        bodyCell.Phrase = new Phrase(string.Format("{0:n4}", total* viewModel.CurrencyRate), bold_font);
        //        bodyTable.AddCell(bodyCell);

        //        document.Add(bodyTable);

        //        #endregion Body
        //    }



        //    #region BodyFooter

        //    PdfPTable bodyFooterTable = new PdfPTable(6);
        //    bodyFooterTable.SetWidths(new float[] { 3f, 6f, 2f, 6f, 10f, 10f });
        //    bodyFooterTable.WidthPercentage = 100;

        //    PdfPCell bodyFooterCell = new PdfPCell() { Border = Rectangle.NO_BORDER };

        //    bodyFooterCell.Colspan = 1;
        //    bodyFooterCell.Phrase = new Phrase("");
        //    bodyFooterTable.AddCell(bodyFooterCell);

        //    bodyFooterCell.Colspan = 1;
        //    bodyFooterCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    bodyFooterCell.Phrase = new Phrase("Rincian per bagian:", normal_font);
        //    bodyFooterTable.AddCell(bodyFooterCell);

        //    bodyFooterCell.Colspan = 4;
        //    bodyFooterCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    bodyFooterCell.Phrase = new Phrase("");
        //    bodyFooterTable.AddCell(bodyFooterCell);

        //    total = viewModel.CurrencyId > 0 ? total * viewModel.CurrencyRate : total;

        //    foreach (var unit in percentageUnits)
        //    {
        //        bodyFooterCell.Colspan = 1;
        //        bodyFooterCell.Phrase = new Phrase("");
        //        bodyFooterTable.AddCell(bodyFooterCell);

        //        bodyFooterCell.Phrase = new Phrase(unit.Key, normal_font);
        //        bodyFooterTable.AddCell(bodyFooterCell);

        //        bodyFooterCell.Phrase = new Phrase(viewModel.AccountBank.Currency.Code, normal_font);
        //        bodyFooterTable.AddCell(bodyFooterCell);

        //        //bodyFooterCell.Phrase = new Phrase(string.Format("{0:n4}", unit.Value), normal_font);
        //        //bodyFooterTable.AddCell(bodyFooterCell);

                

        //        bodyFooterCell.Phrase = new Phrase(string.Format("{0:n4}", unit.Value* total/100), normal_font);
        //        bodyFooterTable.AddCell(bodyFooterCell);


        //        bodyFooterCell.Colspan = 2;
        //        bodyFooterCell.Phrase = new Phrase("");
        //        bodyFooterTable.AddCell(bodyFooterCell);
        //    }

        //    bodyFooterCell.Colspan = 6;
        //    bodyFooterCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    bodyFooterCell.Phrase = new Phrase("");
        //    bodyFooterTable.AddCell(bodyFooterCell);


        //    bodyFooterCell.Colspan = 1;
        //    bodyFooterCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    bodyFooterCell.Phrase = new Phrase("");
        //    bodyFooterTable.AddCell(bodyFooterCell);

        //    bodyFooterCell.Phrase = new Phrase("Terbilang", normal_font);
        //    bodyFooterTable.AddCell(bodyFooterCell);

        //    bodyFooterCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    bodyFooterCell.Phrase = new Phrase(": " + viewModel.AccountBank.Currency.Code, normal_font);
        //    bodyFooterTable.AddCell(bodyFooterCell);

        //    bodyFooterCell.Colspan = 3;
        //    bodyFooterCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    bodyFooterCell.Phrase = new Phrase(NumberToTextIDN.terbilang(total), normal_font);
        //    bodyFooterTable.AddCell(bodyFooterCell);


        //    document.Add(bodyFooterTable);
        //    document.Add(new Paragraph("\n"));

        //    #endregion BodyFooter

        //    #region Footer

        //    PdfPTable footerTable = new PdfPTable(2);
        //    PdfPCell cellFooter = new PdfPCell() { Border = Rectangle.NO_BORDER };

        //    float[] widthsFooter = new float[] { 10f, 5f };
        //    footerTable.SetWidths(widthsFooter);
        //    footerTable.WidthPercentage = 100;

        //    cellFooter.Phrase = new Phrase("Dikeluarkan dengan cek/BG No. : " + viewModel.BGCheckNumber, normal_font);
        //    footerTable.AddCell(cellFooter);

        //    cellFooter.Phrase = new Phrase("", normal_font);
        //    footerTable.AddCell(cellFooter);

        //    PdfPTable signatureTable = new PdfPTable(3);
        //    PdfPCell signatureCell = new PdfPCell() { HorizontalAlignment = Element.ALIGN_CENTER };
        //    signatureCell.Phrase = new Phrase("Bag. Keuangan", normal_font);
        //    signatureTable.AddCell(signatureCell);

        //    signatureCell.Colspan = 2;
        //    signatureCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    signatureCell.Phrase = new Phrase("Direksi", normal_font);
        //    signatureTable.AddCell(signatureCell);

        //    signatureTable.AddCell(new PdfPCell()
        //    {
        //        Phrase = new Phrase("---------------------------", normal_font),
        //        FixedHeight = 40,
        //        VerticalAlignment = Element.ALIGN_BOTTOM,
        //        HorizontalAlignment = Element.ALIGN_CENTER
        //    });
        //    signatureTable.AddCell(new PdfPCell()
        //    {
        //        Phrase = new Phrase("---------------------------", normal_font),
        //        FixedHeight = 40,
        //        Border = Rectangle.NO_BORDER,
        //        VerticalAlignment = Element.ALIGN_BOTTOM,
        //        HorizontalAlignment = Element.ALIGN_CENTER
        //    });
        //    signatureTable.AddCell(new PdfPCell()
        //    {
        //        Phrase = new Phrase("---------------------------", normal_font),
        //        FixedHeight = 40,
        //        Border = Rectangle.NO_BORDER,
        //        VerticalAlignment = Element.ALIGN_BOTTOM,
        //        HorizontalAlignment = Element.ALIGN_CENTER
        //    });

        //    footerTable.AddCell(new PdfPCell(signatureTable));

        //    cellFooter.Phrase = new Phrase("", normal_font);
        //    footerTable.AddCell(cellFooter);
        //    document.Add(footerTable);

        //    #endregion Footer

        //    document.Close();
        //    byte[] byteInfo = stream.ToArray();
        //    stream.Write(byteInfo, 0, byteInfo.Length);
        //    stream.Position = 0;

        //    return stream;
        //}
    }
}

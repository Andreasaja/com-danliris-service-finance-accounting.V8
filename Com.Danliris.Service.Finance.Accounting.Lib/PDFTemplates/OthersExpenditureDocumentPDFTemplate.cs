using Com.Danliris.Service.Finance.Accounting.Lib.Services.OthersExpenditureProofDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates
{
    public class OthersExpenditureDocumentPDFTemplate
    {
        public MemoryStream GeneratePdfTemplate(OthersExpenditureProofDocumentPDFViewModel model, int clientTimeZoneOffset)
        {
            const int MARGIN = 15;

            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

            Document document = new Document(PageSize.A4, MARGIN, MARGIN, MARGIN, MARGIN);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            /*Dictionary<string, double> units = new Dictionary<string, double>();
            model.Items = model.Items.OrderBy(o => o.COAId).ToList();*/

            #region Header

            PdfPTable headerTable = new PdfPTable(2);
            headerTable.SetWidths(new float[] { 10f, 10f });
            headerTable.WidthPercentage = 100;
            PdfPTable headerTable1 = new PdfPTable(1);
            PdfPTable headerTable2 = new PdfPTable(2);
            headerTable2.SetWidths(new float[] { 15f, 40f });
            headerTable2.WidthPercentage = 100;

            PdfPCell cellHeader1 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody = new PdfPCell() { Border = Rectangle.NO_BORDER };

            PdfPCell cellHeaderCS2 = new PdfPCell() { Border = Rectangle.NO_BORDER, Colspan = 2 };
            PdfPCell cellHeaderCS3 = new PdfPCell() { Border = Rectangle.NO_BORDER };


            cellHeaderCS2.Phrase = new Phrase("BUKTI PENGELUARAN BANK (LAIN-LAIN)", bold_font);
            cellHeaderCS2.HorizontalAlignment = Element.ALIGN_CENTER;
            headerTable.AddCell(cellHeaderCS2);

            cellHeaderCS3.Phrase = new Phrase("", bold_font);
            headerTable.AddCell(cellHeaderCS3);
            cellHeaderCS3.Phrase = new Phrase("No. " + model.DocumentNo, bold_font);
            cellHeaderCS3.HorizontalAlignment = Element.ALIGN_CENTER;
            headerTable.AddCell(cellHeaderCS3);

            cellHeaderBody.Phrase = new Phrase("PT. DANLIRIS", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Kel. Banaran, Kec. Grogol", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Sukoharjo - 57100", normal_font);
            headerTable1.AddCell(cellHeaderBody);

            cellHeader1.AddElement(headerTable1);
            headerTable.AddCell(cellHeader1);

            cellHeaderCS2.Phrase = new Phrase("", bold_font);
            headerTable2.AddCell(cellHeaderCS2);

            /*cellHeaderBody.Phrase = new Phrase("No. " + model.DocumentNo, bold_font);
            cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
            headerTable2.AddCell(cellHeaderBody);*/

            /*cellHeaderBody.Colspan = 2;
            cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody.Phrase = new Phrase("No. " + model.DocumentNo, bold_font);
            headerTable2.AddCell(cellHeaderBody);

            cellHeaderBody.Colspan = 1;
            cellHeaderBody.HorizontalAlignment = Element.ALIGN_LEFT;*/
            cellHeaderBody.Phrase = new Phrase("Tanggal", normal_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(": " + model.Date.AddHours(clientTimeZoneOffset).ToString("dd MMMM yyyy"), normal_font);
            headerTable2.AddCell(cellHeaderBody);

            /*List<string> supplier = model.Details.Select(m => m.SupplierName).Distinct().ToList();
            cellHeaderBody.Phrase = new Phrase("Dibayarkan ke", normal_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(": " + (supplier.Count > 0 ? supplier[0] : "-"), normal_font);
            headerTable2.AddCell(cellHeaderBody);

            for (int i = 1; i < supplier.Count; i++)
            {
                cellHeaderBody.Phrase = new Phrase("", normal_font);
                headerTable2.AddCell(cellHeaderBody);
                cellHeaderBody.Phrase = new Phrase(": " + supplier[i], normal_font);
                headerTable2.AddCell(cellHeaderBody);
            }*/

            cellHeaderBody.Phrase = new Phrase("Bank", normal_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(": " + model.Bank.BankName + " - A/C : " + model.Bank.AccountNumber, normal_font);
            headerTable2.AddCell(cellHeaderBody);

            cellHeader2.AddElement(headerTable2);
            headerTable.AddCell(cellHeader2);

            cellHeaderCS2.Phrase = new Phrase("", normal_font);
            headerTable.AddCell(cellHeaderCS2);

            document.Add(headerTable);

            #endregion Header
            int index = 1;
            decimal total = 0;
            if (model.Bank.Currency.Code != "IDR")
            {
                #region BodyNonIdr

                PdfPTable bodyTable = new PdfPTable(6);
                PdfPCell bodyCell = new PdfPCell();

                float[] widthsBody = new float[] { 5f, 10f, 10f, 10f, 7f, 15f };
                bodyTable.SetWidths(widthsBody);
                bodyTable.WidthPercentage = 100;

                bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                bodyCell.Phrase = new Phrase("No.", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("COA", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Nama COA", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Keterangan", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Mata Uang", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Jumlah", bold_font);
                bodyTable.AddCell(bodyCell);

                
                /*foreach (item in model.Items)
                {*/
                    /*
                    var items = detail.Items
                        .GroupBy(m => new { m.UnitCode, m.UnitName })
                        .Select(s => new
                        {
                            s.First().UnitCode,
                            s.First().UnitName,
                            Total = s.Sum(d => detail.Vat == 0 ? d.Price : d.Price * 1.1)
                        });
                    */
                    
                    foreach (var item in model.Items)
                    {

                        bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        /*bodyCell.VerticalAlignment = Element.ALIGN_TOP;*/
                        bodyCell.Phrase = new Phrase((index++).ToString(), normal_font);
                        bodyTable.AddCell(bodyCell);

                        bodyCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        bodyCell.Phrase = new Phrase(item.COA.Code, normal_font);
                        bodyTable.AddCell(bodyCell);

                        bodyCell.Phrase = new Phrase(item.COA.Name, normal_font);
                        bodyTable.AddCell(bodyCell);

                        bodyCell.Phrase = new Phrase(item.Remark, normal_font);
                        bodyTable.AddCell(bodyCell);
                    
                        bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        bodyCell.Phrase = new Phrase(model.Bank.Currency.Code, normal_font);
                        bodyTable.AddCell(bodyCell);

                        bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        bodyCell.Phrase = new Phrase(string.Format("{0:n4}", item.Debit), normal_font);
                        bodyTable.AddCell(bodyCell);

                    /*if (units.ContainsKey(item.UnitCode))
                    {
                        units[item.UnitCode] += item.Total;
                    }
                    else
                    {
                        units.Add(item.UnitCode, item.Total);
                    }*/

                    total += item.Debit;
                    }
                /*}*/
                
                bodyCell.Colspan = 3;
                bodyCell.Border = Rectangle.NO_BORDER;
                bodyCell.Phrase = new Phrase("", normal_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Colspan = 1;
                bodyCell.Border = Rectangle.BOX;
                bodyCell.HorizontalAlignment = Element.ALIGN_LEFT;
                bodyCell.Phrase = new Phrase("Total Debit", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Colspan = 1;
                bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                bodyCell.Phrase = new Phrase(model.Bank.Currency.Code, bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                bodyCell.Phrase = new Phrase(string.Format("{0:n4}", total), bold_font);
                bodyTable.AddCell(bodyCell);

                document.Add(bodyTable);

                #endregion BodyNonIdr
            }
            else
            {
                #region BodyIdr
                PdfPTable bodyTable = new PdfPTable(6);
                PdfPCell bodyCell = new PdfPCell();

                float[] widthsBody = new float[] { 5f, 10f, 10f, 10f, 7f, 15f };
                bodyTable.SetWidths(widthsBody);
                bodyTable.WidthPercentage = 100;

                bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                bodyCell.Phrase = new Phrase("No.", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("COA", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Nama COA", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Keterangan", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Mata Uang", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Jumlah", bold_font);
                bodyTable.AddCell(bodyCell);
                /*
                foreach (BankExpenditureNoteDetailModel detail in model.Details)
                {
                    var items = detail.Items
                        .GroupBy(m => new { m.UnitCode, m.UnitName })
                        .Select(s => new
                        {
                            s.First().UnitCode,
                            s.First().UnitName,
                            Total = s.Sum(d => detail.Vat == 0 ? d.Price : d.Price * 1.1)
                        });*/
                foreach (var item in model.Items)
                    {
                        bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        bodyCell.VerticalAlignment = Element.ALIGN_TOP;
                        bodyCell.Phrase = new Phrase((index++).ToString(), normal_font);
                        bodyTable.AddCell(bodyCell);

                        bodyCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        bodyCell.Phrase = new Phrase(item.COA.Code, normal_font);
                        bodyTable.AddCell(bodyCell);

                        bodyCell.Phrase = new Phrase(item.COA.Name, normal_font);
                        bodyTable.AddCell(bodyCell);

                        bodyCell.Phrase = new Phrase(item.Remark, normal_font);
                        bodyTable.AddCell(bodyCell);
                    
                        bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        bodyCell.Phrase = new Phrase(model.Bank.Currency.Code, normal_font);
                        bodyTable.AddCell(bodyCell);

                        bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        bodyCell.Phrase = new Phrase(string.Format("{0:n4}", item.Debit), normal_font);
                        bodyTable.AddCell(bodyCell);

                        /*
                        if (units.ContainsKey(item.UnitCode))
                        {
                            units[item.UnitCode] += (item.Total * model.CurrencyRate);
                        }
                        else
                        {
                            units.Add(item.UnitCode, (item.Total * model.CurrencyRate));
                        }*/

                        total += item.Debit;
                    }
                /*}*/

                bodyCell.Colspan = 3;
                bodyCell.Border = Rectangle.NO_BORDER;
                bodyCell.Phrase = new Phrase("", normal_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Colspan = 1;
                bodyCell.Border = Rectangle.BOX;
                bodyCell.HorizontalAlignment = Element.ALIGN_LEFT;
                bodyCell.Phrase = new Phrase("Total Debit", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Colspan = 1;
                bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                bodyCell.Phrase = new Phrase(model.Bank.Currency.Code, bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                bodyCell.Phrase = new Phrase(string.Format("{0:n4}", total), bold_font);
                bodyTable.AddCell(bodyCell);

                document.Add(bodyTable);

                #endregion BodyIdr
            }



            #region BodyFooter

            PdfPTable bodyFooterTable = new PdfPTable(6);
            bodyFooterTable.SetWidths(new float[] { 3f, 6f, 2f, 6f, 10f, 10f });
            bodyFooterTable.WidthPercentage = 100;

            PdfPCell bodyFooterCell = new PdfPCell() { Border = Rectangle.NO_BORDER };

            /*bodyFooterCell.Colspan = 1;
            bodyFooterCell.Phrase = new Phrase("");
            bodyFooterTable.AddCell(bodyFooterCell);

            bodyFooterCell.Colspan = 1;
            bodyFooterCell.HorizontalAlignment = Element.ALIGN_LEFT;
            bodyFooterCell.Phrase = new Phrase("Rincian per bagian:", normal_font);
            bodyFooterTable.AddCell(bodyFooterCell);

            bodyFooterCell.Colspan = 4;
            bodyFooterCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            bodyFooterCell.Phrase = new Phrase("");
            bodyFooterTable.AddCell(bodyFooterCell);
            total = model.Bank.Currency.Id > 0 ? total * model.Bank.Currency.Rate : total;
            */
            /*
            foreach (var unit in units)
            {
                bodyFooterCell.Colspan = 1;
                bodyFooterCell.Phrase = new Phrase("");
                bodyFooterTable.AddCell(bodyFooterCell);

                bodyFooterCell.Phrase = new Phrase(unit.Key, normal_font);
                bodyFooterTable.AddCell(bodyFooterCell);

                bodyFooterCell.Phrase = new Phrase(model.BankCurrencyCode, normal_font);
                bodyFooterTable.AddCell(bodyFooterCell);

                bodyFooterCell.Phrase = new Phrase(string.Format("{0:n4}", unit.Value), normal_font);
                bodyFooterTable.AddCell(bodyFooterCell);

                bodyFooterCell.Colspan = 2;
                bodyFooterCell.Phrase = new Phrase("");
                bodyFooterTable.AddCell(bodyFooterCell);
            }
            */

            bodyFooterCell.Colspan = 1;
            bodyFooterCell.HorizontalAlignment = Element.ALIGN_LEFT;
            bodyFooterCell.Phrase = new Phrase("");
            bodyFooterTable.AddCell(bodyFooterCell);

            bodyFooterCell.Phrase = new Phrase("Terbilang", normal_font);
            bodyFooterTable.AddCell(bodyFooterCell);

            bodyFooterCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            bodyFooterCell.Phrase = new Phrase(": " + model.Bank.Currency.Code, normal_font);
            bodyFooterTable.AddCell(bodyFooterCell);

            bodyFooterCell.Colspan = 3;
            bodyFooterCell.HorizontalAlignment = Element.ALIGN_LEFT;
            bodyFooterCell.Phrase = new Phrase(NumberToTextIDN.terbilang(Convert.ToDouble(total)), normal_font);
            bodyFooterTable.AddCell(bodyFooterCell);


            document.Add(bodyFooterTable);
            document.Add(new Paragraph("\n"));

            #endregion BodyFooter

            #region Footer

            PdfPTable footerTable = new PdfPTable(2);
            PdfPCell cellFooter = new PdfPCell() { Border = Rectangle.NO_BORDER };

            float[] widthsFooter = new float[] { 10f, 5f };
            footerTable.SetWidths(widthsFooter);
            footerTable.WidthPercentage = 100;

            cellFooter.Phrase = new Phrase("Dikeluarkan dengan cek/BG No. : " + model.CekBgNo, normal_font);
            footerTable.AddCell(cellFooter);

            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);

            PdfPTable signatureTable = new PdfPTable(3);
            PdfPCell signatureCell = new PdfPCell() { HorizontalAlignment = Element.ALIGN_CENTER };
            signatureCell.Phrase = new Phrase("Bag. Keuangan", normal_font);
            signatureTable.AddCell(signatureCell);

            signatureCell.Colspan = 2;
            signatureCell.HorizontalAlignment = Element.ALIGN_CENTER;
            signatureCell.Phrase = new Phrase("Direksi", normal_font);
            signatureTable.AddCell(signatureCell);

            signatureTable.AddCell(new PdfPCell()
            {
                Phrase = new Phrase("---------------------------", normal_font),
                FixedHeight = 40,
                VerticalAlignment = Element.ALIGN_BOTTOM,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            signatureTable.AddCell(new PdfPCell()
            {
                Phrase = new Phrase("---------------------------", normal_font),
                FixedHeight = 40,
                Border = Rectangle.NO_BORDER,
                VerticalAlignment = Element.ALIGN_BOTTOM,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            signatureTable.AddCell(new PdfPCell()
            {
                Phrase = new Phrase("---------------------------", normal_font),
                FixedHeight = 40,
                Border = Rectangle.NO_BORDER,
                VerticalAlignment = Element.ALIGN_BOTTOM,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            footerTable.AddCell(new PdfPCell(signatureTable));

            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
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

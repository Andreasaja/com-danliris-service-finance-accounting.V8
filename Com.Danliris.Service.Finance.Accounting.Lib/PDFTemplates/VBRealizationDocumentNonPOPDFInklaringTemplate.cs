using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO;
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
    public class VBRealizationDocumentNonPOPDFInklaringTemplate
    {
        public MemoryStream GeneratePdfTemplate(VBRealizationDocumentNonPOViewModel viewModel, int timeoffsset)
        {
            const int MARGIN = 20;
            const int MARGIN_VERTICAL = 10;

            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 11);
            Font normal_font_8 = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 11);
            Font bold_font_8 = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font note_font = FontFactory.GetFont(BaseFont.HELVETICA_OBLIQUE, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_italic_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLDOBLIQUE, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 12);
            Font Title_bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 13);

            Document document = new Document(PageSize.A4, MARGIN_VERTICAL, MARGIN_VERTICAL, MARGIN, MARGIN);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            #region Header
            PdfPTable headerTable_A = new PdfPTable(2);
            PdfPTable headerTable_B = new PdfPTable(1);
            PdfPTable headerTable_C = new PdfPTable(1);
            PdfPTable headerTable1 = new PdfPTable(1);
            PdfPTable headerTable2 = new PdfPTable(1);
            PdfPTable headerTable3 = new PdfPTable(7);
            PdfPTable headerTable3a = new PdfPTable(7);
            PdfPTable headerTable3b = new PdfPTable(5);

            PdfPTable headerTable4 = new PdfPTable(2);

            headerTable_A.SetWidths(new float[] { 10f, 10f });
            headerTable_A.WidthPercentage = 100;
            headerTable3.SetWidths(new float[] { 5f, 15f, 15f, 15f, 20f, 15f, 20f });
            headerTable3.WidthPercentage = 110;
            headerTable3a.SetWidths(new float[] { 3f, 15f, 5f, 15f, 15f, 15f, 16f });
            headerTable3a.WidthPercentage = 110;
            headerTable3b.SetWidths(new float[] { 3f, 15f, 5f, 15f, 62f });
            headerTable3b.WidthPercentage = 110;
            headerTable4.SetWidths(new float[] { 10f, 40f });
            headerTable4.WidthPercentage = 100;

            PdfPCell cellHeader1 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader3 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader4 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader3a = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody1 = new PdfPCell() { };
            PdfPCell cellHeaderBody1a = new PdfPCell() { BorderWidthTop = 2 };
            PdfPCell cellHeaderBody1a1 = new PdfPCell() { };
            PdfPCell cellHeaderBody1a2 = new PdfPCell() { };
            PdfPCell cellHeaderBody1b = new PdfPCell() { BorderWidthTop = 2 };
            PdfPCell cellHeaderBody1b1 = new PdfPCell() { };
            PdfPCell cellHeaderBody1b2 = new PdfPCell() { };
            PdfPCell cellHeaderBody1c = new PdfPCell() { };
            PdfPCell cellHeaderBody2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody3 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody4 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody4a = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody4b = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody5 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody5a = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody6 = new PdfPCell() { };

            cellHeader1.AddElement(headerTable1);
            headerTable_A.AddCell(cellHeader1);
            cellHeader2.AddElement(headerTable2);
            headerTable_A.AddCell(cellHeader2);
            document.Add(headerTable_A);

            cellHeaderBody.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody1.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody1a.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellHeaderBody1a1.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellHeaderBody1a2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellHeaderBody1b.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody1b1.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody1b2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody1c.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody3.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellHeaderBody4.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody4a.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody4b.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellHeaderBody5a.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody6.HorizontalAlignment = Element.ALIGN_LEFT;

            // Document title
            cellHeaderBody2.Colspan = 7;
            cellHeaderBody2.Phrase = new Phrase("REALISASI VB INKLARING TANPA PO", bold_font);
            headerTable3.AddCell(cellHeaderBody2);

            // Document number
            cellHeaderBody3.Colspan = 7;
            cellHeaderBody3.Phrase = new Phrase($"{viewModel.DocumentNo}", bold_font);
            headerTable3.AddCell(cellHeaderBody3);

            // Realisasi VB Bagian
            cellHeaderBody3.Colspan = 7;
            cellHeaderBody3.HorizontalAlignment = Element.ALIGN_LEFT; // Override default to right
            cellHeaderBody3.Phrase = new Phrase($"Realisasi VB Bagian: {viewModel.Unit.Name}", bold_font);
            headerTable3.AddCell(cellHeaderBody3);

            // Tanggal
            cellHeaderBody3.Colspan = 7;
            cellHeaderBody3.HorizontalAlignment = Element.ALIGN_LEFT; // Override default to right
            cellHeaderBody3.Phrase = new Phrase($"Tanggal: {viewModel.Date?.AddHours(timeoffsset).ToString("dd MMMM yyyy", new CultureInfo("id-ID"))}", bold_font);
            headerTable3.AddCell(cellHeaderBody3);

            // New line
            cellHeaderBody3.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody3);

            // Table header
            cellHeaderBody1.Phrase = new Phrase("No", normal_font);
            headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody1.Phrase = new Phrase("Tanggal", normal_font);
            headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody1.Colspan = 2;
            cellHeaderBody1.Phrase = new Phrase("Keterangan", normal_font);
            headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody1.Colspan = 1;
            cellHeaderBody1.Phrase = new Phrase("No. BL / AWB", normal_font);
            headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody1.Phrase = new Phrase("Mata Uang", normal_font);
            headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody1.Phrase = new Phrase("Jumlah", normal_font);
            headerTable3.AddCell(cellHeaderBody1);

            int index = 1;
            /*decimal count_price = 0;*/
            /*decimal total_all = 0;*/
            decimal total_realization = 0;

            decimal ppn_manually = 0;
            decimal pph_supplier = 0;
            decimal pph_danliris = 0;

            var currencyCode = viewModel.Currency.Code;
            var currencydescription = viewModel.Currency.Description;

            foreach (var itm in viewModel.Items)
            {
                // No
                cellHeaderBody1.Phrase = new Phrase(index.ToString(), normal_font);
                cellHeaderBody1.HorizontalAlignment = Element.ALIGN_CENTER;
                headerTable3.AddCell(cellHeaderBody1);
                index++;

                // Tanggal
                cellHeaderBody1.Phrase = new Phrase(itm.DateDetail?.AddHours(timeoffsset).ToString("dd/MM/yyyy"), normal_font);
                headerTable3.AddCell(cellHeaderBody1);

                // Keterangan
                cellHeaderBody1.Colspan = 2;
                cellHeaderBody1.Phrase = new Phrase(itm.Remark, normal_font);
                cellHeaderBody1.HorizontalAlignment = Element.ALIGN_LEFT;
                headerTable3.AddCell(cellHeaderBody1);

                // No. BL / AWB
                cellHeaderBody1.Colspan = 1;
                cellHeaderBody1.Phrase = new Phrase(itm.BLAWBNumber, normal_font);
                cellHeaderBody1.HorizontalAlignment = Element.ALIGN_LEFT;
                headerTable3.AddCell(cellHeaderBody1);

                if (itm.IsGetPPn)
                {
                    /*var temp = itm.Amount * 0.1m;
                    total_all = itm.Amount + temp;*/

                    ppn_manually += itm.PPnAmount;
                }
                /*else
                {
                    total_all = itm.Amount;
                }*/

                if (itm.IsGetPPh)
                {
                    if (itm.IncomeTaxBy == "Supplier")
                    {
                        pph_supplier += itm.PPhAmount;
                    }
                    else
                    {
                        pph_danliris += itm.PPhAmount;
                    }
                }

                // Mata Uang
                cellHeaderBody1.Colspan = 1;
                cellHeaderBody1.Phrase = new Phrase(currencyCode, normal_font);
                cellHeaderBody1.HorizontalAlignment = Element.ALIGN_CENTER;
                headerTable3.AddCell(cellHeaderBody1);

                // Jumlah
                cellHeaderBody1.Phrase = new Phrase(itm.Amount.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
                cellHeaderBody1.HorizontalAlignment = Element.ALIGN_RIGHT; // Override default to center
                headerTable3.AddCell(cellHeaderBody1);

                /*count_price += total_all;*/
                total_realization += itm.Amount;
            }

            // Jumlah Realisasi
            cellHeaderBody1a.Colspan = 5;
            cellHeaderBody1a.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody1a.Phrase = new Phrase("Jumlah Realisasi", normal_font);
            headerTable3.AddCell(cellHeaderBody1a);

            // Mata Uang
            cellHeaderBody1b.Phrase = new Phrase(currencyCode, normal_font);
            headerTable3.AddCell(cellHeaderBody1b);

            // Jumlah
            cellHeaderBody1a.Phrase = new Phrase(total_realization.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            cellHeaderBody1a.HorizontalAlignment = Element.ALIGN_RIGHT;
            headerTable3.AddCell(cellHeaderBody1a);

            // PPn
            cellHeaderBody1a1.Colspan = 5;
            cellHeaderBody1a1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody1a1.Phrase = new Phrase("PPN", normal_font);
            headerTable3.AddCell(cellHeaderBody1a1);

            // Mata Uang
            cellHeaderBody1b1.Phrase = new Phrase(currencyCode, normal_font);
            headerTable3.AddCell(cellHeaderBody1b1);

            // Jumlah
            cellHeaderBody1a1.Phrase = new Phrase(ppn_manually.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            cellHeaderBody1a1.HorizontalAlignment = Element.ALIGN_RIGHT;
            headerTable3.AddCell(cellHeaderBody1a1);

            // PPh ditanggung Dan Liris
            cellHeaderBody1a1.Colspan = 5;
            cellHeaderBody1a1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody1a1.Phrase = new Phrase("PPh ditanggung Dan Liris", normal_font);
            headerTable3.AddCell(cellHeaderBody1a1);

            // Mata Uang
            cellHeaderBody1b1.Phrase = new Phrase(currencyCode, normal_font);
            headerTable3.AddCell(cellHeaderBody1b1);

            // Jumlah
            cellHeaderBody1a1.Phrase = new Phrase(pph_danliris.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            cellHeaderBody1a1.HorizontalAlignment = Element.ALIGN_RIGHT;
            headerTable3.AddCell(cellHeaderBody1a1);

            // PPh ditanggung Supplier
            cellHeaderBody1a1.Colspan = 5;
            cellHeaderBody1a1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody1a1.Phrase = new Phrase("PPh ditanggung Supplier", normal_font);
            headerTable3.AddCell(cellHeaderBody1a1);

            // Mata Uang
            cellHeaderBody1b1.Phrase = new Phrase(currencyCode, normal_font);
            headerTable3.AddCell(cellHeaderBody1b1);

            // Jumlah
            cellHeaderBody1a1.Phrase = new Phrase(pph_supplier.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            cellHeaderBody1a1.HorizontalAlignment = Element.ALIGN_RIGHT;
            headerTable3.AddCell(cellHeaderBody1a1);

            // Total Keseluruhan
            cellHeaderBody1a2.Colspan = 5;
            cellHeaderBody1a2.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody1a2.Phrase = new Phrase("Total", normal_font);
            headerTable3.AddCell(cellHeaderBody1a2);

            // Mata Uang
            cellHeaderBody1b2.Phrase = new Phrase(currencyCode, normal_font);
            headerTable3.AddCell(cellHeaderBody1b2);

            // Jumlah
            var grandTotal = total_realization + ppn_manually - pph_supplier;
            cellHeaderBody1a2.Phrase = new Phrase(grandTotal.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            cellHeaderBody1a2.HorizontalAlignment = Element.ALIGN_RIGHT;
            headerTable3.AddCell(cellHeaderBody1a2);

            var vbDate = viewModel.VBDocument?.Date?.AddHours(timeoffsset).ToString("dd-MMMM-yy", new CultureInfo("id-ID"));
            if (viewModel.VBNonPOType == "Tanpa Nomor VB")
                if (viewModel.VBDocument != null && !string.IsNullOrWhiteSpace(viewModel.VBDocument?.DocumentNo))
                    vbDate = viewModel.VBDocument?.Date?.AddHours(timeoffsset).ToString("dd-MMMM-yy", new CultureInfo("id-ID"));
                else
                    vbDate = "";

            // Tanggal VB
            cellHeaderBody6.Colspan = 3;
            cellHeaderBody6.Phrase = new Phrase($"Tanggal VB: {vbDate}", normal_font);
            headerTable3.AddCell(cellHeaderBody6);

            // No VB
            cellHeaderBody1.Colspan = 2;
            cellHeaderBody1.Phrase = new Phrase($"No.VB: {viewModel.VBDocument?.DocumentNo}", normal_font);
            cellHeaderBody1.HorizontalAlignment = Element.ALIGN_LEFT;
            headerTable3.AddCell(cellHeaderBody1);

            if (viewModel.VBDocument == null)
            {
                // Mata Uang
                cellHeaderBody1.Colspan = 1;
                cellHeaderBody1.Phrase = new Phrase(currencyCode, normal_font);
                cellHeaderBody1.HorizontalAlignment = Element.ALIGN_CENTER;
                headerTable3.AddCell(cellHeaderBody1);

                // Jumlah
                cellHeaderBody1.Phrase = new Phrase(0.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
                cellHeaderBody1.HorizontalAlignment = Element.ALIGN_RIGHT; // Override default to center
                headerTable3.AddCell(cellHeaderBody1);
            }
            else
            {
                // Mata Uang
                cellHeaderBody1.Colspan = 1;
                cellHeaderBody1.Phrase = new Phrase(currencyCode, normal_font);
                cellHeaderBody1.HorizontalAlignment = Element.ALIGN_CENTER;
                headerTable3.AddCell(cellHeaderBody1);

                // Jumlah
                cellHeaderBody1.Phrase = new Phrase($"{viewModel.VBDocument?.Amount.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font);
                cellHeaderBody1.HorizontalAlignment = Element.ALIGN_RIGHT; // Override default to center
                headerTable3.AddCell(cellHeaderBody1);
            }

            var priceterbilang = grandTotal;
            var res = grandTotal - (viewModel.VBDocument == null ? 0 : viewModel.VBDocument.Amount.GetValueOrDefault());

            if (res > 0)
            {
                // Kurang
                cellHeaderBody5.Colspan = 5;
                cellHeaderBody5.Phrase = new Phrase("Kurang", bold_font);
                headerTable3.AddCell(cellHeaderBody5);

                // Mata Uang
                cellHeaderBody5a.Phrase = new Phrase(currencyCode, normal_font);
                headerTable3.AddCell(cellHeaderBody5a);

                // Jumlah
                cellHeaderBody5a.Phrase = new Phrase("(" + res.ToString("#,##0.00", new CultureInfo("id-ID")) + ")", normal_font);
                cellHeaderBody5a.HorizontalAlignment = Element.ALIGN_RIGHT; // Override default to center
                headerTable3.AddCell(cellHeaderBody5a);
            }
            else
            {
                // Sisa
                cellHeaderBody5.Colspan = 5;
                cellHeaderBody5.Phrase = new Phrase("Sisa", bold_font);
                headerTable3.AddCell(cellHeaderBody5);

                // Mata Uang
                cellHeaderBody5a.Phrase = new Phrase(currencyCode, normal_font);
                headerTable3.AddCell(cellHeaderBody5a);

                // Jumlah
                cellHeaderBody5a.Phrase = new Phrase($"{(res * -1).ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font);
                cellHeaderBody5a.HorizontalAlignment = Element.ALIGN_RIGHT; // Override default to center
                headerTable3.AddCell(cellHeaderBody5a);
            }

            string total = grandTotal.ToString("#,##0.00", new CultureInfo("id-ID"));

            // New Line
            cellHeaderBody4a.Colspan = 7;
            cellHeaderBody4a.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4a);

            // Terbilang
            cellHeaderBody4a.Colspan = 7;
            cellHeaderBody4a.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody4a.Phrase = new Phrase("Terbilang: " + Nom(grandTotal, currencyCode, currencydescription), normal_font);
            headerTable3.AddCell(cellHeaderBody4a);

            // New Line
            cellHeaderBody4a.Colspan = 7;
            cellHeaderBody4a.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4a);

            cellHeader3.AddElement(headerTable3);
            headerTable_B.AddCell(cellHeader3);
            cellHeader4.AddElement(headerTable4);
            headerTable_B.AddCell(cellHeader4);
            document.Add(headerTable_B);

            // Beban Unit
            cellHeaderBody.Colspan = 4;
            cellHeaderBody.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody.Phrase = new Phrase("Beban Unit:", bold_font);
            headerTable3a.AddCell(cellHeaderBody);

            if (pph_supplier != 0 || pph_danliris != 0)
            {
                // Header PPH23
                cellHeaderBody.Colspan = 1;
                cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
                cellHeaderBody.Phrase = new Phrase("PPH 23", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);

                if (ppn_manually != 0)
                {
                    // Header PPN
                    cellHeaderBody.Colspan = 1;
                    cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellHeaderBody.Phrase = new Phrase("PPN", normal_font_8);
                    headerTable3a.AddCell(cellHeaderBody);

                    // Empty space
                    cellHeaderBody.Colspan = 1;
                    cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellHeaderBody.Phrase = new Phrase(" ", normal_font_8);
                    headerTable3a.AddCell(cellHeaderBody);
                }
                else
                {
                    // Empty space
                    cellHeaderBody.Colspan = 2;
                    cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellHeaderBody.Phrase = new Phrase(" ", normal_font_8);
                    headerTable3a.AddCell(cellHeaderBody);
                }
            }
            else
            {
                if (ppn_manually != 0)
                {
                    // Header PPN
                    cellHeaderBody.Colspan = 1;
                    cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellHeaderBody.Phrase = new Phrase("PPN", normal_font_8);
                    headerTable3a.AddCell(cellHeaderBody);

                    // Empty space
                    cellHeaderBody.Colspan = 2;
                    cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellHeaderBody.Phrase = new Phrase(" ", normal_font_8);
                    headerTable3a.AddCell(cellHeaderBody);
                }
                else
                {
                    // Empty space
                    cellHeaderBody.Colspan = 3;
                    cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellHeaderBody.Phrase = new Phrase(" ", normal_font_8);
                    headerTable3a.AddCell(cellHeaderBody);
                }
            }

            // New Line
            cellHeaderBody.Colspan = 7;
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3a.AddCell(cellHeaderBody);
            #endregion Header

            #region NewCheckbox
            var layoutOrderOther = viewModel.UnitCosts.ToList().FirstOrDefault(s => s.Unit.VBDocumentLayoutOrder == 10);
            var unitCost12 = viewModel.UnitCosts.ToList().FirstOrDefault(s => s.Unit.VBDocumentLayoutOrder == 12);

            if (unitCost12 != null)
                unitCost12.Unit.VBDocumentLayoutOrder = 10;

            if (layoutOrderOther != null)
                layoutOrderOther.Unit.VBDocumentLayoutOrder = 12;

            var items = viewModel.UnitCosts.Where(element => element.IsSelected).OrderBy(s => s.Unit.VBDocumentLayoutOrder).ToList();
            List<PdfFormField> annotations = new List<PdfFormField>();
            foreach (var item in items)
            {
                PdfPCell cellform = new PdfPCell() { Border = Rectangle.NO_BORDER };
                cellform.FixedHeight = 5f;

                // Initiate form checkbox 
                PdfFormField _checkGroup = PdfFormField.CreateEmpty(writer);
                RadioCheckField _radioG;
                PdfFormField _radioField1;
                Rectangle kotak = new Rectangle(100, 100);
                _radioG = new RadioCheckField(writer, kotak, "abc", "Yes");
                _radioG.CheckType = RadioCheckField.TYPE_CHECK;
                _radioG.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
                _radioG.BorderColor = BaseColor.Black;
                _radioG.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;

                _radioG.Checked = item.IsSelected;
                bool flag = item.IsSelected;

                _radioG.Rotation = 0;
                _radioG.Options = TextField.READ_ONLY;
                _radioField1 = _radioG.CheckField;

                cellform.CellEvent
                     = new BebanUnitEvent(_checkGroup, _radioField1, 1);
                headerTable3a.AddCell(cellform);

                // Beban Unit Item
                cellHeaderBody.Colspan = 1;
                if (string.IsNullOrEmpty(item.Unit.Name))
                {
                    cellHeaderBody.Phrase = new Phrase("......", normal_font_8);
                }
                else
                {
                    string unitName = item.Unit.Name;
                    if (item.Unit.Code == "S2")
                    {
                        unitName = "SPINNING";
                    }
                    cellHeaderBody.Phrase = new Phrase(unitName, normal_font_8);
                }
                cellHeaderBody.HorizontalAlignment = Element.ALIGN_LEFT;
                headerTable3a.AddCell(cellHeaderBody);

                cellHeaderBody.Phrase = new Phrase("", normal_font_8);

                if (!flag)
                {
                    cellHeaderBody.Colspan = 2;
                    cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                    headerTable3a.AddCell(cellHeaderBody);
                }
                else
                {
                    var nom = item.Amount.ToString("#,##0.00", new CultureInfo("id-ID"));

                    // Beban Unit Item Mata Uang
                    cellHeaderBody.Colspan = 1;
                    cellHeaderBody.Phrase = new Phrase(currencyCode, normal_font_8);
                    cellHeaderBody.HorizontalAlignment = Element.ALIGN_RIGHT;
                    headerTable3a.AddCell(cellHeaderBody);

                    // Beban Unit Item Nominal
                    cellHeaderBody.Colspan = 1;
                    cellHeaderBody.Phrase = new Phrase(nom, normal_font_8);
                    cellHeaderBody.HorizontalAlignment = Element.ALIGN_RIGHT;
                    headerTable3a.AddCell(cellHeaderBody);
                }

                //PPh
                if (pph_supplier != 0 && pph_danliris == 0)
                {
                    decimal pph_supplier_unit_item = ((item.Amount / grandTotal) * pph_supplier);

                    cellHeaderBody.Colspan = 1;
                    cellHeaderBody.Phrase = new Phrase(pph_supplier_unit_item.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font_8);
                    cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerTable3a.AddCell(cellHeaderBody);
                }
                else if (pph_supplier == 0 && pph_danliris != 0)
                {
                    decimal pph_danliris_unit_item = ((item.Amount / grandTotal) * pph_danliris);

                    cellHeaderBody.Colspan = 1;
                    cellHeaderBody.Phrase = new Phrase(pph_danliris_unit_item.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font_8);
                    cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerTable3a.AddCell(cellHeaderBody);
                }
                else if (pph_supplier != 0 && pph_danliris != 0)
                {
                    decimal pph_unit_item = ((item.Amount / grandTotal) * (pph_supplier + pph_danliris));

                    cellHeaderBody.Colspan = 1;
                    cellHeaderBody.Phrase = new Phrase(pph_unit_item.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font_8);
                    cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerTable3a.AddCell(cellHeaderBody);
                }

                //PPn
                if (ppn_manually != 0)
                {
                    decimal ppn_unit_item = ((item.Amount / grandTotal) * ppn_manually);

                    if (pph_danliris == 0 && pph_supplier == 0)
                    {
                        cellHeaderBody.Colspan = 1;
                        cellHeaderBody.Phrase = new Phrase(ppn_unit_item.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font_8);
                        cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
                        headerTable3a.AddCell(cellHeaderBody);

                        cellHeaderBody.Colspan = 1;
                        cellHeaderBody.Phrase = new Phrase(" ", normal_font_8);
                        headerTable3a.AddCell(cellHeaderBody);
                    }
                    else
                    {
                        cellHeaderBody.Colspan = 1;
                        cellHeaderBody.Phrase = new Phrase(ppn_unit_item.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font_8);
                        cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;
                        headerTable3a.AddCell(cellHeaderBody);
                    }
                }
                else
                {
                    if (pph_danliris == 0 && pph_supplier == 0)
                    {
                        cellHeaderBody.Colspan = 2;
                        cellHeaderBody.Phrase = new Phrase(" ", normal_font_8);
                        headerTable3a.AddCell(cellHeaderBody);
                    }
                    else
                    {
                        cellHeaderBody.Colspan = 1;
                        cellHeaderBody.Phrase = new Phrase(" ", normal_font_8);
                        headerTable3a.AddCell(cellHeaderBody);
                    }
                }

                // Empty space
                cellHeaderBody.Colspan = 1;
                cellHeaderBody.Phrase = new Phrase(" ", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);

                annotations.Add(_checkGroup);
            }

            for (var i = 0; i < 9 - (3 * (items.Count() % 3)); i++)
            {
                cellHeaderBody.Phrase = new Phrase(" ", normal_font);
                headerTable3a.AddCell(cellHeaderBody);
            }

            cellHeader3a.AddElement(headerTable3a);
            headerTable_C.AddCell(cellHeader3a);

            var cellLeft = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            };

            var emptyBorder = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER
            };

            cellLeft.Colspan = 5;
            cellLeft.Phrase = new Phrase("\n\nKeterangan: ", normal_font);
            headerTable3b.AddCell(cellLeft);

            cellLeft.Colspan = 1;
            cellLeft.Phrase = new Phrase("", normal_font);
            headerTable3b.AddCell(cellLeft);
            cellLeft.Colspan = 4;
            cellLeft.Phrase = new Phrase(viewModel.Remark, normal_font);
            headerTable3b.AddCell(cellLeft);
            emptyBorder.AddElement(headerTable3b);
            headerTable_C.AddCell(emptyBorder);

            document.Add(headerTable_C);

            foreach (var annotation in annotations)
            {
                writer.AddAnnotation(annotation);
            }
            #endregion

            #region Keterangan
            cellHeaderBody4a.Colspan = 7;
            cellHeaderBody4a.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody4a.Phrase = new Phrase("Keterangan: " + viewModel.Remark, normal_font);
            headerTable3.AddCell(cellHeaderBody4a);
            #endregion

            #region Footer
            PdfPTable table = new PdfPTable(5)
            {
                WidthPercentage = 97
            };
            float[] widths = new float[] { 1f, 1f, 1f, 1f, 1f };
            table.SetWidths(widths);
            PdfPCell cell = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            };

            cell.Phrase = new Phrase("\n\n", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("\n\n", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("\n\n", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("\n\n", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("\n\n", normal_font);
            table.AddCell(cell);

            // Menyetujui
            cell.Phrase = new Phrase("Menyetujui,", normal_font);
            table.AddCell(cell);

            // Diperiksa
            cell.Phrase = new Phrase("Diperiksa,", normal_font);
            table.AddCell(cell);

            // Mengetahui
            cell.Colspan = 2;
            cell.Phrase = new Phrase("Mengetahui,", normal_font);
            table.AddCell(cell);

            // Pembuat laporan
            cell.Colspan = 1;
            cell.Phrase = new Phrase("Pembuat laporan,", normal_font);
            table.AddCell(cell);

            for (var i = 0; i < 11; i++)
            {
                cell.Phrase = new Phrase("", normal_font);
                table.AddCell(cell);
                cell.Phrase = new Phrase("", normal_font);
                table.AddCell(cell);
                cell.Phrase = new Phrase("", normal_font);
                table.AddCell(cell);
                cell.Phrase = new Phrase("", normal_font);
                table.AddCell(cell);
                cell.Phrase = new Phrase("", normal_font);
                table.AddCell(cell);
            }

            cell.Phrase = new Phrase("(..................)", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("(..................)", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("(..................)", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase($"(..................)", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase($"({viewModel.CreatedBy})", normal_font);
            table.AddCell(cell);
            document.Add(table);
            #endregion Footer

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private string Nom(decimal total, string symbol, string description)
        {
            string TotalPaidString;
            string CurrencySay;
            if (symbol == "Rp" || symbol.ToUpper() == "IDR")
            {
                TotalPaidString = NumberToTextIDN.terbilang((double)total);
                CurrencySay = "Rupiah";
            }
            else
            {
                TotalPaidString = NumberToTextIDN.terbilang((double)total);
                CurrencySay = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(description.ToLower());
            }

            return TotalPaidString + " " + CurrencySay;
        }

        private decimal GetPPhValue(VBRealizationDocumentNonPOViewModel viewModel)
        {
            decimal val = 0;

            foreach (var itm in viewModel.Items)
            {
                if (itm.IsGetPPh == true && itm.IncomeTaxBy == "Supplier")
                {
                    val += itm.Amount * ((decimal)itm.IncomeTax.Rate.GetValueOrDefault() / 100);
                }
            }

            return val;
        }

        private decimal GetPPhValueDanLiris(VBRealizationDocumentNonPOViewModel viewModel)
        {
            decimal val = 0;

            foreach (var itm in viewModel.Items)
            {
                if (itm.IsGetPPh == true && itm.IncomeTaxBy != "Supplier")
                {
                    val += itm.Amount * ((decimal)itm.IncomeTax.Rate.GetValueOrDefault() / 100);
                }
            }

            return val;
        }
    }
}
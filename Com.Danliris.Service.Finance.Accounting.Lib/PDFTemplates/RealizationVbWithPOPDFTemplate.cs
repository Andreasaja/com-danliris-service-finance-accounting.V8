using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO
{
    public class RealizationVbWithPOPDFTemplate
    {
        public MemoryStream GeneratePdfTemplate(RealizationVbWithPOViewModel viewModel, int timeoffsset)
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
            PdfPTable headerTable3 = new PdfPTable(5);
            PdfPTable headerTable3a = new PdfPTable(9);
            PdfPTable headerTable4 = new PdfPTable(2);
            headerTable_A.SetWidths(new float[] { 10f, 10f });
            headerTable_A.WidthPercentage = 100;
            headerTable3.SetWidths(new float[] { 5f, 15f, 40f, 20f, 20f });
            headerTable3.WidthPercentage = 110;
            headerTable3a.SetWidths(new float[] { 20f, 3f, 20f, 20f, 3f, 20f, 20f, 3f, 20f });
            headerTable3a.WidthPercentage = 120;
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
            PdfPCell cellHeaderBody1b = new PdfPCell() { BorderWidthTop = 2 };
            PdfPCell cellHeaderBody2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody3 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody4 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody4a = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody4b = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody5 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody6 = new PdfPCell() { };

            cellHeader1.AddElement(headerTable1);
            headerTable_A.AddCell(cellHeader1);

            cellHeader2.AddElement(headerTable2);
            headerTable_A.AddCell(cellHeader2);

            document.Add(headerTable_A);

            cellHeaderBody.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody1.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody1a.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody1b.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody3.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody4.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody4a.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody4b.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellHeaderBody6.HorizontalAlignment = Element.ALIGN_LEFT;

            cellHeaderBody2.Colspan = 5;
            cellHeaderBody2.Phrase = new Phrase("REALISASI VB DENGAN PO", bold_font);
            headerTable3.AddCell(cellHeaderBody2);

            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody3.Colspan = 5;
            cellHeaderBody3.Phrase = new Phrase($"Realisasi VB Bagian: {viewModel.numberVB.UnitName}", bold_font);
            headerTable3.AddCell(cellHeaderBody3);

            cellHeaderBody3.Colspan = 5;
            cellHeaderBody3.Phrase = new Phrase($"Tanggal: {viewModel.Date?.AddHours(timeoffsset).ToString("dd MMMM yyyy", new CultureInfo("id-ID"))}", bold_font);
            headerTable3.AddCell(cellHeaderBody3);

            cellHeaderBody1.Phrase = new Phrase("No", normal_font);
            headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody1.Phrase = new Phrase("Tanggal", normal_font);
            headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody1.Phrase = new Phrase("Keterangan", normal_font);
            headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody1.Phrase = new Phrase("Unit", normal_font);
            headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody1.Phrase = new Phrase("Jumlah", normal_font);
            headerTable3.AddCell(cellHeaderBody1);

            int index = 1;
            decimal count_price = 0;
            string currencycode = "";
            double currencyrate = 0;
            decimal[] priceTotal = new decimal[viewModel.Items.Count];

            int j = 0;
            foreach (var itm in viewModel.Items)
            {

                foreach (var itm2 in itm.item)
                {
                    var temp = itm2.unitReceiptNote;

                    foreach (var itm3 in temp.items)
                    {
                        priceTotal[j] = itm3.PriceTotal;
                        j++;
                    }
                    break;
                }
                break;
            }

            int k = 0;
            List<Part> parts = new List<Part>();

            foreach (var itm in viewModel.Items)
            {
                cellHeaderBody1.Phrase = new Phrase(index.ToString(), normal_font);
                headerTable3.AddCell(cellHeaderBody1);
                index++;

                cellHeaderBody1.Phrase = new Phrase(itm.date?.AddHours(timeoffsset).ToString("dd/MM/yyyy"), normal_font);
                headerTable3.AddCell(cellHeaderBody1);

                cellHeaderBody1.Phrase = new Phrase(itm.no + $" ({itm.supplier.name})", normal_font);
                headerTable3.AddCell(cellHeaderBody1);

                cellHeaderBody1.Phrase = new Phrase(itm.division, normal_font);
                headerTable3.AddCell(cellHeaderBody1);

                currencycode = itm.currency.code;
                currencyrate = itm.currency.rate;

                parts.Add(new Part() { Unit = itm.division, Amount = priceTotal[k], CurrencyCode = itm.currency.code,CurrencyRate = itm.currency.rate});

                cellHeaderBody1.Phrase = new Phrase("Rp." + Convert_Rate(priceTotal[k], currencycode, currencyrate).ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
                headerTable3.AddCell(cellHeaderBody1);
                count_price += Convert_Rate(priceTotal[k], currencycode, currencyrate);
                k++;
            }

            cellHeaderBody1b.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody1b);
            cellHeaderBody1b.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody1b);
            cellHeaderBody1a.Colspan = 2;
            cellHeaderBody1a.Phrase = new Phrase("Total Realisasi", normal_font);
            headerTable3.AddCell(cellHeaderBody1a);
            cellHeaderBody1b.Phrase = new Phrase("Rp." + count_price.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            headerTable3.AddCell(cellHeaderBody1b);
            //cellHeaderBody1.Phrase = new Phrase(" ", normal_font);
            //headerTable3.AddCell(cellHeaderBody1);

            //cellHeaderBody1.Colspan = 2;
            cellHeaderBody1.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody1.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody6.Colspan = 2;
            cellHeaderBody6.Phrase = new Phrase($"No.VB: {viewModel.numberVB.VBNo}", normal_font);
            headerTable3.AddCell(cellHeaderBody6);
            cellHeaderBody1.Phrase = new Phrase("Rp." + viewModel.numberVB.Amount.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            headerTable3.AddCell(cellHeaderBody1);

            var res = count_price - viewModel.numberVB.Amount;

            cellHeaderBody5.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody5);
            cellHeaderBody5.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody5);
            cellHeaderBody5.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody5);
            

            if (res > 0)
            {
                cellHeaderBody5.Phrase = new Phrase("Kurang", bold_font);
                headerTable3.AddCell(cellHeaderBody5);

                cellHeaderBody5.Phrase = new Phrase("(Rp." + res.ToString("#,##0.00", new CultureInfo("id-ID")) + ")", normal_font);
                headerTable3.AddCell(cellHeaderBody5);
            }
            else
            {
                cellHeaderBody5.Phrase = new Phrase("Sisa", bold_font);
                headerTable3.AddCell(cellHeaderBody5);

                cellHeaderBody5.Phrase = new Phrase("Rp." + (res * -1).ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
                headerTable3.AddCell(cellHeaderBody5);
            }            

            cellHeaderBody4.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4);
            cellHeaderBody4.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4);
            cellHeaderBody4.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4);
            cellHeaderBody4.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4);
            cellHeaderBody4.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4);

            cellHeaderBody4a.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4a);
            cellHeaderBody4a.Phrase = new Phrase("Terbilang : ", normal_font);
            headerTable3.AddCell(cellHeaderBody4a);
            cellHeaderBody4b.Colspan = 2;
            cellHeaderBody4b.Phrase = new Phrase(Nom(count_price, viewModel), normal_font);
            headerTable3.AddCell(cellHeaderBody4b);
            cellHeaderBody4b.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4b);

            cellHeaderBody4.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4);
            cellHeaderBody4.Phrase = new Phrase("Beban Unit :", bold_font);
            headerTable3.AddCell(cellHeaderBody4);
            cellHeaderBody4.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4);
            cellHeaderBody4.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4);
            cellHeaderBody4.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4);

            cellHeader3.AddElement(headerTable3);
            headerTable_B.AddCell(cellHeader3);

            cellHeader4.AddElement(headerTable4);
            headerTable_B.AddCell(cellHeader4);

            document.Add(headerTable_B);

            #endregion Header

            #region CheckBox
            
            decimal total_count = 0;

            cellHeaderBody.Phrase = new Phrase("Spinning 1", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);

            //Create_Box(writer,headerTable3a);

            PdfPCell cellform = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform.FixedHeight = 5f;
            //initiate form checkbox 

            PdfFormField _checkGroup = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG;
            PdfFormField _radioField1;
            Rectangle kotak = new Rectangle(100, 100);
            _radioG = new RadioCheckField(writer, kotak, "abc", "Yes");
            _radioG.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG.BorderColor = BaseColor.Black;
            _radioG.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;

            foreach(var tem in parts)
            {
                if(tem.Unit.ToUpper().Contains("SPINNING 1"))
                {
                    _radioG.Checked = true;
                        
                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG.Checked = false;
                }
            }

            
            _radioG.Rotation = 0;
            _radioG.Options = TextField.READ_ONLY;
            _radioField1 = _radioG.CheckField;

            cellform.CellEvent
                 = new BebanUnitEvent(_checkGroup, _radioField1, 1);
            headerTable3a.AddCell(cellform);

            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }

            cellHeaderBody.Phrase = new Phrase("Printing", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform11 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform11.FixedHeight = 5f;
            //initiate form checkbox
            PdfFormField _checkGroup11 = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG11;
            PdfFormField _radioField111;
            Rectangle kotak11 = new Rectangle(100, 100);
            _radioG11 = new RadioCheckField(writer, kotak11, "abc", "Yes");
            _radioG11.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG11.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG11.BorderColor = BaseColor.Black;
            _radioG11.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;


            
            total_count = 0;
            foreach (var tem in parts)
            {
                if (tem.Unit.ToUpper().Contains("PRINTING"))
                {
                    _radioG11.Checked = true;

                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG11.Checked = false;
                }
            }

            _radioG11.Rotation = 0;
            _radioG11.Options = TextField.READ_ONLY;
            _radioField111 = _radioG11.CheckField;
            cellform11.CellEvent
                 = new BebanUnitEvent(_checkGroup11, _radioField111, 1);
            headerTable3a.AddCell(cellform11);

            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }



            cellHeaderBody.Phrase = new Phrase("Konfeksi 2B", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform8 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform8.FixedHeight = 5f;
            //initiate form checkbox
            PdfFormField _checkGroup8 = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG8;
            PdfFormField _radioField18;
            Rectangle kotak8 = new Rectangle(100, 100);
            _radioG8 = new RadioCheckField(writer, kotak8, "abc", "Yes");
            _radioG8.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG8.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG8.BorderColor = BaseColor.Black;
            _radioG8.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;

            
            total_count = 0;
            foreach (var tem in parts)
            {
                if (tem.Unit.ToUpper().Contains("KONFEKSI 2B"))
                {
                    _radioG8.Checked = true;

                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG8.Checked = false;
                }
            }


            _radioG8.Rotation = 0;
            _radioG8.Options = TextField.READ_ONLY;
            _radioField18 = _radioG8.CheckField;
            cellform8.CellEvent
                 = new BebanUnitEvent(_checkGroup8, _radioField18, 1);
            headerTable3a.AddCell(cellform8);

            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }


            cellHeaderBody.Phrase = new Phrase("Spinning 2", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform5 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform5.FixedHeight = 5f;
            //initiate form checkbox
            PdfFormField _checkGroup5 = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG5;
            PdfFormField _radioField15;
            Rectangle kotak5 = new Rectangle(100, 100);
            _radioG5 = new RadioCheckField(writer, kotak5, "abc", "Yes");
            _radioG5.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG5.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG5.BorderColor = BaseColor.Black;
            _radioG5.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;

            
            total_count = 0;
            foreach (var tem in parts)
            {
                if (tem.Unit.ToUpper().Contains("SPINNING 2"))
                {
                    _radioG5.Checked = true;

                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG5.Checked = false;
                }
            }

            _radioG5.Rotation = 0;
            _radioG5.Options = TextField.READ_ONLY;
            _radioField15 = _radioG5.CheckField;
            cellform5.CellEvent
                 = new BebanUnitEvent(_checkGroup5, _radioField15, 1);
            headerTable3a.AddCell(cellform5);

            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }

            cellHeaderBody.Phrase = new Phrase("Dyeing", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform5a = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform5.FixedHeight = 5f;
            //initiate form checkbox
            PdfFormField _checkGroup5a = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG5a;
            PdfFormField _radioField15a;
            Rectangle kotak5a = new Rectangle(100, 100);
            _radioG5a = new RadioCheckField(writer, kotak5a, "abc", "Yes");
            _radioG5a.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG5a.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG5a.BorderColor = BaseColor.Black;
            _radioG5a.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;


            
            total_count = 0;
            foreach (var tem in parts)
            {
                if (tem.Unit.ToUpper().Contains("SPINNING 1"))
                {
                    _radioG5a.Checked = true;

                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG5a.Checked = false;
                }
            }

            _radioG5a.Rotation = 0;
            _radioG5a.Options = TextField.READ_ONLY;
            _radioField15a = _radioG5a.CheckField;
            cellform5a.CellEvent
                 = new BebanUnitEvent(_checkGroup5a, _radioField15a, 1);
            headerTable3a.AddCell(cellform5a);

            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }

            cellHeaderBody.Phrase = new Phrase("Konfeksi 2C", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform13 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform13.FixedHeight = 5f;
            //initiate form checkbox
            PdfFormField _checkGroup13 = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG13;
            PdfFormField _radioField113;
            Rectangle kotak13 = new Rectangle(100, 100);
            _radioG13 = new RadioCheckField(writer, kotak13, "abc", "Yes");
            _radioG13.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG13.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG13.BorderColor = BaseColor.Black;
            _radioG13.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;


            
            total_count = 0;
            foreach (var tem in parts)
            {
                if (tem.Unit.ToUpper().Contains("SPINNING 1"))
                {
                    _radioG13.Checked = true;

                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG13.Checked = false;
                }
            }

            _radioG13.Rotation = 0;
            _radioG13.Options = TextField.READ_ONLY;
            _radioField113 = _radioG13.CheckField;
            cellform13.CellEvent
                 = new BebanUnitEvent(_checkGroup13, _radioField113, 1);
            headerTable3a.AddCell(cellform13);

            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }

            cellHeaderBody.Phrase = new Phrase("Spinning 3", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform10 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform10.FixedHeight = 5f;
            //initiate form checkbox
            PdfFormField _checkGroup10 = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG10;
            PdfFormField _radioField110;
            Rectangle kotak10 = new Rectangle(100, 100);
            _radioG10 = new RadioCheckField(writer, kotak10, "abc", "Yes");
            _radioG10.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG10.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG10.BorderColor = BaseColor.Black;
            _radioG10.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;

            
            total_count = 0;
            foreach (var tem in parts)
            {
                if (tem.Unit.ToUpper().Contains("SPINNING 1"))
                {
                    _radioG10.Checked = true;

                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG10.Checked = false;
                }
            }

            _radioG10.Rotation = 0;
            _radioG10.Options = TextField.READ_ONLY;
            _radioField110 = _radioG10.CheckField;
            cellform10.CellEvent
                 = new BebanUnitEvent(_checkGroup10, _radioField110, 1);
            headerTable3a.AddCell(cellform10);
            
            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);
            }

            cellHeaderBody.Phrase = new Phrase("Konfeksi 1A", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform7 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform7.FixedHeight = 5f;
            //initiate form checkbox
            PdfFormField _checkGroup7 = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG7;
            PdfFormField _radioField17;
            Rectangle kotak7 = new Rectangle(100, 100);
            _radioG7 = new RadioCheckField(writer, kotak7, "abc", "Yes");
            _radioG7.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG7.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG7.BorderColor = BaseColor.Black;
            _radioG7.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;


            
            total_count = 0;
            foreach (var tem in parts)
            {
                if (tem.Unit.ToUpper().Contains("SPINNING 1"))
                {
                    _radioG7.Checked = true;

                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG7.Checked = false;
                }
            }

            _radioG7.Rotation = 0;
            _radioG7.Options = TextField.READ_ONLY;
            _radioField17 = _radioG7.CheckField;
            cellform7.CellEvent
                 = new BebanUnitEvent(_checkGroup7, _radioField17, 1);
            headerTable3a.AddCell(cellform7);
            
            if(total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }

            cellHeaderBody.Phrase = new Phrase("Umum", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform4 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform4.FixedHeight = 5f;
            //initiate form checkbox
            PdfFormField _checkGroup4 = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG4;
            PdfFormField _radioField14;
            Rectangle kotak4 = new Rectangle(100, 100);
            _radioG4 = new RadioCheckField(writer, kotak4, "abc", "Yes");
            _radioG4.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG4.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG4.BorderColor = BaseColor.Black;
            _radioG4.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;


            
            total_count = 0;
            foreach (var tem in parts)
            {
                if (tem.Unit.ToUpper().Contains("SPINNING 1"))
                {
                    _radioG4.Checked = true;

                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG4.Checked = false;
                }
            }

            _radioG4.Rotation = 0;
            _radioG4.Options = TextField.READ_ONLY;
            _radioField14 = _radioG4.CheckField;
            cellform4.CellEvent
                 = new BebanUnitEvent(_checkGroup4, _radioField14, 1);
            headerTable3a.AddCell(cellform4);
            
            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }

            cellHeaderBody.Phrase = new Phrase("Weaving 1", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform1 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform1.FixedHeight = 5f;
            //initiate form checkbox 

            PdfFormField _checkGroup1 = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG1;
            PdfFormField _radioField11;
            Rectangle kotak1 = new Rectangle(100, 100);
            _radioG1 = new RadioCheckField(writer, kotak1, "abc", "Yes");
            _radioG1.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG1.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG1.BorderColor = BaseColor.Black;
            _radioG1.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;

            
            total_count = 0;
            foreach (var tem in parts)
            {
                if (tem.Unit.ToUpper().Contains("SPINNING 1"))
                {
                    _radioG1.Checked = true;

                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG1.Checked = false;
                }
            }

            _radioG1.Rotation = 0;
            _radioG1.Options = TextField.READ_ONLY;
            _radioField11 = _radioG1.CheckField;

            cellform1.CellEvent
                 = new BebanUnitEvent(_checkGroup1, _radioField11, 1);
            headerTable3a.AddCell(cellform1);
            
            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }

            cellHeaderBody.Phrase = new Phrase("Konfeksi 1B", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform12 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform12.FixedHeight = 5f;
            //initiate form checkbox
            PdfFormField _checkGroup12 = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG12;
            PdfFormField _radioField112;
            Rectangle kotak12 = new Rectangle(100, 100);
            _radioG12 = new RadioCheckField(writer, kotak12, "abc", "Yes");
            _radioG12.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG12.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG12.BorderColor = BaseColor.Black;
            _radioG12.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;

            
            total_count = 0;
            foreach (var tem in parts)
            {
                if (tem.Unit.ToUpper().Contains("SPINNING 1"))
                {
                    _radioG12.Checked = true;

                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG12.Checked = false;
                }
            }

            _radioG12.Rotation = 0;
            _radioG12.Options = TextField.READ_ONLY;
            _radioField112 = _radioG12.CheckField;
            cellform12.CellEvent
                 = new BebanUnitEvent(_checkGroup12, _radioField112, 1);
            headerTable3a.AddCell(cellform12);
            
            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }


            foreach (var itm in viewModel.Items)
            {
                if (!itm.division.ToUpper().Contains("SPINNING 1") && !itm.division.ToUpper().Contains("SPINNING 2") && !itm.division.ToUpper().Contains("SPINNING 3")
                    && !itm.division.ToUpper().Contains("WEAVING 1") && !itm.division.ToUpper().Contains("WEAVING 2") && !itm.division.ToUpper().Contains("PRINTING")
                    && !itm.division.ToUpper().Contains("DYEING") && !itm.division.ToUpper().Contains("KONFEKSI 1A") && !itm.division.ToUpper().Contains("KONFEKSI 1B")
                    && !itm.division.ToUpper().Contains("KONFEKSI 2A") && !itm.division.ToUpper().Contains("KONFEKSI 2B") && !itm.division.ToUpper().Contains("KONFEKSI 2C")
                    && !itm.division.ToUpper().Contains("UMUM"))
                {
                    cellHeaderBody.Phrase = new Phrase(itm.division.ToUpper(), normal_font_8);
                    headerTable3a.AddCell(cellHeaderBody);
                }
                else
                {
                    cellHeaderBody.Phrase = new Phrase(" ", normal_font_8);
                    headerTable3a.AddCell(cellHeaderBody);
                }
                break;
            }

            
            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform9 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform9.FixedHeight = 5f;
            //initiate form checkbox
            PdfFormField _checkGroup9 = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG9;
            PdfFormField _radioField19;
            Rectangle kotak9 = new Rectangle(100, 100);
            _radioG9 = new RadioCheckField(writer, kotak9, "abc", "Yes");
            _radioG9.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG9.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG9.BorderColor = BaseColor.Black;
            _radioG9.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;

            
            total_count = 0;
            foreach (var itm in viewModel.Items)
            {
                if (!itm.division.ToUpper().Contains("SPINNING 1") && !itm.division.ToUpper().Contains("SPINNING 2") && !itm.division.ToUpper().Contains("SPINNING 3")
                    && !itm.division.ToUpper().Contains("WEAVING 1") && !itm.division.ToUpper().Contains("WEAVING 2") && !itm.division.ToUpper().Contains("PRINTING")
                    && !itm.division.ToUpper().Contains("DYEING") && !itm.division.ToUpper().Contains("KONFEKSI 1A") && !itm.division.ToUpper().Contains("KONFEKSI 1B")
                    && !itm.division.ToUpper().Contains("KONFEKSI 2A") && !itm.division.ToUpper().Contains("KONFEKSI 2B") && !itm.division.ToUpper().Contains("KONFEKSI 2C")
                    && !itm.division.ToUpper().Contains("UMUM"))
                {
                    _radioG9.Checked = true;

                    currencycode = itm.currency.code;
                    currencyrate = itm.currency.rate;

                    foreach (var itm2 in itm.item)
                    {
                        var temp = itm2.unitReceiptNote;

                        foreach (var itm3 in temp.items)
                        {

                            //total = Convert_Rate(itm3.PriceTotal, currencycode, currencyrate).ToString("#,##0.00", new CultureInfo("id-ID"));
                            total_count += Convert_Rate(itm3.PriceTotal, currencycode, currencyrate);
                        }
                        break;
                    }
                }
                else
                {
                    _radioG9.Checked = false;
                }
                break;
            }

            _radioG9.Rotation = 0;
            _radioG9.Options = TextField.READ_ONLY;
            _radioField19 = _radioG9.CheckField;
            cellform9.CellEvent
                 = new BebanUnitEvent(_checkGroup9, _radioField19, 1);
            headerTable3a.AddCell(cellform9);
            //cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            //headerTable3a.AddCell(cellHeaderBody);
            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }



            cellHeaderBody.Phrase = new Phrase("Weaving 2", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform6 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform6.FixedHeight = 5f;
            //initiate form checkbox
            PdfFormField _checkGroup6 = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG6;
            PdfFormField _radioField16;
            Rectangle kotak6 = new Rectangle(100, 100);
            _radioG6 = new RadioCheckField(writer, kotak6, "abc", "Yes");
            _radioG6.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG6.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG6.BorderColor = BaseColor.Black;
            _radioG6.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;

            
            total_count = 0;
            foreach (var tem in parts)
            {
                if (tem.Unit.ToUpper().Contains("SPINNING 1"))
                {
                    _radioG6.Checked = true;

                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG6.Checked = false;
                }
            }

            _radioG6.Rotation = 0;
            _radioG6.Options = TextField.READ_ONLY;
            _radioField16 = _radioG6.CheckField;
            cellform6.CellEvent
                 = new BebanUnitEvent(_checkGroup6, _radioField16, 1);
            headerTable3a.AddCell(cellform6);
            
            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }

            cellHeaderBody.Phrase = new Phrase("Konfeksi 2A", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            PdfPCell cellform3 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellform3.FixedHeight = 5f;
            //initiate form checkbox
            PdfFormField _checkGroup3 = PdfFormField.CreateEmpty(writer);
            RadioCheckField _radioG3;
            PdfFormField _radioField13;
            Rectangle kotak3 = new Rectangle(100, 100);
            _radioG3 = new RadioCheckField(writer, kotak3, "abc", "Yes");
            _radioG3.CheckType = RadioCheckField.TYPE_CHECK;
            _radioG3.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
            _radioG3.BorderColor = BaseColor.Black;
            _radioG3.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;

            
            total_count = 0;
            foreach (var tem in parts)
            {
                if (tem.Unit.ToUpper().Contains("SPINNING 1"))
                {
                    _radioG3.Checked = true;

                    currencycode = tem.CurrencyCode;
                    currencyrate = tem.CurrencyRate;

                    total_count += Convert_Rate(tem.Amount, currencycode, currencyrate);
                }
                else
                {
                    _radioG3.Checked = false;
                }
            }

            _radioG3.Rotation = 0;
            _radioG3.Options = TextField.READ_ONLY;
            _radioField13 = _radioG3.CheckField;
            cellform3.CellEvent
                 = new BebanUnitEvent(_checkGroup3, _radioField13, 1);
            headerTable3a.AddCell(cellform3);
            
            if (total_count == 0)
            {
                cellHeaderBody.Phrase = new Phrase(":Rp...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($":Rp   {total_count.ToString("#,##0.00", new CultureInfo("id-ID"))}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }


            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);


            cellHeaderBody.Phrase = new Phrase(" ", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font_8);
            headerTable3a.AddCell(cellHeaderBody);

            cellHeader3a.AddElement(headerTable3a);
            headerTable_C.AddCell(cellHeader3a);
            document.Add(headerTable_C);
            writer.AddAnnotation(_checkGroup);
            writer.AddAnnotation(_checkGroup1);
            writer.AddAnnotation(_checkGroup3);
            writer.AddAnnotation(_checkGroup4);
            writer.AddAnnotation(_checkGroup5);
            writer.AddAnnotation(_checkGroup5a);
            writer.AddAnnotation(_checkGroup6);
            writer.AddAnnotation(_checkGroup7);
            writer.AddAnnotation(_checkGroup8);
            writer.AddAnnotation(_checkGroup9);
            writer.AddAnnotation(_checkGroup10);
            writer.AddAnnotation(_checkGroup11);
            writer.AddAnnotation(_checkGroup12);
            writer.AddAnnotation(_checkGroup13);
            #endregion

            #region Footer

            PdfPTable table = new PdfPTable(5)
            {
                WidthPercentage = 100
            };
            float[] widths = new float[] { 1f, 1f, 1f, 1f, 1f };
            table.SetWidths(widths);
            PdfPCell cell = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            };

            PdfPCell cellLeft = new PdfPCell()
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            };

            PdfPCell cellColspan = new PdfPCell()
            {
                Colspan = 2,
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            };


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

            cell.Phrase = new Phrase("Menyetujui,", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("Diperiksa,", normal_font);
            table.AddCell(cell);
            cellColspan.Phrase = new Phrase("Mengetahui,", normal_font);
            table.AddCell(cellColspan);
            //cell.Phrase = new Phrase("", normal_font);
            //table.AddCell(cell);
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
            cell.Phrase = new Phrase("(..................)", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase($"({viewModel.numberVB.CreateBy})", normal_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Kasir", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("Verifikasi", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase($"Dir {viewModel.numberVB.UnitName}", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase($"Kabag {viewModel.numberVB.UnitName}", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase(viewModel.numberVB.UnitName, normal_font);
            table.AddCell(cell);

            document.Add(table);
            #endregion Footer

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private string Nom(decimal total, RealizationVbWithPOViewModel viewModel)
        {

            string TotalPaidString = NumberToTextIDN.terbilang((double)total);

            return TotalPaidString + " Rupiah";
        }

        private decimal Convert_Rate(decimal price, string code, double rate)
        {
            double convertCurrency = 0;


            if (code == "IDR")
            {
                convertCurrency = (double)price;
            }
            else
            {
                convertCurrency = (Math.Round((double)price * (double)rate));
            }


            return (decimal)convertCurrency;
        }
    }

    public class Part
    {
        public string Unit { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public double CurrencyRate { get; set; }
    }
}
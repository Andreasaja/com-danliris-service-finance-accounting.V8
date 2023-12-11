using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Sales.Lib.Utilities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Globalization;
using System.IO;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBNonPO
{
    public class RealizationVbNonPOPDFTemplate
    {
        public MemoryStream GeneratePdfTemplate(RealizationVbNonPOViewModel viewModel, int timeoffsset)
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
            PdfPTable headerTable3 = new PdfPTable(4);
            PdfPTable headerTable3a = new PdfPTable(9);
            PdfPTable headerTable4 = new PdfPTable(2);
            headerTable_A.SetWidths(new float[] { 10f, 10f });
            headerTable_A.WidthPercentage = 100;
            headerTable3.SetWidths(new float[] { 10f, 20f, 40f, 20f });
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
            cellHeaderBody1a.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody1a1.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody1a2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody1b.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody1b1.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody1b2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody1c.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody3.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody4.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody4a.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody4b.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody5.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellHeaderBody5a.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody6.HorizontalAlignment = Element.ALIGN_LEFT;

            cellHeaderBody2.Colspan = 5;
            cellHeaderBody2.Phrase = new Phrase("REALISASI VB TANPA PO", bold_font);
            headerTable3.AddCell(cellHeaderBody2);

            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody3.Colspan = 4;
            cellHeaderBody3.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellHeaderBody3.Phrase = new Phrase($"{viewModel.VBRealizationNo}", bold_font);
            headerTable3.AddCell(cellHeaderBody3);

            cellHeaderBody3.Colspan = 4;
            cellHeaderBody3.HorizontalAlignment = Element.ALIGN_LEFT;
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
            //cellHeaderBody1.Phrase = new Phrase("Unit", normal_font);
            //headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody1.Phrase = new Phrase("Jumlah", normal_font);
            headerTable3.AddCell(cellHeaderBody1);

            int index = 1;
            decimal count_price = 0;
            decimal total_realization = 0;

            var items = viewModel.numberVB.UnitLoad.Split(",");

            string lastitem = items[items.Length - 1];

            lastitem = lastitem.Trim();

            decimal total_all = 0;
            foreach (var item in viewModel.Items)
            {
                if (item.isGetPPn == true)
                {
                    var temp = item.Amount.GetValueOrDefault() * 0.1m;
                    total_all += item.Amount.GetValueOrDefault() + temp;
                }
                else
                {
                    total_all += item.Amount.GetValueOrDefault();
                }
            }

            var currencyCode = viewModel.numberVB.CurrencyCode;
            var currencydescription = viewModel.numberVB.CurrencyDescription;

            foreach (var itm in viewModel.Items)
            {
                cellHeaderBody1.Phrase = new Phrase(index.ToString(), normal_font);
                headerTable3.AddCell(cellHeaderBody1);
                index++;

                cellHeaderBody1.Phrase = new Phrase(itm.DateDetail?.AddHours(timeoffsset).ToString("dd/MM/yyyy"), normal_font);
                headerTable3.AddCell(cellHeaderBody1);

                cellHeaderBody1.Phrase = new Phrase(itm.Remark, normal_font);
                headerTable3.AddCell(cellHeaderBody1);

                var currencycode = viewModel.numberVB.CurrencyCode;
                var currencyrate = (double)viewModel.numberVB.CurrencyRate;


                if (itm.isGetPPn == true)
                {
                    var temp = itm.Amount.GetValueOrDefault() * 0.1m;
                    total_all = itm.Amount.GetValueOrDefault() + temp;

                }
                else
                {
                    total_all = itm.Amount.GetValueOrDefault();
                }

                cellHeaderBody1.Phrase = new Phrase($"{currencyCode}        " + Convert_Rate(itm.Amount.GetValueOrDefault(), currencycode, currencyrate).ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
                headerTable3.AddCell(cellHeaderBody1);
                count_price += Convert_Rate(total_all, currencycode, currencyrate);
                total_realization += Convert_Rate(itm.Amount.GetValueOrDefault(), currencycode, currencyrate);
            }

            cellHeaderBody1b.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody1b);
            //cellHeaderBody1b.Phrase = new Phrase(" ", normal_font);
            //headerTable3.AddCell(cellHeaderBody1b);
            cellHeaderBody1a.Colspan = 2;
            cellHeaderBody1a.Phrase = new Phrase("Jumlah Realisasi", normal_font);
            headerTable3.AddCell(cellHeaderBody1a);
            cellHeaderBody1b.Phrase = new Phrase($"{currencyCode}       " + total_realization.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            headerTable3.AddCell(cellHeaderBody1b);



            cellHeaderBody1b1.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody1b1);
            //cellHeaderBody1b.Phrase = new Phrase(" ", normal_font);
            //headerTable3.AddCell(cellHeaderBody1b);
            cellHeaderBody1a1.Colspan = 2;
            cellHeaderBody1a1.Phrase = new Phrase("PPn", normal_font);
            headerTable3.AddCell(cellHeaderBody1a1);
            cellHeaderBody1b1.Phrase = new Phrase($"{currencyCode}       " + (count_price - total_realization).ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            headerTable3.AddCell(cellHeaderBody1b1);

            cellHeaderBody1b1.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody1b1);
            //cellHeaderBody1b.Phrase = new Phrase(" ", normal_font);
            //headerTable3.AddCell(cellHeaderBody1b);
            cellHeaderBody1a1.Colspan = 2;
            cellHeaderBody1a1.Phrase = new Phrase("PPh", normal_font);
            headerTable3.AddCell(cellHeaderBody1a1);
            cellHeaderBody1b1.Phrase = new Phrase($"{currencyCode}       " + (GetPPhValue(viewModel)).ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            headerTable3.AddCell(cellHeaderBody1b1);

            cellHeaderBody1b2.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody1b2);
            //cellHeaderBody1b.Phrase = new Phrase(" ", normal_font);
            //headerTable3.AddCell(cellHeaderBody1b);
            cellHeaderBody1a2.Colspan = 2;
            cellHeaderBody1a2.Phrase = new Phrase("Total Keseluruhan", normal_font);
            var grandTotal = count_price - GetPPhValue(viewModel);
            headerTable3.AddCell(cellHeaderBody1a2);
            cellHeaderBody1b2.Phrase = new Phrase($"{currencyCode}       " + grandTotal.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            headerTable3.AddCell(cellHeaderBody1b2);

            //cellHeaderBody1.Colspan = 2;
            cellHeaderBody6.Colspan = 2;
            //cellHeaderBody6.Phrase = new Phrase(" ", normal_font);
            //headerTable3.AddCell(cellHeaderBody6);
            var vbDate = viewModel.numberVB.Date?.AddHours(timeoffsset).ToString("dd-MMMM-yy", new CultureInfo("id-ID"));
            if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                if (viewModel.numberVB != null && !string.IsNullOrWhiteSpace(viewModel.numberVB.VBNo))
                    vbDate = viewModel.numberVB.Date?.AddHours(timeoffsset).ToString("dd-MMMM-yy", new CultureInfo("id-ID"));
                else
                    vbDate = "";

            cellHeaderBody6.Phrase = new Phrase($"Tanggal VB : {vbDate}", normal_font);
            headerTable3.AddCell(cellHeaderBody6);
            //
            cellHeaderBody1.Phrase = new Phrase($"No.VB: {viewModel.numberVB.VBNo}", normal_font);
            headerTable3.AddCell(cellHeaderBody1);
            cellHeaderBody1.Phrase = new Phrase($"{currencyCode}        " + viewModel.numberVB.Amount.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            headerTable3.AddCell(cellHeaderBody1);

            var priceterbilang = count_price;

            var res = (count_price - GetPPhValue(viewModel)) - viewModel.numberVB.Amount;

            cellHeaderBody5.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody5);
            cellHeaderBody5.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody5);
            //cellHeaderBody5.Phrase = new Phrase(" ", normal_font);
            //headerTable3.AddCell(cellHeaderBody5);


            if (res > 0)
            {
                cellHeaderBody5.Phrase = new Phrase("Kurang", bold_font);
                headerTable3.AddCell(cellHeaderBody5);

                cellHeaderBody5a.Phrase = new Phrase($"({currencyCode}      " + res.ToString("#,##0.00", new CultureInfo("id-ID")) + ")", normal_font);
                headerTable3.AddCell(cellHeaderBody5a);
            }
            else
            {
                cellHeaderBody5.Phrase = new Phrase("Sisa", bold_font);
                headerTable3.AddCell(cellHeaderBody5);

                cellHeaderBody5a.Phrase = new Phrase($"{currencyCode}       " + (res * -1).ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
                headerTable3.AddCell(cellHeaderBody5a);
            }

            count_price /= items.Length;
            string total = count_price.ToString("#,##0.00", new CultureInfo("id-ID"));

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
            cellHeaderBody4b.Phrase = new Phrase(Nom(grandTotal, currencyCode, currencydescription), normal_font);
            headerTable3.AddCell(cellHeaderBody4b);
            //cellHeaderBody4b.Phrase = new Phrase(" ", normal_font);
            //headerTable3.AddCell(cellHeaderBody4b);

            cellHeaderBody4.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody4);
            cellHeaderBody4.Phrase = new Phrase("Beban Unit :", bold_font);
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

            string weightunit = "";
            string resunit = "";
            string val_result = "";

            if (viewModel.TypeVBNonPO == "Dengan Nomor VB")
            {
                weightunit = viewModel.numberVB.UnitLoad.ToUpper();
            }
            else if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
            {
                if (viewModel.Spinning1 == true)
                {
                    resunit += "Spinning 1,";
                    val_result += viewModel.AmountSpinning1.ToString() + ",";
                }

                if (viewModel.Spinning2 == true)
                {
                    resunit += "Spinning 2,";
                    val_result += viewModel.AmountSpinning2.ToString() + ",";
                }

                if (viewModel.Spinning3 == true)
                {
                    resunit += "Spinning 3,";
                    val_result += viewModel.AmountSpinning3.ToString() + ",";
                }

                if (viewModel.Weaving1 == true)
                {
                    resunit += "Weaving 1,";
                    val_result += viewModel.AmountWeaving1.ToString() + ",";
                }

                if (viewModel.Weaving2 == true)
                {
                    resunit += "Weaving 2,";
                    val_result += viewModel.AmountWeaving2.ToString() + ",";
                }

                if (viewModel.Finishing == true)
                {
                    resunit += "Finishing,";
                    val_result += viewModel.AmountFinishing.ToString() + ",";
                }

                if (viewModel.Printing == true)
                {
                    resunit += "Printing,";
                    val_result += viewModel.AmountPrinting.ToString() + ",";
                }

                if (viewModel.Konfeksi1A == true)
                {
                    resunit += "Konfeksi 1A,";
                    val_result += viewModel.AmountKonfeksi1A.ToString() + ",";
                }

                if (viewModel.Konfeksi1B == true)
                {
                    resunit += "Konfeksi 1B,";
                    val_result += viewModel.AmountKonfeksi1B.ToString() + ",";
                }

                if (viewModel.Konfeksi2A == true)
                {
                    resunit += "Konfeksi 2A,";
                    val_result += viewModel.AmountKonfeksi2A.ToString() + ",";
                }

                if (viewModel.Konfeksi2B == true)
                {
                    resunit += "Konfeksi 2B,";
                    val_result += viewModel.AmountKonfeksi2B.ToString() + ",";
                }

                if (viewModel.Konfeksi2C == true)
                {
                    resunit += "Konfeksi 2C,";
                    val_result += viewModel.AmountKonfeksi2C.ToString() + ",";
                }

                if (viewModel.Umum == true)
                {
                    resunit += "Umum,";
                    val_result += viewModel.AmountUmum.ToString() + ",";
                }

                if (viewModel.Others == true)
                {
                    resunit += viewModel.DetailOthers + ",";
                    val_result += viewModel.AmountOthers.ToString() + ",";
                }

                resunit = resunit.Remove(resunit.Length - 1);
                val_result = val_result.Remove(val_result.Length - 1);

                weightunit = resunit.ToUpper();
            }

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

            bool flag;
            if (weightunit.Contains("SPINNING 1"))
            {
                _radioG.Checked = true;
                flag = true;
            }
            else
            {
                _radioG.Checked = false;
                flag = false;
            }

            _radioG.Rotation = 0;
            _radioG.Options = TextField.READ_ONLY;
            _radioField1 = _radioG.CheckField;

            cellform.CellEvent
                 = new BebanUnitEvent(_checkGroup, _radioField1, 1);
            headerTable3a.AddCell(cellform);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountSpinning1.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountSpinning1VB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8); //total
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


            if (weightunit.Contains("PRINTING"))
            {
                _radioG11.Checked = true;
                flag = true;
            }
            else
            {
                _radioG11.Checked = false;
                flag = false;
            }

            _radioG11.Rotation = 0;
            _radioG11.Options = TextField.READ_ONLY;
            _radioField111 = _radioG11.CheckField;
            cellform11.CellEvent
                 = new BebanUnitEvent(_checkGroup11, _radioField111, 1);
            headerTable3a.AddCell(cellform11);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountPrinting.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountPrintingVB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8);
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

            if (weightunit.Contains("KONFEKSI 2B"))
            {
                _radioG8.Checked = true;
                flag = true;
            }
            else
            {
                _radioG8.Checked = false;
                flag = false;
            }


            _radioG8.Rotation = 0;
            _radioG8.Options = TextField.READ_ONLY;
            _radioField18 = _radioG8.CheckField;
            cellform8.CellEvent
                 = new BebanUnitEvent(_checkGroup8, _radioField18, 1);
            headerTable3a.AddCell(cellform8);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountKonfeksi2B.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountKonfeksi2BVB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8);
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

            if (weightunit.Contains("SPINNING 2"))
            {
                _radioG5.Checked = true;
                flag = true;
            }
            else
            {
                _radioG5.Checked = false;
                flag = false;
            }

            _radioG5.Rotation = 0;
            _radioG5.Options = TextField.READ_ONLY;
            _radioField15 = _radioG5.CheckField;
            cellform5.CellEvent
                 = new BebanUnitEvent(_checkGroup5, _radioField15, 1);
            headerTable3a.AddCell(cellform5);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountSpinning2.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountSpinning2VB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }

            cellHeaderBody.Phrase = new Phrase("Finishing", normal_font_8);
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


            if (weightunit.Contains("DYEING"))
            {
                _radioG5a.Checked = true;
                flag = true;
            }
            else
            {
                _radioG5a.Checked = false;
                flag = false;
            }

            _radioG5a.Rotation = 0;
            _radioG5a.Options = TextField.READ_ONLY;
            _radioField15a = _radioG5a.CheckField;
            cellform5a.CellEvent
                 = new BebanUnitEvent(_checkGroup5a, _radioField15a, 1);
            headerTable3a.AddCell(cellform5a);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {total}", normal_font_8);
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


            if (weightunit.Contains("KONFEKSI 2C"))
            {
                _radioG13.Checked = true;
                flag = true;
            }
            else
            {
                _radioG13.Checked = false;
                flag = false;
            }

            _radioG13.Rotation = 0;
            _radioG13.Options = TextField.READ_ONLY;
            _radioField113 = _radioG13.CheckField;
            cellform13.CellEvent
                 = new BebanUnitEvent(_checkGroup13, _radioField113, 1);
            headerTable3a.AddCell(cellform13);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountKonfeksi2C.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountKonfeksi2CVB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8);
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

            if (weightunit.Contains("SPINNING 3"))
            {
                _radioG10.Checked = true;
                flag = true;
            }
            else
            {
                _radioG10.Checked = false;
                flag = false;
            }

            _radioG10.Rotation = 0;
            _radioG10.Options = TextField.READ_ONLY;
            _radioField110 = _radioG10.CheckField;
            cellform10.CellEvent
                 = new BebanUnitEvent(_checkGroup10, _radioField110, 1);
            headerTable3a.AddCell(cellform10);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountSpinning3.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountSpinning3VB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8);
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


            if (weightunit.Contains("KONFEKSI 1A"))
            {
                _radioG7.Checked = true;
                flag = true;
            }
            else
            {
                _radioG7.Checked = false;
                flag = false;
            }

            _radioG7.Rotation = 0;
            _radioG7.Options = TextField.READ_ONLY;
            _radioField17 = _radioG7.CheckField;
            cellform7.CellEvent
                 = new BebanUnitEvent(_checkGroup7, _radioField17, 1);
            headerTable3a.AddCell(cellform7);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountKonfeksi1A.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountKonfeksi1AVB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8);
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


            if (weightunit.Contains("UMUM"))
            {
                _radioG4.Checked = true;
                flag = true;
            }
            else
            {
                _radioG4.Checked = false;
                flag = false;
            }

            _radioG4.Rotation = 0;
            _radioG4.Options = TextField.READ_ONLY;
            _radioField14 = _radioG4.CheckField;
            cellform4.CellEvent
                 = new BebanUnitEvent(_checkGroup4, _radioField14, 1);
            headerTable3a.AddCell(cellform4);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountUmum.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountUmumVB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8);
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

            if (weightunit.Contains("WEAVING 1"))
            {
                _radioG1.Checked = true;
                flag = true;
            }
            else
            {
                _radioG1.Checked = false;
                flag = false;
            }

            _radioG1.Rotation = 0;
            _radioG1.Options = TextField.READ_ONLY;
            _radioField11 = _radioG1.CheckField;

            cellform1.CellEvent
                 = new BebanUnitEvent(_checkGroup1, _radioField11, 1);
            headerTable3a.AddCell(cellform1);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountWeaving1.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountWeaving1VB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8);
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

            if (weightunit.Contains("KONFEKSI 1B"))
            {
                _radioG12.Checked = true;
                flag = true;
            }
            else
            {
                _radioG12.Checked = false;
                flag = false;
            }

            _radioG12.Rotation = 0;
            _radioG12.Options = TextField.READ_ONLY;
            _radioField112 = _radioG12.CheckField;
            cellform12.CellEvent
                 = new BebanUnitEvent(_checkGroup12, _radioField112, 1);
            headerTable3a.AddCell(cellform12);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountKonfeksi1B.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountKonfeksi1BVB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }

            if (CheckVerified(lastitem.ToUpper()))
            {
                cellHeaderBody.Phrase = new Phrase(lastitem, normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                cellHeaderBody.Phrase = new Phrase("......", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
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

            if (CheckVerified(lastitem.ToUpper()))
            {
                _radioG9.Checked = true;
                flag = true;
            }
            else
            {
                _radioG9.Checked = false;
                flag = false;
            }

            _radioG9.Rotation = 0;
            _radioG9.Options = TextField.READ_ONLY;
            _radioField19 = _radioG9.CheckField;
            cellform9.CellEvent
                 = new BebanUnitEvent(_checkGroup9, _radioField19, 1);
            headerTable3a.AddCell(cellform9);
            //cellHeaderBody.Phrase = new Phrase("", normal_font_8);
            //headerTable3a.AddCell(cellHeaderBody);
            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountOthers.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountOthersVB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8);
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

            if (weightunit.Contains("WEAVING 2"))
            {
                _radioG6.Checked = true;
                flag = true;
            }
            else
            {
                _radioG6.Checked = false;
                flag = false;
            }

            _radioG6.Rotation = 0;
            _radioG6.Options = TextField.READ_ONLY;
            _radioField16 = _radioG6.CheckField;
            cellform6.CellEvent
                 = new BebanUnitEvent(_checkGroup6, _radioField16, 1);
            headerTable3a.AddCell(cellform6);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountWeaving2.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountWeaving2VB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8);
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

            if (weightunit.Contains("KONFEKSI 2A"))
            {
                _radioG3.Checked = true;
                flag = true;
            }
            else
            {
                _radioG3.Checked = false;
                flag = false;
            }

            _radioG3.Rotation = 0;
            _radioG3.Options = TextField.READ_ONLY;
            _radioField13 = _radioG3.CheckField;
            cellform3.CellEvent
                 = new BebanUnitEvent(_checkGroup3, _radioField13, 1);
            headerTable3a.AddCell(cellform3);

            if (flag == false)
            {
                cellHeaderBody.Phrase = new Phrase($"...........", normal_font_8);
                headerTable3a.AddCell(cellHeaderBody);
            }
            else
            {
                var nom = "";

                if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
                {
                    nom = viewModel.AmountKonfeksi2A.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }
                else
                {
                    nom = viewModel.AmountKonfeksi2AVB.GetValueOrDefault().ToString("#,##0.00", new CultureInfo("id-ID"));
                }

                cellHeaderBody.Phrase = new Phrase($"{currencyCode}   {nom}", normal_font_8);
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

            PdfPTable table = new PdfPTable(4)
            {
                WidthPercentage = 100
            };
            float[] widths = new float[] { 1f, 1f, 1f, 1f };
            table.SetWidths(widths);
            PdfPCell cell = new PdfPCell()
            {
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

            cell.Phrase = new Phrase("Menyetujui,", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("Diperiksa,", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("Mengetahui,", normal_font);
            table.AddCell(cell);
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
            }

            cell.Phrase = new Phrase("(..................)", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("(..................)", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("(..................)", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase($"(..................)", normal_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Kasir", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase("Verifikasi", normal_font);
            table.AddCell(cell);
            cell.Phrase = new Phrase($"..................", normal_font);
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

        private bool CheckVerified(string Unit)
        {
            bool res;

            if (Unit == "SPINNING 1" || Unit == "SPINNING 2" || Unit == "SPINNING 3"
                || Unit == "WEAVING 1" || Unit == "WEAVING 2" || Unit == "PRINTING"
                || Unit == "DYEING" || Unit == "KONFEKSI 1A" || Unit == "KONFEKSI 1B"
                || Unit == "KONFEKSI 2A" || Unit == "KONFEKSI 2B" || Unit == "KONFEKSI 2C"
                || Unit == "UMUM")
            {
                res = false;
            }
            else
            {
                res = true;
            }

            return res;
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

        private decimal Convert_Rate(decimal price, string code, double rate)
        {
            double convertCurrency;

            convertCurrency = (double)price;

            return (decimal)convertCurrency;
        }

        private decimal GetPPhValue(RealizationVbNonPOViewModel viewModel)
        {
            decimal val = 0;

            foreach (var itm in viewModel.Items)
            {
                if (itm.isGetPPh == true && itm.IncomeTaxBy == "Supplier")
                {
                    //if (itm.IncomeTax.rate.GetValueOrDefault().Contains("."))
                    //{
                    //    itm.IncomeTax.rate = itm.incomeTax.rate.Replace(".", ",");
                    //}

                    val += itm.Amount.GetValueOrDefault() * ((decimal)itm.IncomeTax.rate.GetValueOrDefault() / 100);
                }
            }

            return val;
        }
    }
}
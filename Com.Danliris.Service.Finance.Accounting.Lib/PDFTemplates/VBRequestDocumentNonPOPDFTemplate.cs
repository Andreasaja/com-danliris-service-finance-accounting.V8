using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
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
    public class VBRequestDocumentNonPOPDFTemplate
    {
        public MemoryStream GeneratePdfTemplate(VBRequestDocumentNonPODto data, int clientTimeZoneOffset)
        {
            const int MARGIN = 20;

            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
            Font beban_unit_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
            Font note_font = FontFactory.GetFont(BaseFont.HELVETICA_OBLIQUE, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_italic_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLDOBLIQUE, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 12);
            Font Title_bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 13);

            Document document = new Document(PageSize.A5.Rotate(), MARGIN, MARGIN, MARGIN, MARGIN);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            string TotalPaidString;


            #region CustomModel

            decimal amount;
            string CurrencyDescription;
            if (data.Currency.Code == "IDR")
            {
                amount = data.Amount.GetValueOrDefault();
                TotalPaidString = NumberToTextIDN.terbilang(Convert.ToDouble(amount));
                CurrencyDescription = "Rupiah";
            }
            else
            {
                amount = data.Amount.GetValueOrDefault();
                TotalPaidString = NumberToTextIDN.terbilang(Convert.ToDouble(amount));

                CurrencyDescription = data.Currency.Description;
                CurrencyDescription = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CurrencyDescription.ToLower());
            }

            #endregion CustomModel

            #region Header

            PdfPTable headerTable_A = new PdfPTable(2);
            PdfPTable headerTable_B = new PdfPTable(1);
            PdfPTable headerTable_C = new PdfPTable(1);
            PdfPTable headerTable1 = new PdfPTable(1);
            PdfPTable headerTable2 = new PdfPTable(1);
            PdfPTable headerTable3 = new PdfPTable(3);
            PdfPTable headerTable3a = new PdfPTable(10);
            PdfPTable headerTable3b = new PdfPTable(4);
            PdfPTable headerTable4 = new PdfPTable(2);

            PdfPTable Title = new PdfPTable(1);
            Title.SetWidths(new float[] { 1f });
            Title.WidthPercentage = 100;

            PdfPTable IdentityTable = new PdfPTable(6);
            IdentityTable.SetWidths(new float[] { 2f, 2f, 2f, 2f, 1f, 3f });
            IdentityTable.WidthPercentage = 100;

            headerTable_A.SetWidths(new float[] { 10f, 10f });
            headerTable_A.WidthPercentage = 100;
            headerTable3.SetWidths(new float[] { 40f, 4f, 100f });
            headerTable3.WidthPercentage = 100;
            headerTable3a.SetWidths(new float[] { 3f, 10f, 3f, 10f, 3f, 10f, 3f, 10f, 3f, 13f });
            headerTable3a.WidthPercentage = 100;
            headerTable3b.SetWidths(new float[] { 1f, 1f, 1f, 1f });
            headerTable3b.WidthPercentage = 100;
            headerTable4.SetWidths(new float[] { 10f, 40f });
            headerTable4.WidthPercentage = 100;

            PdfPCell cellHeader1 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader3 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader4 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader3a = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody3 = new PdfPCell() { Border = Rectangle.NO_BORDER };

            cellHeaderBody.Phrase = new Phrase("Kepada Yth.......", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Kasir PT. Danliris", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Di tempat", normal_font);
            headerTable1.AddCell(cellHeaderBody);

            cellHeader1.AddElement(headerTable1);
            headerTable_A.AddCell(cellHeader1);

            cellHeader2.AddElement(headerTable2);
            headerTable_A.AddCell(cellHeader2);

            document.Add(headerTable_A);

            cellHeaderBody.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeaderBody2.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeaderBody3.HorizontalAlignment = Element.ALIGN_RIGHT;

            cellHeaderBody2.Phrase = new Phrase("PERMOHONAN VB TANPA PO", bold_font);
            Title.AddCell(cellHeaderBody2);

            cellHeaderBody2.Phrase = new Phrase(" ", bold_font);
            Title.AddCell(cellHeaderBody2);

            document.Add(Title);


            cellHeaderBody3.Phrase = new Phrase(" ", normal_font);
            IdentityTable.AddCell(cellHeaderBody3);

            cellHeaderBody3.Phrase = new Phrase(" ", normal_font);
            IdentityTable.AddCell(cellHeaderBody3);

            cellHeaderBody3.Phrase = new Phrase(" ", normal_font);
            IdentityTable.AddCell(cellHeaderBody3);

            cellHeaderBody3.Phrase = new Phrase(" ", normal_font);
            IdentityTable.AddCell(cellHeaderBody3);

            cellHeaderBody.Phrase = new Phrase("No", normal_font);
            IdentityTable.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase($" : {data.DocumentNo}", normal_font);
            IdentityTable.AddCell(cellHeaderBody);


            cellHeaderBody3.Phrase = new Phrase(" ", normal_font);
            IdentityTable.AddCell(cellHeaderBody3);

            cellHeaderBody3.Phrase = new Phrase(" ", normal_font);
            IdentityTable.AddCell(cellHeaderBody3);

            cellHeaderBody3.Phrase = new Phrase(" ", normal_font);
            IdentityTable.AddCell(cellHeaderBody3);

            cellHeaderBody3.Phrase = new Phrase(" ", normal_font);
            IdentityTable.AddCell(cellHeaderBody3);

            cellHeaderBody.Phrase = new Phrase("Tanggal", normal_font);
            IdentityTable.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase($" : {data.Date?.AddHours(clientTimeZoneOffset).ToString("dd/MM/yyyy")}", normal_font);
            IdentityTable.AddCell(cellHeaderBody);

            document.Add(IdentityTable);

            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("VB Uang", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(":", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase($"{data.Currency.Code} " + amount.ToString("#,##0.00", new CultureInfo("id-ID")), normal_font);
            headerTable3.AddCell(cellHeaderBody);


            cellHeaderBody.Phrase = new Phrase("Terbilang", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(":", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(TotalPaidString + " " + CurrencyDescription, normal_font);
            headerTable3.AddCell(cellHeaderBody);


            cellHeaderBody.Phrase = new Phrase("Kegunaan", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(":", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(data.Purpose, normal_font);
            headerTable3.AddCell(cellHeaderBody);


            if (data.IsInklaring) {
                string noBl = (string.IsNullOrEmpty(data.NoBL)) ? "-" : data.NoBL;
                cellHeaderBody.Phrase = new Phrase("No. BL / AWB", normal_font);
                headerTable3.AddCell(cellHeaderBody);
                cellHeaderBody.Phrase = new Phrase(":", normal_font);
                headerTable3.AddCell(cellHeaderBody);
                cellHeaderBody.Phrase = new Phrase(noBl, normal_font);
                headerTable3.AddCell(cellHeaderBody);


                string noPo = (string.IsNullOrEmpty(data.NoPO)) ? "-" : data.NoPO;
                cellHeaderBody.Phrase = new Phrase("No. Kontrak / PO", normal_font);
                headerTable3.AddCell(cellHeaderBody);
                cellHeaderBody.Phrase = new Phrase(":", normal_font);
                headerTable3.AddCell(cellHeaderBody);
                cellHeaderBody.Phrase = new Phrase(noPo, normal_font);
                headerTable3.AddCell(cellHeaderBody);
            }


            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("Beban Unit  :", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            headerTable3.AddCell(cellHeaderBody);


            cellHeader3.AddElement(headerTable3);
            headerTable_B.AddCell(cellHeader3);

            cellHeader4.AddElement(headerTable4);
            headerTable_B.AddCell(cellHeader4);

            document.Add(headerTable_B);
            #endregion Header

            #region NewCheckbox
            //List<PdfFormField> annotations = new List<PdfFormField>();

            var items = data.Items.Where(element => element.IsSelected).OrderBy(element => element.Unit.VBDocumentLayoutOrder).ToList();

            foreach (var item in items)
            {
                //cellHeaderBody.Phrase = new Phrase("", normal_font);
                //cellHeaderBody.PaddingBottom = 5;

                //PdfPCell cellform = new PdfPCell() { Border = Rectangle.NO_BORDER };
                //cellform.FixedHeight = 5f;

                //PdfFormField _checkGroup = PdfFormField.CreateEmpty(writer);
                //RadioCheckField _radioG;
                //PdfFormField _radioField1;
                //Rectangle kotak = new Rectangle(50, 50);
                //_radioG = new RadioCheckField(writer, kotak, "abc", "Yes");
                //_radioG.CheckType = RadioCheckField.TYPE_CHECK;
                //_radioG.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
                //_radioG.BorderColor = BaseColor.Black;
                //_radioG.BorderWidth = BaseField.BORDER_WIDTH_THIN;


                //_radioG.Checked = item.IsSelected;

                //_radioG.Rotation = 90;
                //_radioG.Options = TextField.READ_ONLY;
                //_radioField1 = _radioG.CheckField;

                //cellform.CellEvent
                //= new BebanUnitEvent(_checkGroup, _radioField1, 1);
                //headerTable3a.AddCell(cellform);

                if (string.IsNullOrEmpty(item.Unit.Name))
                {
                    cellHeaderBody.Phrase = new Phrase("- .......", beban_unit_font);
                }
                else
                {

                    string unitName = item.Unit.Name;
                    if (item.Unit.Code == "S2")
                    {
                        unitName = "SPINNING";
                    }
                    cellHeaderBody.Phrase = new Phrase("- " + unitName, beban_unit_font);
                }

                headerTable3b.AddCell(cellHeaderBody);

                //annotations.Add(_checkGroup);
            }

            //cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            //headerTable3a.AddCell(cellHeaderBody);
            //cellHeaderBody.Phrase = new Phrase(" ", normal_font);
            //headerTable3a.AddCell(cellHeaderBody);

            for (var i = 0; i < 4 - (items.Count % 4); i++)
            {
                cellHeaderBody.Phrase = new Phrase(" ", normal_font);
                headerTable3b.AddCell(cellHeaderBody);
            }

            cellHeader3a.AddElement(headerTable3b);
            headerTable_C.AddCell(cellHeader3a);
            document.Add(headerTable_C);

            //foreach (var annotation in annotations)
            //{
            //    writer.AddAnnotation(annotation);
            //}

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

            for (var i = 0; i <= 5; i++) {
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

            cell.Colspan = 2;
            cell.Phrase = new Phrase("Menyetujui,", normal_font);
            table.AddCell(cell);
            //cell.Phrase = new Phrase("", normal_font);
            //table.AddCell(cell);
            cell.Phrase = new Phrase("Mengetahui,", normal_font);
            table.AddCell(cell);
            cell.Colspan = 1;
            cell.Phrase = new Phrase("Diminta Oleh,", normal_font);
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
            cell.Phrase = new Phrase($"({data.CreatedBy})", normal_font);
            table.AddCell(cell);

            //cell.Phrase = new Phrase("Kasir", normal_font);
            //table.AddCell(cell);
            //cell.Phrase = new Phrase("Anggaran", normal_font);
            //table.AddCell(cell);
            //cell.Phrase = new Phrase("..................", normal_font);
            //table.AddCell(cell);
            //cell.Phrase = new Phrase($"{data.SuppliantUnit.Name}", normal_font);
            //table.AddCell(cell);

            document.Add(table);
            #endregion Footer

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

    }
}

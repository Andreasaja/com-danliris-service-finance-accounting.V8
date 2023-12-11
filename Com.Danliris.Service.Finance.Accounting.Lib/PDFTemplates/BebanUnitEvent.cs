using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates
{
    public class BebanUnitEvent : IPdfPCellEvent
    {
        protected PdfFormField parent;
        /** The child field that has to be added */
        protected PdfFormField kid;
        /** The padding of the field inside the cell */
        protected float padding;


        public BebanUnitEvent(PdfFormField parent, PdfFormField kid, float padding)
        {
            this.parent = parent;
            this.kid = kid;
            this.padding = padding;
        }

        /**
         * Add the child field to the parent, and sets the coordinates of the child field.
         */
        public void CellLayout(PdfPCell cell, Rectangle rect, PdfContentByte[] cb)
        {
            parent.AddKid(kid);
            kid.SetWidget(new Rectangle(rect.GetLeft(padding), rect.GetTop(padding + 1f) - 11f, rect.GetRight(padding), rect.GetTop(padding + 1f)), PdfAnnotation.HighlightInvert);
            //kid.SetWidget(new Rectangle(rect.Left, rect.Bottom + size, rect.Width, rect.Height - size), PdfAnnotation.HighlightInvert);

        }

        //public PdfPCell Create_Rectangle(PdfWriter writer)
        //{
        //    PdfPCell cellform = new PdfPCell() { Border = Rectangle.NO_BORDER };
        //    cellform.FixedHeight = 5f;
        //    //initiate form checkbox 
        //    //PdfFormField _checkGroup = PdfFormField.CreateRadioButton(writer, true);
        //    PdfFormField _checkGroup = PdfFormField.CreateEmpty(writer);
        //    RadioCheckField _radioG;
        //    PdfFormField _radioField1;
        //    Rectangle kotak = new Rectangle(100, 100);
        //    _radioG = new RadioCheckField(writer, kotak, "abc", "Yes");
        //    _radioG.CheckType = RadioCheckField.TYPE_CHECK;
        //    _radioG.BorderStyle = PdfBorderDictionary.STYLE_SOLID;
        //    _radioG.BorderColor = BaseColor.Black;
        //    _radioG.BorderWidth = BaseField.BORDER_WIDTH_MEDIUM;
        //    _radioG.Checked = true;
        //    _radioG.Rotation = 90;
        //    _radioG.Options = TextField.READ_ONLY;
        //    _radioField1 = _radioG.CheckField;

        //    cellform.CellEvent
        //         = new BebanUnitEvent(_checkGroup, _radioField1, 1);

        //    return cellform;
        //}
    }
}

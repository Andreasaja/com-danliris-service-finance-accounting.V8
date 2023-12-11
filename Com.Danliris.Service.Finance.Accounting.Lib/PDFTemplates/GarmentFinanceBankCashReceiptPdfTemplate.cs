using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Sales.Lib.Utilities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates
{
    public class GarmentFinanceBankCashReceiptPdfTemplate
    {
        public MemoryStream GeneratePdfTemplate(BankCashReceiptViewModel viewModel, int clientTimeZoneOffset)
        {
            const int MARGIN = 8;

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
            string TotalPaidString = "";
            if (viewModel != null)
            {
                if(viewModel.Currency.Code == "IDR")
                {
                    //if (viewModel.Amount.ToString().EndsWith(",00")){
                        TotalPaidString = Terbilang((double)viewModel.Amount, "IDR");
                    //}
                    //else
                    //{
                      //  TotalPaidString = NumberToTextIDN.terbilang((double)viewModel.Amount) + " " + NumberToTextIDN.terbilangKoma((double)viewModel.Amount) + " Rupiah";
                        //TotalPaidString = TotalPaidString.Replace("koma", "");
                    //}
                    
                } else
                {
                    //if (viewModel.Amount.ToString().EndsWith(",00"))
                    //{
                    //    TotalPaidString = NumberToTextIDN.terbilang((double)viewModel.Amount) + " Dollar";
                    //}
                    //else
                    //{
                        TotalPaidString = Terbilang((double)viewModel.Amount, "USD");
                        TotalPaidString = TotalPaidString.Replace("koma", "");
                    //}
                }
            }
            var arrayRemarks = new List<string>();
            foreach(var item in viewModel.Items)
            {
                arrayRemarks.Add(item.Remarks);
            }

            string payment = viewModel.BankCashReceiptType.Name=="PENJUALAN EKSPOR" || viewModel.BankCashReceiptType.Name == "PENJUALAN LOKAL" ? viewModel.Buyer.Name : String.Join(", ", arrayRemarks);


            #endregion CustomModel

            #region Header

            PdfPTable headerTable_A = new PdfPTable(1);
            PdfPTable headerTable_B = new PdfPTable(1);
            PdfPTable headerTable1 = new PdfPTable(1);
            PdfPTable headerTable2 = new PdfPTable(1);
            PdfPTable headerTable3 = new PdfPTable(3);
            PdfPTable headerTable4 = new PdfPTable(2);
            PdfPTable headerTitle = new PdfPTable(1);
            PdfPTable headerReceiptNo = new PdfPTable(1);
            headerTable_A.SetWidths(new float[] { 15f });
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
            PdfPCell cellMoney = new PdfPCell() { Border = Rectangle.BOX };
            cellMoney.FixedHeight = 50f;

            cellHeaderBody.Phrase = new Phrase("PT. DAN LIRIS", Title_bold_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Kel. Banaran, Kec. Grogol, Kab. Dati II Sukoharjo", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("PO BOX 166 Solo - 57100 Jawa Tengah Indonesia", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Telp. 0271-714400, Fax. 0271-717178", normal_font);
            headerTable1.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable1.AddCell(cellHeaderBody);

            cellHeader1.AddElement(headerTable1);
            headerTable_A.AddCell(cellHeader1);

            document.Add(headerTable_A);

            cellHeaderBody.HorizontalAlignment = Element.ALIGN_CENTER;

            cellHeaderBody.Phrase = new Phrase("K W I T A N S I ", header_font);
            headerTitle.AddCell(cellHeaderBody);

            document.Add(headerTitle);

            cellHeaderBody.HorizontalAlignment = Element.ALIGN_RIGHT;

            cellHeaderBody.Phrase = new Phrase(viewModel?.ReceiptNo, header_font);
            headerReceiptNo.AddCell(cellHeaderBody);

            document.Add(headerReceiptNo);

            cellHeaderBody.HorizontalAlignment = Element.ALIGN_LEFT;

            cellHeaderBody.Phrase = new Phrase("BANK ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(":", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(viewModel?.Bank.BankName + " - " + viewModel?.Bank.AccountNumber, normal_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("TELAH TERIMA DARI ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(":", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(payment, normal_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("BANYAKNYA UANG ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(":", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellMoney.Phrase = new Phrase(TotalPaidString.ToUpper(), normal_font);
            headerTable3.AddCell(cellMoney);

            cellHeaderBody.Phrase = new Phrase("UNTUK PEMBAYARAN ", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(":", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(viewModel?.Remarks, normal_font);
            headerTable3.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font);
            headerTable3.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("", normal_font);
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
            footerTable2.SetWidths(new float[] { 30f, 80f });
            footerTable2.WidthPercentage = 100;

            PdfPCell cellFooterLeft1 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellFooterLeft2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderFooter = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellTerbilang = new PdfPCell() { Border = Rectangle.NO_BORDER };



            cellHeaderFooter.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTerbilang.HorizontalAlignment = Element.ALIGN_CENTER;

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
            cellTerbilang.Border = Rectangle.BOX;
            cellTerbilang.FixedHeight = 30f;
            cellTerbilang.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellTerbilang.Phrase = new Phrase("T E R B I L A N G : " + viewModel?.Currency.Code + " " + viewModel?.Amount.ToString("#,##0.00", new CultureInfo("id-ID")), bold_font);
            footerTable1.AddCell(cellTerbilang);
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
            cellHeaderFooter.Phrase = new Phrase("", note_font);
            footerTable1.AddCell(cellHeaderFooter);

            cellFooterLeft1.AddElement(footerTable1);
            footerTable.AddCell(cellFooterLeft1);

            cellHeaderFooter.Phrase = new Phrase("", normal_font);
            footerTable2.AddCell(cellHeaderFooter);
            cellHeaderFooter.Phrase = new Phrase("SOLO, " + viewModel?.ReceiptDate.AddHours(clientTimeZoneOffset).ToString("dd MMMM yyyy", new CultureInfo("id-ID")), normal_font);
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
            cellHeaderFooter.Phrase = new Phrase("Dicetak Tanggal :" + DateTimeOffset.Now.AddHours(clientTimeZoneOffset).ToString("dd MMMM yyyy / HH:mm:ss", new CultureInfo("id-ID")), normal_font);
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
        string[] satuan = { "Nol", "Satu", "Dua", "Tiga", "Empat", "Lima", "Enam", "Tujuh", "Delapan", "Sembilan" };
        string[] belasan = { "Sepuluh", "Sebelas", "Dua Belas", "Tiga Belas", "Empat Belas", "Lima Belas", "Enam Belas", "Tujuh Belas", "Delapan Belas", "Sembilan Belas" };
        string[] puluhan = { "", "", "Dua Puluh", "Tiga Puluh", "Empat Puluh", "Lima Puluh", "Enam Puluh", "Tujuh Puluh", "Delapan Puluh", "Sembilan Puluh" };
        string[] ribuan = { "", "ribu", "juta", "milyar", "triliyun", "kuadrilyun", "kuintiliun", "sekstiliun", "septiliun", "oktiliun", "noniliun", "desiliun" };
        public string Terbilang(double d, string code)
        {
            

            var strHasil = "";
            var isNegative = (d) < 0;
            d = Convert.ToDouble((Math.Round(d * 100) / 100).ToString("N2"));
            if (isNegative)
            {
                d = d * -1;
            }

            if (d.ToString().IndexOf(".") > -1 || d.ToString().IndexOf(",") > -1)
            {
                var a = 0.00;
                if (d.ToString().IndexOf(",") > -1)
                {
                    a = Convert.ToDouble((d.ToString().Substring(d.ToString().IndexOf(",") + 1)));
                }

                if (d.ToString().IndexOf(".") > -1)
                {
                    a = Convert.ToDouble((d.ToString().Substring(d.ToString().IndexOf(".") + 1)));
                }

                if (a != 0)
                {
                    string tambahan = "";
                    if (code != "IDR")
                    {
                        tambahan = " Sen";

                        strHasil = " Dollar " + TerbilangKoma(d) + tambahan;
                    }
                    else
                    {
                        strHasil = TerbilangKoma(d) + " Rupiah";
                    }
                }
                if (d.ToString().IndexOf(",") > -1)
                {
                    d = Convert.ToDouble(d.ToString().Substring(0, d.ToString().IndexOf(",")));
                }

                if (d.ToString().IndexOf(".") > -1)
                {
                    d = Convert.ToDouble(d.ToString().Substring(0, d.ToString().IndexOf(".")));
                }

            } else
            {
                string tambahan = "";
                if (code != "IDR")
                {

                    strHasil = strHasil + " Dollar";
                }
                else
                {
                    strHasil = strHasil + " Rupiah";
                }
            }

            var nDigit = 0;
            var nPosisi = 0;

            var strTemp = Math.Truncate(d).ToString();
            for (var i = strTemp.Length; i > 0; i--)
            {
                var tmpBuff = "";
                nDigit = Convert.ToInt32(strTemp.Substring(i - 1, 1), 10);
                nPosisi = (strTemp.Length - i) + 1;
                switch (nPosisi % 3)
                {
                    case 1:
                        var bAllZeros = false;
                        if (i == 1)
                            tmpBuff = satuan[nDigit] + " ";
                        else if (strTemp.Substring(i - 2, 1) == "1")
                            tmpBuff = belasan[nDigit] + " ";
                        else if (nDigit > 0)
                            tmpBuff = satuan[nDigit] + " ";
                        else
                        {
                            bAllZeros = true;
                            if (i > 1)
                                if (strTemp.Substring(i - 2, 1) != "0")
                                    bAllZeros = false;
                            if (i > 2)
                                if (strTemp.Substring(i - 3, 1) != "0")
                                    bAllZeros = false;
                            tmpBuff = "";
                        }

                        if ((!bAllZeros) && (nPosisi > 1))
                            if ((strTemp.Length == 4) && (strTemp.Substring(0, 1) == "1"))
                                tmpBuff = "Se" + ribuan[Convert.ToInt32(Math.Round(Convert.ToDecimal(nPosisi) / 3))] + " ";
                            else
                                tmpBuff = tmpBuff + ribuan[Convert.ToInt32(Math.Round(Convert.ToDecimal(nPosisi) / 3))] + " ";
                        strHasil = tmpBuff + strHasil;
                        break;
                    case 2:
                        if (nDigit > 0)
                            strHasil = (puluhan[nDigit] + " " + strHasil).Trim();
                        break;
                    case 0:
                        if (nDigit > 0)
                            if (nDigit == 1)
                                strHasil = "Seratus " + strHasil;
                            else
                                strHasil = satuan[nDigit] + " Ratus " + strHasil;
                        break;
                }

            }

            

            if (strTemp.Length > 0)
            {
                if (isNegative)
                {
                    strHasil = $"minus { strHasil}";
                }
            }

            strHasil = strHasil.Trim().ToLower();
            if (strHasil.Length > 0)
            {
                strHasil = strHasil.Substring(0, 1).ToUpper() +
                    strHasil.Substring(1, strHasil.Length - 1);
            }
            
            return strHasil; 

        }

        public string TerbilangKoma(double frac)
        {
            var stringFrac = frac.ToString();
            var a = "";

            if (stringFrac.IndexOf(".") > -1)
            {
                a = frac.ToString().Substring(frac.ToString().IndexOf(".") + 1);
            }
            else if (stringFrac.IndexOf(",") > -1)
            {
                a = frac.ToString().Substring(frac.ToString().IndexOf(",") + 1);
            }

            string fixNumber = "";
            if (a.Length > 4)
            {
                if (stringFrac.IndexOf(".") > -1)
                {
                    fixNumber = (frac.ToString("N4")).ToString().Substring((frac.ToString("N4")).ToString().IndexOf(".") + 1);
                }
                else if (stringFrac.IndexOf(",") > -1)
                {
                    fixNumber = (frac.ToString("N4")).ToString().Substring((frac.ToString("N4")).ToString().IndexOf(",") + 1);
                }
            }
            else
            {
                fixNumber = a;
            }

            var strHasil = "koma";
            for (var i = 0; i < fixNumber.Length; i++)
            {
                var temp = Convert.ToInt32(fixNumber[i].ToString());
                strHasil = strHasil + " " + satuan[temp];
            }

            return strHasil;
        }
    }

    
}

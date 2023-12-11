using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow.PdfGenerator
{
    public static class CashflowDivisionPdfGenerator
    {
        private static readonly Font _headerFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 11);
        private static readonly Font _subHeaderFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
        private static readonly Font _normalFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _normalBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _normalBoldWhiteFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9, 0, BaseColor.White);
        private static readonly Font _smallBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _smallerBoldWhiteFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7, 0, BaseColor.White);

        public static MemoryStream Generate(DivisionDto division, DateTimeOffset dueDate, int offset, BudgetCashflowDivision data)
        {
            var document = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
            var stream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            SetTitle(document, division, dueDate, offset);
            SetTable(document, data, division, dueDate, offset);

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private static void SetTable(Document document, BudgetCashflowDivision data, DivisionDto division, DateTimeOffset dueDate, int offset)
        {
            var cellRotate = new PdfPCell()
            {
                Border = Element.RECTANGLE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_TOP,
                Rotation = 90
            };

            var cellCenter = new PdfPCell()
            {
                Border = Element.RECTANGLE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            var cellLeft = new PdfPCell()
            {
                Border = Element.RECTANGLE,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            var cellRight = new PdfPCell()
            {
                Border = Element.RECTANGLE,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            var dynamicHeadersPerPage = 2;
            var splittedHeaders = data.Headers.Select((header, index) => data.Headers.Skip(index * dynamicHeadersPerPage).Take(dynamicHeadersPerPage).ToList()).Where(header => header.Any()).ToList();

            var isFirstPage = true;
            var loopedHeaders = 0;
            var loopedChildHeaders = 0;
            var isLastPage = false;
            var loopedCounter = 0;
            foreach (var headers in splittedHeaders)
            {
                loopedCounter += headers.Count * 3;
                if (loopedCounter == data.Headers.Count * 3)
                    isLastPage = true;

                if (isFirstPage)
                {
                    var dynamicColumns = headers.Count * 3;
                    var columns = 5 + dynamicColumns;

                    if (isLastPage)
                        columns += 1;

                    var table = new PdfPTable(columns)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };

                    var widths = new List<float>() { 2f, 2f, 1f, 20f, 4f };

                    foreach (var header in headers)
                    {
                        widths.Add(8f);
                        widths.Add(8f);
                        widths.Add(8f);
                    }

                    if (isLastPage)
                        widths.Add(8f);

                    table.SetWidths(widths.ToArray());

                    cellCenter.Rowspan = 2;
                    cellCenter.Colspan = 4;
                    cellCenter.Phrase = new Phrase("KETERANGAN", _smallBoldFont);
                    table.AddCell(cellCenter);
                    cellCenter.Colspan = 1;
                    cellCenter.Phrase = new Phrase("MATA UANG", _smallBoldFont);
                    table.AddCell(cellCenter);

                    foreach (var header in headers)
                    {
                        cellCenter.Rowspan = 1;
                        cellCenter.Colspan = 3;
                        cellCenter.Phrase = new Phrase(header, _smallBoldFont);
                        table.AddCell(cellCenter);
                    }

                    if (isLastPage)
                    {
                        cellCenter.Rowspan = 2;
                        cellCenter.Colspan = 1;
                        cellCenter.Phrase = new Phrase("TOTAL", _smallBoldFont);
                        table.AddCell(cellCenter);
                    }

                    foreach (var header in headers)
                    {
                        cellCenter.Rowspan = 1;
                        cellCenter.Colspan = 1;

                        cellCenter.Phrase = new Phrase("NOMINAL VALAS", _smallBoldFont);
                        table.AddCell(cellCenter);
                        cellCenter.Phrase = new Phrase("NOMINAL IDR", _smallBoldFont);
                        table.AddCell(cellCenter);
                        cellCenter.Phrase = new Phrase("ACTUAL", _smallBoldFont);
                        table.AddCell(cellCenter);
                    }

                    foreach (var item in data.Items)
                    {
                        if (item.IsUseSection)
                        {
                            cellRotate.Rowspan = item.SectionRows > 0 ? item.SectionRows : 1;
                            cellRotate.Phrase = new Phrase(item.CashflowType.Name, _smallBoldFont);
                            table.AddCell(cellRotate);
                        }

                        if (item.IsUseGroup)
                        {
                            cellRotate.Rowspan = item.GroupRows > 0 ? item.GroupRows : 1;
                            cellRotate.Phrase = new Phrase(item.TypeName, _smallBoldFont);
                            table.AddCell(cellRotate);
                        }

                        if (item.IsLabelOnly)
                        {
                            var labelOnlyColspan = 3 + dynamicColumns;
                            if (isLastPage)
                                labelOnlyColspan += 1;
                            cellLeft.Colspan = labelOnlyColspan;
                            cellLeft.Phrase = new Phrase(item.CashflowCategory.Name, _smallBoldFont);
                            table.AddCell(cellLeft);
                        }

                        if (item.IsSubCategory)
                        {
                            cellLeft.Colspan = 1;
                            cellLeft.Phrase = new Phrase("", _smallFont);
                            table.AddCell(cellLeft);

                            if (item.IsShowSubCategoryLabel)
                                cellLeft.Phrase = new Phrase(item.CashflowSubCategory.Name, _smallFont);
                            else
                                cellLeft.Phrase = new Phrase("", _smallFont);
                            table.AddCell(cellLeft);

                            cellCenter.Colspan = 1;
                            cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                            table.AddCell(cellCenter);

                            foreach (var subCategoryItem in item.Items.Skip(loopedChildHeaders).Take(headers.Count))
                            {
                                cellRight.Colspan = 1;
                                cellRight.Phrase = new Phrase(subCategoryItem.CurrencyNominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Nominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Actual.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }

                            if (isLastPage)
                            {
                                cellRight.Phrase = new Phrase(item.DivisionActualTotal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }
                        }

                        if (item.IsSummary)
                        {
                            cellLeft.Colspan = 2;
                            if (item.IsShowSummaryLabel)
                                cellLeft.Phrase = new Phrase(item.SummaryLabel, _smallBoldFont);
                            else
                                cellLeft.Phrase = new Phrase("", _smallFont);
                            table.AddCell(cellLeft);

                            cellCenter.Colspan = 1;
                            cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                            table.AddCell(cellCenter);

                            foreach (var subCategoryItem in item.Items.Skip(loopedChildHeaders).Take(headers.Count))
                            {
                                cellRight.Colspan = 1;
                                cellRight.Phrase = new Phrase(subCategoryItem.CurrencyNominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Nominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Actual.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }

                            if (isLastPage)
                            {
                                cellRight.Phrase = new Phrase(item.DivisionActualTotal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }
                        }

                        if (item.IsDifference)
                        {
                            cellLeft.Colspan = 3;
                            if (item.IsShowDifferenceLabel)
                                cellLeft.Phrase = new Phrase(item.DifferenceLabel, _smallBoldFont);
                            else
                                cellLeft.Phrase = new Phrase("", _smallFont);
                            table.AddCell(cellLeft);

                            cellCenter.Colspan = 1;
                            cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                            table.AddCell(cellCenter);

                            foreach (var subCategoryItem in item.Items.Skip(loopedChildHeaders).Take(headers.Count))
                            {
                                cellRight.Colspan = 1;
                                cellRight.Phrase = new Phrase(subCategoryItem.CurrencyNominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Nominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Actual.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }

                            if (isLastPage)
                            {
                                cellRight.Phrase = new Phrase(item.DivisionActualTotal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }
                        }

                        if (item.IsGeneralSummary)
                        {
                            cellLeft.Colspan = 4;
                            if (item.IsShowGeneralSummaryLabel)
                                cellLeft.Phrase = new Phrase(item.GeneralSummaryLabel, _smallBoldFont);
                            else
                                cellLeft.Phrase = new Phrase("", _smallFont);
                            table.AddCell(cellLeft);

                            cellCenter.Colspan = 1;
                            cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                            table.AddCell(cellCenter);

                            foreach (var subCategoryItem in item.Items.Skip(loopedChildHeaders).Take(headers.Count))
                            {
                                cellRight.Colspan = 1;
                                cellRight.Phrase = new Phrase(subCategoryItem.CurrencyNominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Nominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Actual.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }

                            if (isLastPage)
                            {
                                cellRight.Phrase = new Phrase(item.DivisionActualTotal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }
                        }

                        if (item.IsCurrencyRate)
                        {
                            cellLeft.Colspan = 4;
                            if (item.IsShowCurrencyLabel)
                                cellLeft.Phrase = new Phrase(item.CurrencyRateLabel, _smallBoldFont);
                            else
                                cellLeft.Phrase = new Phrase("", _smallFont);
                            table.AddCell(cellLeft);

                            cellCenter.Colspan = 1;
                            cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                            table.AddCell(cellCenter);

                            cellRight.Colspan = 1;
                            cellRight.Phrase = new Phrase(item.Currency == null ? "" : item.Currency.Rate.ToString("#,##0.00"), _smallFont);
                            table.AddCell(cellRight);
                            var currencyRateColumns = dynamicColumns;
                            if (isLastPage)
                                currencyRateColumns += 1;
                            cellRight.Colspan = currencyRateColumns;
                            cellRight.Phrase = new Phrase("", _smallFont);
                            table.AddCell(cellRight);
                        }

                        if (item.IsEquivalent)
                        {
                            cellLeft.Colspan = 4;
                            cellLeft.Phrase = new Phrase("Total Surplus (Defisit) Equivalent", _smallBoldFont);
                            table.AddCell(cellLeft);

                            cellCenter.Colspan = 1;
                            cellCenter.Phrase = new Phrase("IDR", _smallFont);
                            table.AddCell(cellCenter);
                            //cellRight.Colspan = 1;
                            //cellRight.Phrase = new Phrase(item.Equivalent.ToString("#,##0.00"), _smallFont);
                            //table.AddCell(cellRight);
                            var equivalentColumns = dynamicColumns;
                            //if (isLastPage)
                            //    equivalentColumns += 1;
                            cellRight.Colspan = equivalentColumns;
                            cellRight.Phrase = new Phrase("", _smallFont);
                            table.AddCell(cellRight);

                            if (isLastPage)
                            {
                                cellRight.Colspan = 1;
                                cellRight.Phrase = new Phrase(item.Equivalent.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }
                        }
                    }

                    document.Add(table);
                }
                else
                {
                    document.NewPage();
                    SetTitle(document, division, dueDate, offset);
                    var dynamicColumns = headers.Count * 3;
                    var columns = dynamicColumns;

                    if (isLastPage)
                        columns += 1;

                    var table = new PdfPTable(columns)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };

                    var widths = new List<float>();

                    foreach (var header in headers)
                    {
                        widths.Add(8f);
                        widths.Add(8f);
                        widths.Add(8f);
                    }

                    if (isLastPage)
                        widths.Add(8f);

                    table.SetWidths(widths.ToArray());

                    foreach (var header in headers)
                    {
                        cellCenter.Rowspan = 1;
                        cellCenter.Colspan = 3;
                        cellCenter.Phrase = new Phrase(header, _smallBoldFont);
                        table.AddCell(cellCenter);
                    }

                    if (isLastPage)
                    {
                        cellCenter.Rowspan = 2;
                        cellCenter.Colspan = 1;
                        cellCenter.Phrase = new Phrase("TOTAL", _smallBoldFont);
                        table.AddCell(cellCenter);
                    }

                    foreach (var header in headers)
                    {
                        cellCenter.Rowspan = 1;
                        cellCenter.Colspan = 1;

                        cellCenter.Phrase = new Phrase("NOMINAL VALAS", _smallBoldFont);
                        table.AddCell(cellCenter);
                        cellCenter.Phrase = new Phrase("NOMINAL IDR", _smallBoldFont);
                        table.AddCell(cellCenter);
                        cellCenter.Phrase = new Phrase("ACTUAL", _smallBoldFont);
                        table.AddCell(cellCenter);
                    }

                    foreach (var item in data.Items)
                    {
                        if (item.IsLabelOnly)
                        {
                            cellLeft.Colspan = columns;
                            cellLeft.Phrase = new Phrase("\n", _smallBoldFont);
                            table.AddCell(cellLeft);
                        }

                        if (item.IsSubCategory)
                        {
                            foreach (var subCategoryItem in item.Items.Skip(loopedChildHeaders).Take(headers.Count))
                            {
                                cellRight.Colspan = 1;
                                cellRight.Phrase = new Phrase(subCategoryItem.CurrencyNominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Nominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Actual.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }

                            if (isLastPage)
                            {
                                cellRight.Phrase = new Phrase(item.DivisionActualTotal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }

                        }

                        if (item.IsSummary)
                        {
                            foreach (var subCategoryItem in item.Items.Skip(loopedChildHeaders).Take(headers.Count))
                            {
                                cellRight.Colspan = 1;
                                cellRight.Phrase = new Phrase(subCategoryItem.CurrencyNominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Nominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Actual.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }

                            if (isLastPage)
                            {
                                cellRight.Phrase = new Phrase(item.DivisionActualTotal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }
                        }

                        if (item.IsDifference)
                        {
                            foreach (var subCategoryItem in item.Items.Skip(loopedChildHeaders).Take(headers.Count))
                            {
                                cellRight.Colspan = 1;
                                cellRight.Phrase = new Phrase(subCategoryItem.CurrencyNominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Nominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Actual.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }

                            if (isLastPage)
                            {
                                cellRight.Phrase = new Phrase(item.DivisionActualTotal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }
                        }

                        if (item.IsGeneralSummary)
                        {
                            foreach (var subCategoryItem in item.Items.Skip(loopedChildHeaders).Take(headers.Count))
                            {
                                cellRight.Colspan = 1;
                                cellRight.Phrase = new Phrase(subCategoryItem.CurrencyNominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Nominal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Phrase = new Phrase(subCategoryItem.Actual.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }

                            if (isLastPage)
                            {
                                cellRight.Phrase = new Phrase(item.DivisionActualTotal.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }
                        }

                        if (item.IsCurrencyRate)
                        {
                            cellRight.Colspan = columns;
                            cellRight.Phrase = new Phrase("\n", _smallFont);
                            table.AddCell(cellRight);
                        }

                        if (item.IsEquivalent)
                        {
                            if (!isLastPage)
                            {
                                cellRight.Colspan = columns;
                                cellRight.Phrase = new Phrase("\n", _smallFont);
                                table.AddCell(cellRight);
                            }
                            else
                            {
                                cellRight.Colspan = columns - 1;
                                cellRight.Phrase = new Phrase("\n", _smallFont);
                                table.AddCell(cellRight);
                                cellRight.Colspan = 1;
                                cellRight.Phrase = new Phrase(item.Equivalent.ToString("#,##0.00"), _smallFont);
                                table.AddCell(cellRight);
                            }

                        }
                    }

                    document.Add(table);
                }

                isFirstPage = false;
                loopedHeaders += headers.Count;
                loopedChildHeaders += headers.Count;
            }



        }

        private static void SetTitle(Document document, DivisionDto division, DateTimeOffset dueDate, int offset)
        {
            var company = "PT DAN LIRIS";
            var title = "LAPORAN BUDGET CASHFLOW";
            var divisionName = "DIVISI: ";
            if (division != null)
                divisionName += division.Name;
            else
                divisionName = "SEMUA DIVISI";

            var cultureInfo = new CultureInfo("id-ID");
            var date = $"PERIODE {dueDate.AddMonths(1).AddHours(offset).DateTime.ToString("MMMM yyyy", cultureInfo)}";

            var table = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = Element.ALIGN_LEFT
            };

            var cell = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Phrase = new Phrase(company, _headerFont),
            };
            table.AddCell(cell);

            cell.Phrase = new Phrase(title, _headerFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase(divisionName, _headerFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase(date, _headerFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("\n", _headerFont);
            table.AddCell(cell);

            document.Add(table);
        }
    }
}

using Com.Danliris.Service.Finance.Accounting.Lib.Models.SalesReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseInterface;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.SalesReceipt;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.SalesReceipt
{
    public interface ISalesReceiptService : IBaseService<SalesReceiptModel>
    {
        List<SalesInvoiceReportSalesReceiptViewModel> GetSalesInvoice(SalesInvoicePostForm dataForm);
        List<SalesReceiptReportViewModel> GetReport(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet);
        MemoryStream GenerateExcel(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet);
    }
}

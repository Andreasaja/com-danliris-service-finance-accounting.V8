using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNotVerifiedReportViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PaymentDispositionNotVerifiedReport
{
    public interface IPaymentDispositionNotVerifiedReport
    {
        MemoryStream GenerateExcel(string no, string supplier, string division, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offset, string type);
        Tuple<List<PaymentDispositionNotVerifiedReportViewModel>, int> GetReport(string no, string supplier, string division, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int page, int size, string Order, int offset, string type);
    }
}

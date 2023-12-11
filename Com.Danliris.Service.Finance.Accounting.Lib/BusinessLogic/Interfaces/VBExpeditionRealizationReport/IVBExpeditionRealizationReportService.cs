using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBExpeditionRealizationReport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.VBExpeditionRealizationReport
{
    public interface IVBExpeditionRealizationReportService
    {
        Task<List<VBExpeditionRealizationReportViewModel>> GetReport(int vbRequestId, int vbRealizeId, string ApplicantName, int unitId, int divisionId, string isVerified, DateTimeOffset? realizeDateFrom, DateTimeOffset? realizeDateTo, int offSet);
        Task<MemoryStream> GenerateExcel(int vbRequestId, int vbRealizeId, string ApplicantName, int unitId, int divisionId, string isVerified, DateTimeOffset? realizeDateFrom, DateTimeOffset? realizeDateTo, int offSet);
    }
}

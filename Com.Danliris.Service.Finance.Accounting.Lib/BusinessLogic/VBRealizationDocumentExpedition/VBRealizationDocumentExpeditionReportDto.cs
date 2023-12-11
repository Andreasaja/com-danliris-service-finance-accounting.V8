using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocumentExpedition;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition
{
    public class VBRealizationDocumentExpeditionReportDto
    {
        public VBRealizationDocumentExpeditionReportDto(List<ReportDto> data, int total, int size, int page)
        {
            Data = data;
            Total = total;
            Size = size;
            Page = page;
        }

        public IList<ReportDto> Data { get; private set; }
        public int Total { get; private set; }
        public int Size { get; private set; }
        public int Page { get; private set; }
    }
}
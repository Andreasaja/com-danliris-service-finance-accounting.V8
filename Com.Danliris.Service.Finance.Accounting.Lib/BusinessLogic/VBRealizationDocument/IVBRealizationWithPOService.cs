using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument
{
    public interface IVBRealizationWithPOService
    {
        int Create(FormDto form);
        int Update(int id, FormDto form);
        int Delete(int id);
        ReadResponse<VBRealizationDocumentModel> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        ReadResponse<VBRealizationDocumentModel> ReadByUser(int page, int size, string order, List<string> select, string keyword, string filter);
        VBRealizationWithPODto ReadById(int id);
        VBRealizationPdfDto ReadModelById(int id);
    }
}

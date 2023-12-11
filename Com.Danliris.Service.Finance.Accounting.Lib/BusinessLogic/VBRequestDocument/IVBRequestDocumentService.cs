using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public interface  IVBRequestDocumentService
    {
        Task<int> CreateNonPO(VBRequestDocumentNonPOFormDto form);
        Task<int> UpdateNonPO(int id, VBRequestDocumentNonPOFormDto form);
        Task<VBRequestDocumentNonPODto> GetNonPOById(int id);
        Task<int> DeleteNonPO(int id);

        int CreateWithPO(VBRequestDocumentWithPOFormDto form);
        int UpdateWithPO(int id, VBRequestDocumentWithPOFormDto form);
        VBRequestDocumentWithPODto GetWithPOById(int id);
        int DeleteWithPO(int id);

        List<VBRequestDocumentModel> GetNotApprovedData(int type, int vbId, int suppliantUnitId, DateTime? date, string order);

        ReadResponse<VBRequestDocumentModel> Get(int page, int size, string order, List<string> select, string keyword, string filter);

        ReadResponse<VBRequestDocumentModel> GetByUser(int page, int size, string order, List<string> select, string keyword, string filter);

        Task<int> ApprovalData(ApprovalVBFormDto data);
        Task<int> CancellationDocuments(CancellationFormDto form);
        bool GetVBForPurchasing(int id);
    }
}

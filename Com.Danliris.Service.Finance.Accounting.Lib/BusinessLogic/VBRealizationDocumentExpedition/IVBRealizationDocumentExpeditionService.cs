using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition
{
    public interface IVBRealizationDocumentExpeditionService
    {
        Task<int> InitializeExpedition(int vbRealizationId);
        Task<int> UpdateExpeditionByRealizationId(int vbRealizationId);
        // Done Enhancement
        Task<int> SubmitToVerification(List<int> vbRealizationIds);
        // Done Enhancement
        Task<int> VerificationDocumentReceipt(List<int> vbRealizationIds);
        // Done Enhancement
        Task<int> VerifiedToCashier(List<int> vbRealizationIds);
        // Done Enhancement
        Task<int> VerifiedToCashier(int vbRealizationId);
        // Done Enhancement
        Task<int> CashierReceipt(List<int> vbRealizationIds);
        // Done Enhancement
        Task<int> Reject(int vbRealizationId, string reason);
        Task<int> CashierDelete(int vbRealizationId);
        ReadResponse<VBRealizationDocumentExpeditionModel> Read(int page, int size, string order, string keyword, VBRealizationPosition position, int vbId, int vbRealizationId, DateTimeOffset? realizationDate, string vbRealizationRequestPerson, int unitId);
        ReadResponse<VBRealizationDocumentModel> ReadRealizationToVerification(int vbId, int vbRealizationId, DateTimeOffset? realizationDate, string vbRealizationRequestPerson, int unitId);
        Task<VBRealizationDocumentExpeditionReportDto> GetReports(int vbId, int vbRealizationId, string vbRequestName, int unitId, int divisionId, DateTimeOffset dateStart, DateTimeOffset dateEnd, string status, int page = 1, int size = 25);
        ReadResponse<VBRealizationDocumentExpeditionModel> ReadVerification(int page, int size, string order, string keyword, VBRealizationPosition position, int vbId, int vbRealizationId, DateTimeOffset? realizationDate, string vbRealizationRequestPerson, int unitId);
        Task<VBRequestDocumentModel> ReadByIdAsync(long id); 
        Task<int> ClearanceVBPost(List<ClearancePostId> listId);
        Task<int> ClearanceVBPost(ClearanceFormDto form);
    }
}

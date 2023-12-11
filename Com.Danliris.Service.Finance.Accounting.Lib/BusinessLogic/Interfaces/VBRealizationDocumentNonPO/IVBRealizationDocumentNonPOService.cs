using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseInterface;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.VBRealizationDocumentNonPO
{
    public interface IVBRealizationDocumentNonPOService
    {
        ReadResponse<VBRealizationDocumentModel> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        ReadResponse<VBRealizationDocumentModel> ReadByUser(int page, int size, string order, List<string> select, string keyword, string filter);
        Task<int> CreateAsync(VBRealizationDocumentNonPOViewModel model);
        Task<VBRealizationDocumentNonPOViewModel> ReadByIdAsync(int id);
        Task<int> UpdateAsync(int id, VBRealizationDocumentNonPOViewModel model);
        Task<int> DeleteAsync(int id);
    }
}

using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.ClearaceVB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.ClearaceVB
{
    public interface IClearaceVBService
    {
        ReadResponse<ClearaceVBViewModel> Read(int page, int size, string order, List<string> select, string keyword, string filter);
        
        Task<VBRequestDocumentModel> ReadByIdAsync(long id);
        Task<int> ClearanceVBPost(List<ClearencePostId> listId);
        Task<int> ClearanceVBPost(ClearenceFormDto form);
        Task<int> ClearanceVBUnpost(long id);
    }
}

using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoTextile
{
    public interface IPurchasingMemoTextileService
    {
        int Create(FormDto form);
        ReadResponse<IndexDto> Read(string keyword, int page = 1, int size = 25);
        PurchasingTextileDto Read(int id);
        int Update(int id, FormDto form);
        int Delete(int id);
        Task<int> Posting(PostingFormDto form);
    }
}

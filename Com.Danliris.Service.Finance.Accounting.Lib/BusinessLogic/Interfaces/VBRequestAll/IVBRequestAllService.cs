using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRequestAll;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.VBRequestAll
{
    public interface IVBRequestAllService
    {
        ReadResponse<VBRequestAllViewModel> Read(int page, int size, string order, List<string> select, string keyword, string filter);
    }
}

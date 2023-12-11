using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Services.BasicUploadCsvService
{
    public interface IBasicUploadCsvService<TViewModel>
    {
        Tuple<bool, List<object>> UploadValidate(ref List<TViewModel> Data, List<KeyValuePair<string, StringValues>> Body);

        List<string> CsvHeader { get; }
    }
}

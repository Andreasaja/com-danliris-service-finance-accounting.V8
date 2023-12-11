using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoDetailTextile
{
    public class DispositionDto
    {
        public DispositionDto(int id, string documentNo, DateTimeOffset date, List<FormDetailDto> details)
        {
            Id = id;
            DocumentNo = documentNo;
            Date = date;
            Details = details;
        }

        public int Id { get; set; }
        public string DocumentNo { get; set; }
        public DateTimeOffset Date { get; set; }
        public List<FormDetailDto> Details { get; set; }
    }
}
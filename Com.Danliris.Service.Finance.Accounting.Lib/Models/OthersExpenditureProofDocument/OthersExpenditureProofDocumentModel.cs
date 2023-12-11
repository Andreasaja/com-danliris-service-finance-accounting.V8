using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels;
using Com.Moonlay.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.OthersExpenditureProofDocument
{
    public class OthersExpenditureProofDocumentModel : StandardEntity
    {
        public int AccountBankId { get; set; }
        public DateTimeOffset Date { get; set; }
        [MaxLength(64)]
        public string Type { get; set; }
        public string CekBgNo { get; set; }
        public string Remark { get; set; }
        [NotMapped]
        public bool IsUpdated { get; set; }
        [MaxLength(32)]
        public string DocumentNo { get; set; }
        public bool IsPosted { get; set; }
        public double CurrencyRate { get; set; }

        public void Update(OthersExpenditureProofDocumentCreateUpdateViewModel data)
        {
            AccountBankId = data.AccountBankId.GetValueOrDefault();
            Date = data.Date.GetValueOrDefault();
            Type = data.Type;
            CekBgNo = data.CekBgNo;
            Remark = data.Remark;
            IsUpdated = true;
        }
    }
}

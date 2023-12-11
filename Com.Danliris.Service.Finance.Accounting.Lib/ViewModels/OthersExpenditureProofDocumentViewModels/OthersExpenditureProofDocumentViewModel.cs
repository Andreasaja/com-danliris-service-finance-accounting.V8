using System;
using System.Collections.Generic;
using System.Linq;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.OthersExpenditureProofDocument;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels
{
    public class OthersExpenditureProofDocumentViewModel
    {
        public OthersExpenditureProofDocumentViewModel()
        {
            
        }
        public OthersExpenditureProofDocumentViewModel(OthersExpenditureProofDocumentModel model, List<OthersExpenditureProofDocumentItemModel> items)
        {
            Id = model.Id;
            AccountBankId = model.AccountBankId;
            Date = model.Date;
            Type = model.Type;
            CekBgNo = model.CekBgNo;
            Remark = model.Remark;
            IsPosted = model.IsPosted;
            Items = items.Select(item => new OthersExpenditureProofDocumentItemViewModel(item)).ToList();
        }

        public int Id { get; set; }
        public int AccountBankId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Type { get; set; }
        public string CekBgNo { get; set; }
        public string Remark { get; set; }
        public bool IsPosted { get; set; }

        public ICollection<OthersExpenditureProofDocumentItemViewModel> Items { get; set; }
    }

    public class OthersExpenditureProofDocumentItemViewModel
    {
        public OthersExpenditureProofDocumentItemViewModel(OthersExpenditureProofDocumentItemModel item)
        {
            Id = item.Id;
            Remark = item.Remark;
            COAId = item.COAId;
            Debit = item.Debit;
        }

        public int Id { get; set; }
        public string Remark { get; set; }
        public int COAId { get; set; }
        public decimal Debit { get; set; }
    }
}

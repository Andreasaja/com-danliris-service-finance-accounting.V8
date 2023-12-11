using System;
using System.Collections.Generic;
using System.Linq;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.OthersExpenditureProofDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Services.OthersExpenditureProofDocument
{
    public class OthersExpenditureProofDocumentPDFViewModel
    {
        public OthersExpenditureProofDocumentPDFViewModel()
        {

        }
        public OthersExpenditureProofDocumentPDFViewModel(OthersExpenditureProofDocumentModel model, List<OthersExpenditureProofDocumentItemModel> items, AccountBankViewModel accountBank, List<COAModel> coas)
        {
            Id = model.Id;
            AccountBankId = model.AccountBankId;
            Date = model.Date;
            Type = model.Type;
            Remark = model.Remark;
            Items = items.Select(item => new OthersExpenditureProofDocumentItemPDFViewModel(item, coas)).ToList();
            Bank = accountBank;
            DocumentNo = model.DocumentNo;
            CekBgNo = model.CekBgNo;
        }

        public int Id { get; set; }
        public int AccountBankId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Type { get; set; }
        public string CekBgNo { get; set; }
        public string Remark { get; set; }
        public AccountBankViewModel Bank { get; set; }
        public ICollection<OthersExpenditureProofDocumentItemPDFViewModel> Items { get; set; }
        public string DocumentNo { get; set; }
    }

    public class OthersExpenditureProofDocumentItemPDFViewModel
    {
        public OthersExpenditureProofDocumentItemPDFViewModel(OthersExpenditureProofDocumentItemModel item, List<COAModel> coas)
        {
            Id = item.Id;
            Remark = item.Remark;
            COAId = item.COAId;
            Debit = item.Debit;
            COA = coas.FirstOrDefault(coa => coa.Id == item.COAId);
        }


        public int Id { get; set; }
        public string Remark { get; set; }
        public int COAId { get; set; }
        public decimal Debit { get; set; }
        public COAModel COA { get; set; }
    }
}
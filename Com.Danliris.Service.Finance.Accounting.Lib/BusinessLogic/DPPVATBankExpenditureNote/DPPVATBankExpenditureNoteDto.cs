using Com.Danliris.Service.Finance.Accounting.Lib.Models.DPPVATBankExpenditureNote;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote
{
    public class DPPVATBankExpenditureNoteDto
    {
        public DPPVATBankExpenditureNoteDto(DPPVATBankExpenditureNoteModel model, List<DPPVATBankExpenditureNoteItemModel> items, List<DPPVATBankExpenditureNoteDetailModel> details)
        {
            Id = model.Id;
            Bank = new AccountBankDto(model.BankAccountId, model.BankAccountingCode, model.BankAccountNumber, model.BankName, model.BankCurrencyCode, model.BankCurrencyId, model.BankCurrencyRate);
            Currency = new CurrencyDto(model.CurrencyCode, model.CurrencyId, model.CurrencyRate);
            Supplier = new SupplierDto(model.SupplierId, model.SupplierName, model.IsImportSupplier, null);
            BGCheckNo = model.BGCheckNo;
            Amount = model.Amount;
            Date = model.Date;
            IsPosted = model.IsPosted;
            Items = items.Select(item => new DPPVATBankExpenditureNoteItemDto(item, details.Where(detail => detail.DPPVATBankExpenditureNoteItemId == item.Id).ToList())).ToList();
            DocumentNo = model.DocumentNo;
        }

        public int Id { get; private set; }
        public AccountBankDto Bank { get; private set; }
        public CurrencyDto Currency { get; private set; }
        public SupplierDto Supplier { get; private set; }
        public string BGCheckNo { get; private set; }
        public double Amount { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public bool IsPosted { get; private set; }
        public List<DPPVATBankExpenditureNoteItemDto> Items { get; private set; }
        public string DocumentNo { get; private set; }
    }
}
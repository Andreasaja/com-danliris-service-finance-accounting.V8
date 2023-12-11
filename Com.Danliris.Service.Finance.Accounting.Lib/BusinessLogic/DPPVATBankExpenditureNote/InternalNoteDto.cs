using Com.Danliris.Service.Finance.Accounting.Lib.Models.DPPVATBankExpenditureNote;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote
{
    public class InternalNoteDto
    {
        public InternalNoteDto()
        {

        }

        public InternalNoteDto(DPPVATBankExpenditureNoteItemModel item, List<DPPVATBankExpenditureNoteDetailModel> details)
        {
            Id = item.Id;
            DocumentNo = item.InternalNoteNo;
            Date = item.InternalNoteDate;
            DueDate = item.DueDate;
            Supplier = new SupplierDto(item.SupplierId, item.SupplierName, item.IsImportSupplier, item.SupplierCode);
            VATAmount = item.VATAmount;
            IncomeTaxAmount = item.IncomeTaxAmount;
            DPP = item.DPP;
            TotalAmount = item.TotalAmount;
            Currency = new CurrencyDto(item.CurrencyCode, item.CurrencyId, 0);
            Items = details.Select(detail => new InternalNoteInvoiceDto(detail)).ToList();
        }

        public int Id { get; set; }
        public string DocumentNo { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public SupplierDto Supplier { get; set; }
        public double VATAmount { get; set; }
        public double IncomeTaxAmount { get; set; }
        public double DPP { get; set; }
        public double TotalAmount { get; set; }
        public CurrencyDto Currency { get; set; }
        public List<InternalNoteInvoiceDto> Items { get; set; }
    }
}
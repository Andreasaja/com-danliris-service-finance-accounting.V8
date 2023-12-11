using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingPphBankExpenditureNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingPphBankExpenditureNoteViewModels
{
    public class GarmentPurchasingPphBankExpenditureNoteDataViewModel
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string No { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }
        public string BankAccountName { get; set; }
        public string IncomeTaxName { get; set; }
        public double IncomeTaxRate { get; set; }
        public double TotalDpp { get; set; }
        public double TotalIncomeTax { get; set; }
        public string Currency { get; set; }
        public DateTimeOffset LastModifiedUtc { get; set; }
        public string InvoiceOutNo { get; set; }
        public string INNo { get; set; }
        public DateTimeOffset InvoucieOutDate { get; set; }
        public bool IsPosted { get; set; }

        public GarmentPurchasingPphBankExpenditureNoteDataViewModel()
        {

        }

        public GarmentPurchasingPphBankExpenditureNoteDataViewModel(GarmentPurchasingPphBankExpenditureNoteModel model)
        {
            Id = model.Id;
            Date = model.InvoiceOutDate;
            No = model.InvoiceOutNumber;
            CreatedUtc = model.CreatedUtc;
            BankAccountName = model.BankName;
            IncomeTaxName = model.IncomeTaxName;
            IncomeTaxRate = model.IncomeTaxRate;
            TotalIncomeTax = model.Items.Sum(element => element.IncomeTaxTotal);
            Currency = model.BankCurrencyCode;
            LastModifiedUtc = model.LastModifiedUtc;
            IsPosted = model.IsPosted;
            InvoiceOutNo = model.InvoiceOutNumber;
            InvoucieOutDate = model.InvoiceOutDate;
        }

        public GarmentPurchasingPphBankExpenditureNoteDataViewModel(GarmentPurchasingPphBankExpenditureNoteItemModel model)
        {
            Id = model.GarmentPurchasingPphBankExpenditureNoteId;
            Date = model.GarmentPurchasingPphBankExpenditureNote.InvoiceOutDate;
            //Date = model.GarmentPurchasingPphBankExpenditureNote.CreatedUtc >= model.GarmentPurchasingPphBankExpenditureNote.LastModifiedUtc? model.GarmentPurchasingPphBankExpenditureNote.CreatedUtc : model.GarmentPurchasingPphBankExpenditureNote.LastModifiedUtc;

            No = model.GarmentPurchasingPphBankExpenditureNote.InvoiceOutNumber;
            CreatedUtc = model.GarmentPurchasingPphBankExpenditureNote.CreatedUtc;
            BankAccountName = model.GarmentPurchasingPphBankExpenditureNote.BankName +' '+ model.GarmentPurchasingPphBankExpenditureNote.BankCurrencyCode;
            IncomeTaxName = model.GarmentPurchasingPphBankExpenditureNote.IncomeTaxName;
            IncomeTaxRate = model.GarmentPurchasingPphBankExpenditureNote.IncomeTaxRate;
            //TotalIncomeTax = model.GarmentPurchasingPphBankExpenditureNote.Items.Sum(element => element.IncomeTaxTotal * (element.IncomeTaxRate/100));
            TotalIncomeTax = model.GarmentPurchasingPphBankExpenditureNote.Items.Sum(element => element.TotalPaid * (element.IncomeTaxTotal / 100));

            Currency = model.GarmentPurchasingPphBankExpenditureNote.BankCurrencyCode;
            LastModifiedUtc = model.GarmentPurchasingPphBankExpenditureNote.LastModifiedUtc;
            IsPosted = model.GarmentPurchasingPphBankExpenditureNote.IsPosted;
            InvoiceOutNo = model.GarmentPurchasingPphBankExpenditureNote.InvoiceOutNumber;
            InvoucieOutDate = model.GarmentPurchasingPphBankExpenditureNote.InvoiceOutDate;
            INNo = model.InternalNotesNo;
            TotalDpp = Convert.ToDouble(model.GarmentPurchasingPphBankExpenditureNoteInvoices.Sum(t=> t.Total));
        }
    }
}

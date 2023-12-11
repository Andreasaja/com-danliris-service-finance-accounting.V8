using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingPphBankExpenditureNote;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingPphBankExpenditureNoteViewModels;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.GarmentPurchasingExpenditureNote
{
    public class GarmentPurchasingExpenditureNoteProfile : Profile
    {
        public GarmentPurchasingExpenditureNoteProfile()
        {
            CreateMap<FormInsert, GarmentPurchasingPphBankExpenditureNoteModel>()
                 .ForPath(d=> d.Id ,opt=> opt.MapFrom(s=> s.Id))
                 .ForPath(d => d.InvoiceOutNumber, opt => opt.MapFrom(s => s.PphBankInvoiceNo))
                 .ForPath(d => d.InvoiceOutDate, opt => opt.MapFrom(s => s.Date))
                 .ForPath(d => d.DueDateStart, opt => opt.MapFrom(s => s.DateFrom))
                 .ForPath(d => d.DueDateEnd, opt => opt.MapFrom(s => s.DateTo))
                 .ForPath(d => d.IncomeTaxId, opt => opt.MapFrom(s => s.IncomeTax.Id))
                 .ForPath(d => d.IncomeTaxName, opt => opt.MapFrom(s => s.IncomeTax.Name))
                 .ForPath(d => d.IncomeTaxRate, opt => opt.MapFrom(s => s.IncomeTax.Rate))
                 .ForPath(d => d.AccountBankCOA, opt => opt.MapFrom(s => s.Bank.AccountCOA))
                 .ForPath(d => d.AccountBankName, opt => opt.MapFrom(s => s.Bank.AccountName))
                 .ForPath(d => d.AccountBankNumber, opt => opt.MapFrom(s => s.Bank.AccountNumber))
                 .ForPath(d => d.BankAddress, opt => opt.MapFrom(s => s.Bank.BankAddress))
                 .ForPath(d => d.BankCode, opt => opt.MapFrom(s => s.Bank.Code))
                 .ForPath(d => d.BankName, opt => opt.MapFrom(s => s.Bank.BankName))
                 .ForPath(d => d.BankCode1, opt => opt.MapFrom(s => s.Bank.BankCode))
                 .ForPath(d => d.BankCurrencyCode, opt => opt.MapFrom(s => s.Bank.Currency.Code))
                 .ForPath(d => d.BankCurrencyId, opt => opt.MapFrom(s => s.Bank.Currency.Id))
                 .ForPath(d => d.BankSwiftCode, opt => opt.MapFrom(s => s.Bank.SwiftCode))
                 .ForPath(d => d.IsPosted, opt => opt.MapFrom(s => false))
                 .ForPath(d => d.Items, opt => opt.MapFrom(s => s.PPHBankExpenditureNoteItems.Select(item => new GarmentPurchasingPphBankExpenditureNoteItemModel
                 {
                     Id = item.Id,
                     Date = item.INDate.GetValueOrDefault(),
                     DueDate = item.INDueDate.GetValueOrDefault(),
                     CurrencyCode = item.CurrencyCode,
                     CurrencyId = item.CurrencyId,
                     IncomeTaxId = item.GarmentInvoice == null ? 0 : (int)item.GarmentInvoice.IncomeTaxId,
                     IncomeTaxName = item.GarmentInvoice == null ? string.Empty : item.GarmentInvoice.IncomeTaxName,
                     IncomeTaxRate = item.GarmentInvoice == null? 0 : item.GarmentInvoice.IncomeTaxRate,
                     IncomeTaxTotal = Convert.ToDouble(s.IncomeTax.Rate),
                     SupplierId = item.SupplierId,
                     SupplierName = item.SupplierName,
                     SupplierCode = item.SupplierCode,
                     InternalNotesId = item.INId,
                     InternalNotesNo = item.INNo,
                     PaymentDueDays = item.Items.FirstOrDefault().Details.FirstOrDefault().PaymentDueDays,
                     PaymentMethod = item.Items.FirstOrDefault().Details.FirstOrDefault().PaymentMethod,
                     PaymentType = item.Items.FirstOrDefault().Details.FirstOrDefault().PaymentType,
                     TotalPaid = item.Items.Sum(j => j.TotalAmount.GetValueOrDefault()),
                     AmountDPP = Convert.ToDouble(item.Items.Sum(j=> j.Details.Sum(k=> k.PricePerDealUnit))),
                     GarmentPurchasingPphBankExpenditureNoteInvoices = item.Items.SelectMany(t => t.Details).Select(invoice => new GarmentPurchasingPphBankExpenditureNoteInvoiceModel
                     {
                         Id = invoice.Id.GetValueOrDefault(),
                         InvoicesDate = invoice.InvoiceDate,
                         InvoicesNo = invoice.InvoiceNo,
                         InvoicesId = invoice.InvoiceId,
                         ProductCategory = invoice.ProductCategory,
                         ProductCode = invoice.ProductCode,
                         ProductId = invoice.ProductId,
                         ProductName = invoice.ProductName,
                         Total = Convert.ToDecimal(invoice.PriceTotal),
                         UnitCode = invoice.UnitCode,
                         UnitId = invoice.UnitId,
                         UnitName = invoice.UnitName,
                         PaymentBill = invoice.GarmentDeliveryOrder.PaymentBill,
                         BillNo = invoice.GarmentDeliveryOrder.BillNo,
                         DoNo = invoice.GarmentDeliveryOrder.DONo,
                         NPH = item.Items.FirstOrDefault().GarmentInvoice == null ? string.Empty: item.Items.FirstOrDefault().GarmentInvoice.NPH
                     })
                     .ToList()
                 })
                 ))
                 .ReverseMap();
        }
    }
}

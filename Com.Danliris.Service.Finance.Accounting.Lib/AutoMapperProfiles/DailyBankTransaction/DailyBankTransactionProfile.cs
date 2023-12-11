using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingPphBankExpenditureNote;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingPphBankExpenditureNoteViewModels;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.DailyBankTransaction
{
    public class DailyBankTransactionProfile : Profile
    {
        public DailyBankTransactionProfile()
        {
            
            CreateMap<DailyBankTransactionModel, DailyBankTransactionViewModel>()
                .ForPath(d => d.Date, opt => opt.MapFrom(s => s.Date))
                .ForPath(d => d.IsPosted, opt => opt.MapFrom(s => s.IsPosted))
                /* Bank */
                .ForPath(d => d.Bank.Id, opt => opt.MapFrom(s => s.AccountBankId))
                .ForPath(d => d.Bank.BankCode, opt => opt.MapFrom(s => s.AccountBankCode))
                .ForPath(d => d.Bank.AccountName, opt => opt.MapFrom(s => s.AccountBankAccountName))
                .ForPath(d => d.Bank.AccountNumber, opt => opt.MapFrom(s => s.AccountBankAccountNumber))
                .ForPath(d => d.Bank.BankName, opt => opt.MapFrom(s => s.AccountBankName))
                .ForPath(d => d.Bank.Currency.Id, opt => opt.MapFrom(s => s.AccountBankCurrencyId))
                .ForPath(d => d.Bank.Currency.Code, opt => opt.MapFrom(s => s.AccountBankCurrencyCode))
                .ForPath(d => d.Bank.Currency.Symbol, opt => opt.MapFrom(s => s.AccountBankCurrencySymbol))

                /* Destination Bank */
                .ForPath(d => d.OutputBank.Id, opt => opt.MapFrom(s => s.DestinationBankId))
                .ForPath(d => d.OutputBank.BankCode, opt => opt.MapFrom(s => s.DestinationBankCode))
                .ForPath(d => d.OutputBank.AccountName, opt => opt.MapFrom(s => s.DestinationBankAccountName))
                .ForPath(d => d.OutputBank.AccountNumber, opt => opt.MapFrom(s => s.DestinationBankAccountNumber))
                .ForPath(d => d.OutputBank.BankName, opt => opt.MapFrom(s => s.DestinationBankName))
                .ForPath(d => d.OutputBank.Currency.Id, opt => opt.MapFrom(s => s.DestinationBankCurrencyId))
                .ForPath(d => d.OutputBank.Currency.Code, opt => opt.MapFrom(s => s.DestinationBankCurrencyCode))
                .ForPath(d => d.OutputBank.Currency.Symbol, opt => opt.MapFrom(s => s.DestinationBankCurrencySymbol))

                /* Supplier */
                .ForPath(d => d.Supplier._id, opt => opt.MapFrom(s => s.SupplierId))
                .ForPath(d => d.Supplier.code, opt => opt.MapFrom(s => s.SupplierCode))
                .ForPath(d => d.Supplier.name, opt => opt.MapFrom(s => s.SupplierName))

                /* Buyer */
                .ForPath(d => d.Buyer.Id, opt => opt.MapFrom(s => s.BuyerId))
                .ForPath(d => d.Buyer.Code, opt => opt.MapFrom(s => s.BuyerCode))
                .ForPath(d => d.Buyer.Name, opt => opt.MapFrom(s => s.BuyerName))

                .ReverseMap();

            // this map is for insert from pph to daily bank transaction
            CreateMap<GarmentPurchasingPphBankExpenditureNoteModel, DailyBankTransactionModel>()
                .ForPath(d=> d.Id,opt => opt.MapFrom(s=>0))
                .ForPath(d => d.AccountBankAccountName, opt => opt.MapFrom(s => s.AccountBankName))
                .ForPath(d=> d.AccountBankAccountNumber, opt=> opt.MapFrom(s=>s.AccountBankNumber))
                .ForPath(d=> d.AccountBankCode, opt=>opt.MapFrom(s=> s.BankCode))
                .ForPath(d=> d.AccountBankCurrencyCode, opt=> opt.MapFrom(s=>s.BankCurrencyCode))
                .ForPath(d=> d.AccountBankCurrencyId, opt=> opt.MapFrom(s=>s.BankCurrencyId))
                //.ForPath(d=> d.AccountBankId, opt=> opt.MapFrom(s=> s.BankCode1))
                .ForPath(d=> d.AccountBankName, opt=> opt.MapFrom(s=> s.BankName))
                .ForPath(d=> d.SupplierCode, opt=> opt.MapFrom(s=> s.Items.FirstOrDefault().SupplierId))
                .ForPath(d => d.SupplierId, opt => opt.MapFrom(s => s.Items.FirstOrDefault().SupplierId))
                .ForPath(d => d.SupplierName, opt => opt.MapFrom(s => s.Items.FirstOrDefault().SupplierName))
                .ForPath(d => d.Nominal, opt => opt.MapFrom(s => (s.IncomeTaxRate/100)*s.Items.Sum(j=> j.TotalPaid)))
                .ForPath(d=> d.ReferenceNo, opt=> opt.MapFrom(s=> s.InvoiceOutNumber))
                .ForPath(d=> d.SourceType, opt=> opt.MapFrom(s=> "Operasional"))
                .ForPath(d=> d.Status, opt=> opt.MapFrom(s=> "OUT"))
                .ReverseMap();

            CreateMap<FormInsert, DailyBankTransactionModel>()
                .ForPath(d => d.Id, opt => opt.MapFrom(s => 0))
                .ForPath(d => d.AccountBankAccountName, opt => opt.MapFrom(s => s.Bank.AccountName))
                .ForPath(d => d.AccountBankAccountNumber, opt => opt.MapFrom(s => s.Bank.AccountNumber))
                .ForPath(d => d.AccountBankCode, opt => opt.MapFrom(s => s.Bank.Code))
                .ForPath(d => d.AccountBankCurrencyCode, opt => opt.MapFrom(s => s.Bank.Currency.Code))
                .ForPath(d => d.AccountBankCurrencyId, opt => opt.MapFrom(s => s.Bank.Currency.Id))
                //.ForPath(d=> d.AccountBankId, opt=> opt.MapFrom(s=> s.BankCode1))
                .ForPath(d => d.AccountBankName, opt => opt.MapFrom(s => s.Bank.BankName))
                .ForPath(d => d.SupplierCode, opt => opt.MapFrom(s => s.PPHBankExpenditureNoteItems.FirstOrDefault().SupplierCode))
                .ForPath(d => d.SupplierId, opt => opt.MapFrom(s => s.PPHBankExpenditureNoteItems.FirstOrDefault().SupplierId))
                .ForPath(d => d.SupplierName, opt => opt.MapFrom(s => s.PPHBankExpenditureNoteItems.FirstOrDefault().SupplierName))
                .ForPath(d => d.Nominal, opt => opt.MapFrom(s => s.PPHBankExpenditureNoteItems.Sum(t=> t.Items.Sum(j=> j.TotalIncomeTax))))
                .ForPath(d => d.ReferenceNo, opt => opt.MapFrom(s => s.PphBankInvoiceNo))
                .ForPath(d => d.SourceType, opt => opt.MapFrom(s => "Operasional"))
                .ForPath(d => d.Status, opt => opt.MapFrom(s => "OUT"))
                .ReverseMap();
        }
    }
}

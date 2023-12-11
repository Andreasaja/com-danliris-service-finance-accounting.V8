using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.SalesReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.SalesReceipt;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.SalesReceipt
{
    public class SalesReceiptProfile : Profile
    {
        public SalesReceiptProfile()
        {
            CreateMap<SalesReceiptModel, SalesReceiptViewModel>()

                .ForPath(d => d.Unit.Id, opt => opt.MapFrom(s => s.UnitId))
                .ForPath(d => d.Unit.Name, opt => opt.MapFrom(s => s.UnitName))

                .ForPath(d => d.Buyer.Id, opt => opt.MapFrom(s => s.BuyerId))
                .ForPath(d => d.Buyer.Name, opt => opt.MapFrom(s => s.BuyerName))
                .ForPath(d => d.Buyer.Address, opt => opt.MapFrom(s => s.BuyerAddress))

                .ForPath(d => d.Currency.Id, opt => opt.MapFrom(s => s.CurrencyId))
                .ForPath(d => d.Currency.Code, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.Currency.Symbol, opt => opt.MapFrom(s => s.CurrencySymbol))
                .ForPath(d => d.Currency.Rate, opt => opt.MapFrom(s => s.CurrencyRate))

                .ForPath(d => d.Bank.Id, opt => opt.MapFrom(s => s.BankId))
                .ForPath(d => d.Bank.AccountName, opt => opt.MapFrom(s => s.AccountName))
                .ForPath(d => d.Bank.AccountNumber, opt => opt.MapFrom(s => s.AccountNumber))
                .ForPath(d => d.Bank.BankName, opt => opt.MapFrom(s => s.BankName))
                .ForPath(d => d.Bank.Code, opt => opt.MapFrom(s => s.BankCode))

                .ReverseMap();


            CreateMap<SalesReceiptDetailModel, SalesReceiptDetailViewModel>()

                .ForPath(d => d.SalesInvoice.Id, opt => opt.MapFrom(s => s.SalesInvoiceId))
                .ForPath(d => d.SalesInvoice.SalesInvoiceNo, opt => opt.MapFrom(s => s.SalesInvoiceNo))
                .ForPath(d => d.SalesInvoice.VatType, opt => opt.MapFrom(s => s.VatType))

                .ForPath(d => d.SalesInvoice.Currency.Id, opt => opt.MapFrom(s => s.CurrencyId))
                .ForPath(d => d.SalesInvoice.Currency.Code, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.SalesInvoice.Currency.Symbol, opt => opt.MapFrom(s => s.CurrencySymbol))
                .ForPath(d => d.SalesInvoice.Currency.Rate, opt => opt.MapFrom(s => s.CurrencyRate))

                .ReverseMap();

        }
    }
}
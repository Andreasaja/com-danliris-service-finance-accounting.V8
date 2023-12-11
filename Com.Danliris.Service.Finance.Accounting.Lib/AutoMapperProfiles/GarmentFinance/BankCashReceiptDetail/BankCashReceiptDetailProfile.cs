using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceiptDetail;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.GarmentFinance.BankCashReceiptDetail
{
    public class BankCashReceiptDetailProfile : Profile
    {
        public BankCashReceiptDetailProfile()
        {
            CreateMap<BankCashReceiptDetailModel, BankCashReceiptDetailViewModel>()

                .ForPath(d => d.DebitCoa.Id, opt => opt.MapFrom(s => s.DebitCoaId))
                .ForPath(d => d.DebitCoa.Code, opt => opt.MapFrom(s => s.DebitCoaCode))
                .ForPath(d => d.DebitCoa.Name, opt => opt.MapFrom(s => s.DebitCoaName))

                .ForPath(d => d.InvoiceCoa.Id, opt => opt.MapFrom(s => s.InvoiceCoaId))
                .ForPath(d => d.InvoiceCoa.Code, opt => opt.MapFrom(s => s.InvoiceCoaCode))
                .ForPath(d => d.InvoiceCoa.Name, opt => opt.MapFrom(s => s.InvoiceCoaName))

                .ReverseMap();

            CreateMap<BankCashReceiptDetailItemModel, BankCashReceiptDetailItemViewModel>()
                .ForPath(d => d.BuyerAgent.Id, opt => opt.MapFrom(s => s.BuyerId))
                .ForPath(d => d.BuyerAgent.Code, opt => opt.MapFrom(s => s.BuyerCode))
                .ForPath(d => d.BuyerAgent.Name, opt => opt.MapFrom(s => s.BuyerName))

                .ForPath(d => d.Currency.Id, opt => opt.MapFrom(s => s.CurrencyId))
                .ForPath(d => d.Currency.Code, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.Currency.Rate, opt => opt.MapFrom(s => s.CurrencyRate))
                .ReverseMap();

            CreateMap<BankCashReceiptDetailOtherItemModel, BankCashReceiptDetailOtherItemViewModel>()

                .ForPath(d => d.Account.Id, opt => opt.MapFrom(s => s.ChartOfAccountId))
                .ForPath(d => d.Account.Code, opt => opt.MapFrom(s => s.ChartOfAccountCode))
                .ForPath(d => d.Account.Name, opt => opt.MapFrom(s => s.ChartOfAccountName))

                .ForPath(d => d.Currency.Id, opt => opt.MapFrom(s => s.CurrencyId))
                .ForPath(d => d.Currency.Code, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.Currency.Rate, opt => opt.MapFrom(s => s.CurrencyRate))
                .ReverseMap();
        }
    }
}

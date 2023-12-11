using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DownPayment;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.DownPayment
{
    public class DownPaymentProfile: Profile
    {
        public DownPaymentProfile()
        {
            CreateMap<DownPaymentModel, DownPaymentViewModel>()

                .ForPath(d => d.Bank.AccountName, opt => opt.MapFrom(s => s.AccountName))
                .ForPath(d => d.Bank.AccountNumber, opt => opt.MapFrom(s => s.AccountNumber))
                .ForPath(d => d.Bank.BankName, opt => opt.MapFrom(s => s.BankName))
                .ForPath(d => d.Bank.Currency.Code, opt => opt.MapFrom(s => s.CodeBankCurrency))

                .ForPath(d => d.Buyer.Id, opt => opt.MapFrom(s => s.BuyerId))
                .ForPath(d => d.Buyer.Name, opt => opt.MapFrom(s => s.BuyerName))
                .ForPath(d => d.Buyer.Code, opt => opt.MapFrom(s => s.BuyerCode))

                .ForPath(d => d.Currency.Id, opt => opt.MapFrom(s => s.CurrencyId))
                .ForPath(d => d.Currency.Code, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.Currency.Rate, opt => opt.MapFrom(s => s.CurrencyRate))

                .ReverseMap();
        }
    }
}

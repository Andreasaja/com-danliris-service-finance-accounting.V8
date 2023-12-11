using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceipt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.GarmentFinance.BankCashReceipt
{
    public class BankCashReceiptProfile : Profile
    {
        public BankCashReceiptProfile()
        {
            CreateMap<BankCashReceiptModel, BankCashReceiptViewModel>()
                .ForPath(d => d.Bank.Id, opt => opt.MapFrom(s => s.BankAccountId))
                .ForPath(d => d.Bank.Code, opt => opt.MapFrom(s => s.BankAccountingCode))
                .ForPath(d => d.Bank.BankName, opt => opt.MapFrom(s => s.BankName))
                .ForPath(d => d.Bank.AccountName, opt => opt.MapFrom(s => s.BankAccountName))
                .ForPath(d => d.Bank.AccountNumber, opt => opt.MapFrom(s => s.BankAccountNumber))
                .ForPath(d => d.Bank.AccountCOA, opt => opt.MapFrom(s => s.BankAccountCOA))

                .ForPath(d => d.Currency.Id, opt => opt.MapFrom(s => s.CurrencyId))
                .ForPath(d => d.Currency.Code, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.Currency.Rate, opt => opt.MapFrom(s => s.CurrencyRate))

                .ForPath(d => d.DebitCoa.Id, opt => opt.MapFrom(s => s.DebitCoaId))
                .ForPath(d => d.DebitCoa.Code, opt => opt.MapFrom(s => s.DebitCoaCode))
                .ForPath(d => d.DebitCoa.Name, opt => opt.MapFrom(s => s.DebitCoaName))

                .ForPath(d => d.BankCashReceiptType.Id, opt => opt.MapFrom(s => s.BankCashReceiptTypeId))
                .ForPath(d => d.BankCashReceiptType.Name, opt => opt.MapFrom(s => s.BankCashReceiptTypeName))
                .ForPath(d => d.BankCashReceiptType.COACode, opt => opt.MapFrom(s => s.BankCashReceiptTypeCoaCode))
                .ForPath(d => d.BankCashReceiptType.COAId, opt => opt.MapFrom(s => s.BankCashReceiptTypeCoaId))
                .ForPath(d => d.BankCashReceiptType.COAName, opt => opt.MapFrom(s => s.BankCashReceiptTypeCoaName))

                .ForPath(d => d.Buyer.Id, opt => opt.MapFrom(s => s.BuyerId))
                .ForPath(d => d.Buyer.Code, opt => opt.MapFrom(s => s.BuyerCode))
                .ForPath(d => d.Buyer.Name, opt => opt.MapFrom(s => s.BuyerName))

                .ReverseMap();

            CreateMap<BankCashReceiptItemModel, BankCashReceiptItemViewModel>()
                .ForPath(d => d.AccNumber.Id, opt => opt.MapFrom(s => s.AccNumberCoaId))
                .ForPath(d => d.AccNumber.Code, opt => opt.MapFrom(s => s.AccNumberCoaCode))
                .ForPath(d => d.AccNumber.Name, opt => opt.MapFrom(s => s.AccNumberCoaName))

                //.ForPath(d => d.AccUnit.Id, opt => opt.MapFrom(s => s.AccUnitCoaId))
                //.ForPath(d => d.AccUnit.Code, opt => opt.MapFrom(s => s.AccUnitCoaCode))
                //.ForPath(d => d.AccUnit.Name, opt => opt.MapFrom(s => s.AccUnitCoaName))

                .ForPath(d => d.AccSub.Id, opt => opt.MapFrom(s => s.AccSubCoaId))
                .ForPath(d => d.AccSub.Code, opt => opt.MapFrom(s => s.AccSubCoaCode))
                .ForPath(d => d.AccSub.Name, opt => opt.MapFrom(s => s.AccSubCoaName))

                //.ForPath(d => d.AccAmount.Id, opt => opt.MapFrom(s => s.AccAmountCoaId))
                //.ForPath(d => d.AccAmount.Code, opt => opt.MapFrom(s => s.AccAmountCoaCode))
                //.ForPath(d => d.AccAmount.Name, opt => opt.MapFrom(s => s.AccAmountCoaName))

                .ReverseMap();
        }
        
    }
}

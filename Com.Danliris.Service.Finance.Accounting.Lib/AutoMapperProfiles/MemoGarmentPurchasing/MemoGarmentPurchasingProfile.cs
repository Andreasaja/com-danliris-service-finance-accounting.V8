using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoGarmentPurchasing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.MemoGarmentPurchasing
{
    public class MemoGarmentPurchasingProfile : Profile
    {
        public MemoGarmentPurchasingProfile()
        {
            CreateMap<MemoGarmentPurchasingModel, MemoGarmentPurchasingViewModel>()
                .ForPath(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForPath(d => d.MemoNo, opt => opt.MapFrom(s => s.MemoNo))
                .ForPath(d => d.MemoDate, opt => opt.MapFrom(s => s.MemoDate))

                .ForPath(d => d.AccountingBook.Id, opt => opt.MapFrom(s => s.AccountingBookId))
                .ForPath(d => d.AccountingBook.Code, opt => opt.MapFrom(s => s.AccountingBookCode))
                .ForPath(d => d.AccountingBook.Type, opt => opt.MapFrom(s => s.AccountingBookType))

                .ForPath(d => d.Currency.Id, opt => opt.MapFrom(s => s.GarmentCurrenciesId))
                .ForPath(d => d.Currency.Code, opt => opt.MapFrom(s => s.GarmentCurrenciesCode))
                .ForPath(d => d.Currency.Rate, opt => opt.MapFrom(s => s.GarmentCurrenciesRate))

                .ForPath(d => d.Remarks, opt => opt.MapFrom(s => s.Remarks))
                .ForPath(d => d.IsPosted, opt => opt.MapFrom(s => s.IsPosted))

                .ForPath(d => d.MemoGarmentPurchasingDetails, opt => opt.MapFrom(s => s.MemoGarmentPurchasingDetails))

                .ReverseMap();

            CreateMap<MemoGarmentPurchasingDetailModel, MemoGarmentPurchasingDetailViewModel>()
                .ForPath(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForPath(d => d.COA.Id, opt => opt.MapFrom(s => s.COAId))
                .ForPath(d => d.COA.Name, opt => opt.MapFrom(s => s.COAName))
                .ForPath(d => d.COA.No, opt => opt.MapFrom(s => s.COANo))
                .ForPath(d => d.DebitNominal, opt => opt.MapFrom(s => s.DebitNominal))
                .ForPath(d => d.CreditNominal, opt => opt.MapFrom(s => s.CreditNominal))

                .ReverseMap();

            CreateMap<MemoGarmentPurchasingDetailModel, MemoGarmentPurchasingReportViewModel>()
                .ForPath(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForPath(d => d.COA.Id, opt => opt.MapFrom(s => s.COAId))
                .ForPath(d => d.COA.Name, opt => opt.MapFrom(s => s.COAName))
                .ForPath(d => d.COA.No, opt => opt.MapFrom(s => s.COANo))
                .ForPath(d => d.DebitNominal, opt => opt.MapFrom(s => s.DebitNominal))
                .ForPath(d => d.CreditNominal, opt => opt.MapFrom(s => s.CreditNominal))
                .ForPath(d => d.MemoGarmentPurchasing, opt => opt.MapFrom(s => s.MemoGarmentPurchasing))

                .ReverseMap();

            CreateMap<MemoGarmentPurchasingModel, ListMemoGarmentPurchasingViewModel>()
                .ForPath(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForPath(d => d.MemoNo, opt => opt.MapFrom(s => s.MemoNo))
                .ForPath(d => d.MemoDate, opt => opt.MapFrom(s => s.MemoDate))

                .ForPath(d => d.AccountingBook.Id, opt => opt.MapFrom(s => s.AccountingBookId))
                .ForPath(d => d.AccountingBook.Code, opt => opt.MapFrom(s => s.AccountingBookCode))
                .ForPath(d => d.AccountingBook.Type, opt => opt.MapFrom(s => s.AccountingBookType))

                .ForPath(d => d.Currency.Id, opt => opt.MapFrom(s => s.GarmentCurrenciesId))
                .ForPath(d => d.Currency.Code, opt => opt.MapFrom(s => s.GarmentCurrenciesCode))
                .ForPath(d => d.Currency.Rate, opt => opt.MapFrom(s => s.GarmentCurrenciesRate))

                .ForPath(d => d.Remarks, opt => opt.MapFrom(s => s.Remarks))
                .ForPath(d => d.IsPosted, opt => opt.MapFrom(s => s.IsPosted))
                .ForPath(d => d.TotalAmount, opt => opt.MapFrom(s => s.TotalAmount))

                .ForPath(d => d.MemoGarmentPurchasingDetails, opt => opt.MapFrom(s => s.MemoGarmentPurchasingDetails))

                .ReverseMap();
        }
    }
}

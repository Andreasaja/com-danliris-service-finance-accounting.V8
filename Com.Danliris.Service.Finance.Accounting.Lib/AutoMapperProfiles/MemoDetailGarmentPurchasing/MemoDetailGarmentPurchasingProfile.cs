using System;
using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.MemoDetailGarmentPurchasing
{
    public class MemoDetailGarmentPurchasingProfile: Profile
    {
        public MemoDetailGarmentPurchasingProfile()
        {
            CreateMap<MemoDetailGarmentPurchasingModel, MemoDetailGarmentPurchasingViewModel>()
                .ForPath(d => d.MemoId, opt => opt.MapFrom(s => s.MemoId))
                //.ForPath(d => d.MemoNo, opt => opt.MapFrom(s => s.MemoNo))
                //.ForPath(d => d.MemoDate, opt => opt.MapFrom(s => s.MemoDate))
                //.ForPath(d => d.AccountingBookId, opt => opt.MapFrom(s => s.AccountingBookId))
                //.ForPath(d => d.AccountingBookType, opt => opt.MapFrom(s => s.AccountingBookType))
                //.ForPath(d => d.GarmentCurrenciesId, opt => opt.MapFrom(s => s.GarmentCurrenciesId))
                //.ForPath(d => d.GarmentCurrenciesCode, opt => opt.MapFrom(s => s.GarmentCurrenciesCode))
                //.ForPath(d => d.GarmentCurrenciesRate, opt => opt.MapFrom(s => s.GarmentCurrenciesRate))
                .ForPath(d => d.Remarks, opt => opt.MapFrom(s => s.Remarks))
                .ForPath(d => d.IsPosted, opt => opt.MapFrom(s => s.IsPosted))

                .ReverseMap();

            CreateMap<MemoDetailGarmentPurchasingDetailModel, MemoDetailGarmentPurchasingDetailViewModel>()
                .ForPath(d => d.GarmentDeliveryOrderId, opt => opt.MapFrom(s => s.GarmentDeliveryOrderId))
                .ForPath(d => d.GarmentDeliveryOrderNo, opt => opt.MapFrom(s => s.GarmentDeliveryOrderNo))
                .ForPath(d => d.RemarksDetail, opt => opt.MapFrom(s => s.RemarksDetail))
                .ForPath(d => d.PaymentRate, opt => opt.MapFrom(s => s.PaymentRate))
                .ForPath(d => d.PurchasingRate, opt => opt.MapFrom(s => s.PurchasingRate))
                .ForPath(d => d.MemoAmount, opt => opt.MapFrom(s => s.MemoAmount))
                .ForPath(d => d.MemoIdrAmount, opt => opt.MapFrom(s => s.MemoIdrAmount))
                .ForPath(d => d.SupplierCode, opt => opt.MapFrom(s => s.SupplierCode))
                .ForPath(d => d.SupplierName, opt => opt.MapFrom(s => s.SupplierName))

                .ReverseMap();
        }
    }
}
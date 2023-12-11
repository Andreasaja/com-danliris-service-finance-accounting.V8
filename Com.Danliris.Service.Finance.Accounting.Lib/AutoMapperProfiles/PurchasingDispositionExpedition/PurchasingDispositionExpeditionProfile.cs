using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionExpedition;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.PurchasingDispositionExpedition
{
    public class PurchasingDispositionExpeditionProfile : Profile
    {
        public PurchasingDispositionExpeditionProfile()
        {
            CreateMap<PurchasingDispositionExpeditionItemModel, PurchasingDispositionExpeditionItemViewModel>()
                .ForPath(d => d.product._id, opt => opt.MapFrom(s => s.ProductId))
                .ForPath(d => d.product.code, opt => opt.MapFrom(s => s.ProductCode))
                .ForPath(d => d.product.name, opt => opt.MapFrom(s => s.ProductName))

                .ForPath(d => d.unit._id, opt => opt.MapFrom(s => s.UnitId))
                .ForPath(d => d.unit.code, opt => opt.MapFrom(s => s.UnitCode))
                .ForPath(d => d.unit.name, opt => opt.MapFrom(s => s.UnitName))

                .ForPath(d => d.uom._id, opt => opt.MapFrom(s => s.UomId))
                .ForPath(d => d.uom.unit, opt => opt.MapFrom(s => s.UomUnit))

                .ReverseMap();

            CreateMap<PurchasingDispositionExpeditionModel, PurchasingDispositionExpeditionViewModel>()
                .ForPath(d => d.currency._id, opt => opt.MapFrom(s => s.CurrencyId))
                .ForPath(d => d.currency.code, opt => opt.MapFrom(s => s.CurrencyCode))

                .ForPath(d => d.supplier._id, opt => opt.MapFrom(s => s.SupplierId))
                .ForPath(d => d.supplier.code, opt => opt.MapFrom(s => s.SupplierCode))
                .ForPath(d => d.supplier.name, opt => opt.MapFrom(s => s.SupplierName))

                .ForPath(d => d.incomeTaxVm._id, opt => opt.MapFrom(s => s.IncomeTaxId))
                .ForPath(d => d.incomeTaxVm.name, opt => opt.MapFrom(s => s.IncomeTaxName))
                .ForPath(d => d.incomeTaxVm.rate, opt => opt.MapFrom(s => s.IncomeTaxRate))

                .ForPath(d => d.category._id, opt => opt.MapFrom(s => s.CategoryId))
                .ForPath(d => d.category.code, opt => opt.MapFrom(s => s.CategoryCode))
                .ForPath(d => d.category.name, opt => opt.MapFrom(s => s.CategoryName))

                .ForPath(d => d.division._id, opt => opt.MapFrom(s => s.DivisionId))
                .ForPath(d => d.division.code, opt => opt.MapFrom(s => s.DivisionCode))
                .ForPath(d => d.division.name, opt => opt.MapFrom(s => s.DivisionName))

                .ReverseMap();
        }
    }
}
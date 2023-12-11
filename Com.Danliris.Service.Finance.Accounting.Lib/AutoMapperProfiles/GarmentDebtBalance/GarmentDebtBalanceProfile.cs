using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDebtBalance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.GarmentDebtBalance
{
    public class GarmentDebtBalanceProfile: Profile
    {
        public GarmentDebtBalanceProfile()
        {
            // for view to UI
            CreateMap<GarmentDebtBalanceModel, GarmentDebtBalanceCardDto>()
                .ForMember(d => d.TotalInvoice, s => s.MapFrom(src => src.CurrencyDPPAmount + src.VATAmount - src.IncomeTaxAmount))
                .ForMember(d => d.MutationPurchase, s => s.MapFrom(src => (src.CurrencyDPPAmount + src.VATAmount - src.IncomeTaxAmount) * src.CurrencyRate))
                .ForMember(d => d.MutationPayment, s => s.MapFrom(src => src.BankExpenditureNoteInvoiceAmount))
                .ForMember(d=> d.RemainBalance , s=> s.MapFrom(src=> ((src.CurrencyDPPAmount + src.VATAmount - src.IncomeTaxAmount) * src.CurrencyRate) - src.BankExpenditureNoteInvoiceAmount))
                .ReverseMap();
        }
    }
}

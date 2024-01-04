using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDebtBalance;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.NetCore.Lib;
using Newtonsoft.Json;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance
{
    public class GarmentDebtBalanceService : IGarmentDebtBalanceService
    {
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public GarmentDebtBalanceService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
            _mapper = serviceProvider.GetService<IMapper>();
        }

        public List<GarmentDebtBalanceCardDto> GetDebtBalanceCardDto(int supplierId, int month, int year)
        {
            var garmentDebtBalance = GetData(supplierId, month, year).ToList();

            //auto map
            //List<GarmentDebtBalanceCardDto> garmentDebtDto = Mapper.Map<List<Models.GarmentDebtBalance.GarmentDebtBalanceModel>,List<GarmentDebtBalanceCardDto>>(garmentDebtBalance);
            //List<GarmentDebtBalanceCardDto> garmentDebtDto = garmentDebtBalance.Select(s=> Mapper.Map<GarmentDebtBalanceCardDto>(s)).ToList();
            List<GarmentDebtBalanceCardDto> garmentDebtDto = garmentDebtBalance.Select(s => new GarmentDebtBalanceCardDto(s)).ToList();

            return garmentDebtDto;
        }

        public List<GarmentDebtBalanceCardDto> GetDebtBalanceCardWithBeforeBalanceAndTotalDto(int supplierId, int month, int year)
        {
            var garmentDebtBalance = GetData(supplierId, month, year).OrderBy(s => s.ArrivalDate).ToList();

            //get last balance
            var queryGetAllSaldoBefore = _dbContext.GarmentDebtBalances.Where(s => s.ArrivalDate.Month < month && s.ArrivalDate.Year < year).OrderByDescending(s => s.ArrivalDate).Select(s => new GarmentDebtBalance.GarmentDebtBalanceCardDto(s)).AsQueryable();

            //ini tmbahan
            //berhasil,variable yg berakhiran AsQueryable, hrs di Tolistkan dgn varibale baru(queryGetAllSaldoBeforeList) jk ingin di gunakan lg
            var queryGetAllSaldoBeforeList = queryGetAllSaldoBefore.ToList();
            //---------------

            var lastMonthAndYearSaldo = queryGetAllSaldoBeforeList.Select(s => s.ArrivalDate).FirstOrDefault();
            //filter by last saldo year and month
            var querySaldoBefore = queryGetAllSaldoBeforeList.Where(s => s.ArrivalDate.Month < month && s.ArrivalDate.Year < year).OrderByDescending(s => s.ArrivalDate).ToList();

            var getLastQueryBefore = querySaldoBefore.FirstOrDefault();
            //var lastBalance = getLastQueryBefore == null ? 0 : getLastQueryBefore.RemainBalance;
            var lastBalance = getLastQueryBefore == null ? 0 : querySaldoBefore.Sum(s => s.TotalInvoice);
            var lastBalanceAdd = lastBalance;

            List<GarmentDebtBalanceCardDto> garmentDebtDto = garmentDebtBalance.Select(s => {

                var balance = new GarmentDebtBalanceCardDto(s);
                lastBalanceAdd += balance.TotalInvoice;
                balance.RemainBalance += lastBalanceAdd;
                return balance;
                }).ToList();

            //insert
            ;
            garmentDebtDto.Add(new GarmentDebtBalanceCardDto("<<saldo awal>>", lastBalance));
            garmentDebtDto.Add(new GarmentDebtBalanceCardDto("<<total>>", 0, 0, 0));
            //garmentDebtDto.Add(new GarmentDebtBalanceCardDto("<<saldo akhir>>",lastBalanceAdd,true));

            //garmentDebtDto.Add(new GarmentDebtBalanceCardDto("<<saldo akhir>>", 0, 0, 0,true));


            return garmentDebtDto.OrderBy(d => d.ArrivalDate).ToList();
        }

        public List<GarmentDebtBalanceCardDto> GetDebtBalanceCardWithBeforeBalanceAndSaldoAkhirDto(int supplierId, int month, int year)
        {
            var garmentDebtBalance = GetData(supplierId, month, year).OrderBy(s => s.ArrivalDate).ToList();

            //get last balance
            var queryGetAllSaldoBefore = _dbContext.GarmentDebtBalances.Where(s => s.ArrivalDate.Month < month && s.ArrivalDate.Year <= year && s.SupplierId == supplierId).OrderBy(s => s.ArrivalDate).Select(s => new GarmentDebtBalance.GarmentDebtBalanceCardDto(s)).AsQueryable();

            //ini tmbahan
            //berhasil,variable yg berakhiran AsQueryable, hrs di Tolistkan dgn varibale baru(queryGetAllSaldoBeforeList) jk ingin di gunakan lg
            var queryGetAllSaldoBeforeList = queryGetAllSaldoBefore.ToList();
            //---------------
            
            var lastMonthAndYearSaldo = queryGetAllSaldoBeforeList.Select(s => s.ArrivalDate).FirstOrDefault();
            //filter by last saldo year and month
            var querySaldoBefore = queryGetAllSaldoBeforeList.Where(s => s.ArrivalDate.Month < month && s.ArrivalDate.Year <= year && s.SupplierId == supplierId).OrderBy(s => s.ArrivalDate).ToList();


            //blm bs masuk di bawah ini, dan eror null
            var getLastQueryBefore = querySaldoBefore.FirstOrDefault();
            var lastBalance = getLastQueryBefore == null ? 0 : querySaldoBefore.Sum(s=> s.TotalInvoice);
            var lastBalanceAdd = lastBalance;

            List<GarmentDebtBalanceCardDto> garmentDebtDto = garmentDebtBalance.Select(s => {

                var balance = new GarmentDebtBalanceCardDto(s);
                lastBalanceAdd += balance.TotalInvoice;
                balance.RemainBalance += lastBalanceAdd;
                return balance;
            }).ToList();

            //insert
            garmentDebtDto.Add(new GarmentDebtBalanceCardDto("<<saldo awal>>", lastBalance));
            garmentDebtDto.Add(new GarmentDebtBalanceCardDto("<<saldo akhir>>", lastBalanceAdd, 0, 0));
            //garmentDebtDto.Add(new GarmentDebtBalanceCardDto("<<saldo akhir>>",lastBalanceAdd,true));

            //garmentDebtDto.Add(new GarmentDebtBalanceCardDto("<<saldo akhir>>", 0, 0, 0,true));


            return garmentDebtDto.OrderBy(d => d.ArrivalDate).ToList();
        }

        public int CreateFromCustoms(CustomsFormDto form)
        {
            var model = new GarmentDebtBalanceModel(form.PurchasingCategoryId, form.PurchasingCategoryName, form.BillsNo, form.PaymentBills, form.GarmentDeliveryOrderId, form.GarmentDeliveryOrderNo, form.SupplierId, form.SupplierCode, form.SupplierName, form.SupplierIsImport, form.CurrencyId, form.CurrencyCode, form.CurrencyRate, form.ProductNames, form.ArrivalDate, form.DPPAmount, form.CurrencyDPPAmount, form.PaymentType);
            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentDebtBalances.Add(model);

            _dbContext.SaveChanges();

            return model.Id;
        }

        public GarmentDebtBalanceIndexDto GetDebtBalanceCardIndex(int supplierId, int month, int year)
        {
            var query = GetDebtBalanceCardDto(supplierId, month, year);
            var result = new GarmentDebtBalanceIndexDto
            {
                Data = query,
                Count = query.Count,
                Order = new List<string>(),
                Selected = new List<string>()
            };
            return result;
        }

        public GarmentDebtBalanceIndexDto GetDebtBalanceCardWithBalanceBeforeIndex(int supplierId, int month, int year)
        {
            var query = GetDebtBalanceCardWithBeforeBalanceAndTotalDto(supplierId, month, year);
            var result = new GarmentDebtBalanceIndexDto
            {
                Data = query.OrderBy(element => element.SupplierName).ToList(),
                Count = query.Count,
                Order = new List<string>(),
                Selected = new List<string>()
            };
            return result;
        }
        public GarmentDebtBalanceIndexDto GetDebtBalanceCardWithBalanceBeforeAndRemainBalanceIndex(int supplierId, int month, int year)
        {
            var query = GetDebtBalanceCardWithBeforeBalanceAndSaldoAkhirDto(supplierId, month, year);
            var result = new GarmentDebtBalanceIndexDto
            {
                Data = query.OrderBy(element => element.SupplierName).ToList(),
                Count = query.Count,
                Order = new List<string>(),
                Selected = new List<string>()
            };
            return result;
        }

        public GarmentDebtBalanceIndexDto GetDebtBalanceCardWithBalanceBeforeAndRemainBalanceIndex(string searchingType, int page = 1, int size = 25, string order = "{}", List<string> select = null, string keyword = null, string filter = "{}")
        {
            var query = GetDebtBalanceCardWithBeforeBalanceAndSaldoAkhirDto(searchingType, page, size,order,select,keyword,filter);
            var data = query.Data.Select(s => new GarmentDebtBalanceCardDto(s)).ToList();
            var result = new GarmentDebtBalanceIndexDto
            {
                Data = data,
                Count = query.Count,
                Order = new List<string>(),
                Selected = new List<string>()
            };
            return result;
        }

        private ReadResponse<GarmentDebtBalanceModel> GetDebtBalanceCardWithBeforeBalanceAndSaldoAkhirDto(string searchingType, int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<GarmentDebtBalanceModel> Query = _dbContext.GarmentDebtBalances;

            if (searchingType == "BillsNo")
            {
                List<string> searchAttributes = new List<string>()
                {
                    "BillsNo"
                };

                Query = QueryHelper<GarmentDebtBalanceModel>.Search(Query, searchAttributes, keyword);
            }
            else if (searchingType == "PaymentBills")
            {
                List<string> searchAttributes = new List<string>()
                {
                    "PaymentBills"
                };

                Query = QueryHelper<GarmentDebtBalanceModel>.Search(Query, searchAttributes, keyword);
            }
            else
            {
                
                List<string> searchAttributes = new List<string>()
                {
                    "GarmentDeliveryOrderNo"
                };

                Query = QueryHelper<GarmentDebtBalanceModel>.Search(Query, searchAttributes, keyword);
            }

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<GarmentDebtBalanceModel>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<GarmentDebtBalanceModel>.Order(Query, OrderDictionary);

            Pageable<GarmentDebtBalanceModel> pageable = new Pageable<GarmentDebtBalanceModel>(Query, page - 1, size);
            List<GarmentDebtBalanceModel> Data = pageable.Data.ToList();
            int TotalData = pageable.TotalCount;

            return new ReadResponse<GarmentDebtBalanceModel>(Data, TotalData, OrderDictionary, new List<string>());
        }

        private IQueryable<GarmentDebtBalanceModel> GetData(int supplierId, int month, int year)
        {
            var query = _dbContext.GarmentDebtBalances.AsQueryable();
            if (supplierId > 0)
                query = query.Where(s => s.SupplierId == supplierId);

            if (month > 0)
                query = query.Where(s => s.ArrivalDate.Month == month);

            if (year > 1)
                query = query.Where(s => s.ArrivalDate.Year == year);
            return query;
        }

        public int UpdateFromBankExpenditureNote(int deliveryOrderId, BankExpenditureNoteFormDto form)
        {
            var model = _dbContext.GarmentDebtBalances.FirstOrDefault(entity => entity.GarmentDeliveryOrderId == deliveryOrderId);

            model.SetBankExpenditureNote(form.BankExpenditureNoteId, form.BankExpenditureNoteNo, form.BankExpenditureNoteInvoiceAmount, form.CurrencyBankExpenditureNoteInvoiceAmount);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);

            _dbContext.GarmentDebtBalances.Update(model);

            _dbContext.SaveChanges();

            return model.Id;
        }

        public int UpdateFromInternalNote(int deliveryOrderId, InternalNoteFormDto form)
        {
            var model = _dbContext.GarmentDebtBalances.FirstOrDefault(entity => entity.GarmentDeliveryOrderId == deliveryOrderId);

            model.SetInternalNote(form.InternalNoteId, form.InternalNoteNo);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);

            _dbContext.GarmentDebtBalances.Update(model);

            _dbContext.SaveChanges();

            return model.Id;
        }

        public int UpdateFromInvoice(int deliveryOrderId, InvoiceFormDto form)
        {
            var model = _dbContext.GarmentDebtBalances.FirstOrDefault(entity => entity.GarmentDeliveryOrderId == deliveryOrderId);

            model.SetInvoice(form.InvoiceId, form.InvoiceDate, form.InvoiceNo, form.VATAmount, form.IncomeTaxAmount, form.IsPayVAT, form.IsPayIncomeTax, form.CurrencyVATAmount, form.CurrencyIncomeTaxAmount, form.VATNo);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);

            _dbContext.GarmentDebtBalances.Update(model);

            _dbContext.SaveChanges();

            return model.Id;
        }

        public List<GarmentDebtBalanceSummaryDto> GetDebtBalanceSummary(int supplierId, int month, int year, bool isForeignCurrency, bool supplierIsImport)
        {
            //var query = _dbContext.GarmentDebtBalances.Where(entity => (entity.DPPAmount + entity.CurrencyDPPAmount + entity.VATAmount - entity.IncomeTaxAmount) - entity.BankExpenditureNoteInvoiceAmount != 0);
            var query = _dbContext.GarmentDebtBalances.Select(x => x);
            var beginningOfMonth = new DateTimeOffset(year, month, 1, 0, 0, 0, 0, TimeSpan.Zero);

            query = query.Where(entity => entity.ArrivalDate != DateTimeOffset.MinValue && (entity.ArrivalDate.AddHours(_identityService.TimezoneOffset).Month == month && entity.ArrivalDate.AddHours(_identityService.TimezoneOffset).Year == year));

            if (supplierId > 0)
                query = query.Where(entity => entity.SupplierId == supplierId);

            if (supplierIsImport)
                query = query.Where(entity => entity.SupplierIsImport == supplierIsImport);

            if (isForeignCurrency)
                query = query.Where(entity => !entity.SupplierIsImport && entity.CurrencyCode != "IDR");

            if (!isForeignCurrency && !supplierIsImport)
                query = query.Where(entity => !entity.SupplierIsImport && entity.CurrencyCode == "IDR");

            var supplierCurrencyData = new List<SupplierIdCurrencyIdDto>();

            var tempResult = query
                .GroupBy(entity => new { entity.SupplierId, entity.CurrencyId })
                .Select(entity => new
                {
                    SupplierId = entity.Key.SupplierId,
                    SupplierCode = entity.FirstOrDefault().SupplierCode,
                    SupplierName = entity.FirstOrDefault().SupplierName,
                    SupplierIsImport = entity.FirstOrDefault().SupplierIsImport,
                    CurrencyId = entity.Key.CurrencyId,
                    CurrencyCode = entity.FirstOrDefault().CurrencyCode,
                    CurrencyPurchaseAmount = entity.Sum(sum => sum.CurrencyDPPAmount + sum.CurrencyVATAmount - sum.CurrencyIncomeTaxAmount),
                    CurrencyPaymentAmount = entity.Sum(sum => sum.CurrencyBankExpenditureNoteInvoiceAmount),
                    PaymentAmount = entity.Sum(sum => sum.BankExpenditureNoteInvoiceAmount),
                    PurchaseAmount = entity.Sum(sum => sum.DPPAmount + sum.VATAmount - sum.IncomeTaxAmount)
                })
                .ToList();

            supplierCurrencyData.AddRange(tempResult.Select(element => new SupplierIdCurrencyIdDto(element.SupplierId, element.CurrencyId)));

            var initialBalanceQuery = _dbContext
                .GarmentDebtBalances
                .Where(entity => entity.ArrivalDate <= beginningOfMonth && (entity.DPPAmount + entity.CurrencyDPPAmount + entity.VATAmount - entity.IncomeTaxAmount) - entity.BankExpenditureNoteInvoiceAmount != 0)
                .GroupBy(entity => new { entity.SupplierId, entity.CurrencyId })
                .Select(entity => new
                {
                    SupplierId = entity.Key.SupplierId,
                    SupplierCode = entity.FirstOrDefault().SupplierCode,
                    SupplierName = entity.FirstOrDefault().SupplierName,
                    SupplierIsImport = entity.FirstOrDefault().SupplierIsImport,
                    CurrencyId = entity.Key.CurrencyId,
                    CurrencyCode = entity.FirstOrDefault().CurrencyCode,
                    CurrencyPurchaseAmount = entity.Sum(sum => sum.CurrencyDPPAmount + sum.CurrencyVATAmount - sum.CurrencyIncomeTaxAmount),
                    CurrencyPaymentAmount = entity.Sum(sum => sum.CurrencyBankExpenditureNoteInvoiceAmount),
                    PaymentAmount = entity.Sum(sum => sum.BankExpenditureNoteInvoiceAmount),
                    PurchaseAmount = entity.Sum(sum => sum.DPPAmount + sum.VATAmount - sum.IncomeTaxAmount)
                });

            if (supplierId > 0)
                initialBalanceQuery = initialBalanceQuery.Where(entity => entity.SupplierId == supplierId);

            if (supplierIsImport)
                initialBalanceQuery = initialBalanceQuery.Where(entity => entity.SupplierIsImport == supplierIsImport);

            if (isForeignCurrency)
                initialBalanceQuery = initialBalanceQuery.Where(entity => !entity.SupplierIsImport && entity.CurrencyCode != "IDR");

            if (!isForeignCurrency && !supplierIsImport)
                initialBalanceQuery = initialBalanceQuery.Where(entity => !entity.SupplierIsImport && entity.CurrencyCode == "IDR");

            var initialBalances = initialBalanceQuery.ToList();
            supplierCurrencyData.AddRange(initialBalances.Select(element => new SupplierIdCurrencyIdDto(element.SupplierId, element.CurrencyId)));

            var result = new List<GarmentDebtBalanceSummaryDto>();
            //foreach (var item in tempResult)
            //{
            //    var initialBalance = initialBalances.FirstOrDefault(element => element.CurrencyId == item.CurrencyId && element.SupplierId == item.SupplierId);
            //    var initialBalanceAmount = 0.0;
            //    var currencyInitialBalanceAmount = 0.0;

            //    if (initialBalance != null)
            //    {
            //        initialBalanceAmount = item.PurchaseAmount - item.PaymentAmount;
            //        currencyInitialBalanceAmount = initialBalance.CurrencyPurchaseAmount - initialBalance.CurrencyPaymentAmount;
            //    }

            //    var currentBalance = initialBalanceAmount + (item.PurchaseAmount - item.PaymentAmount);
            //    var currencyCurrentBalance = currencyInitialBalanceAmount + (item.CurrencyPurchaseAmount - item.CurrencyPaymentAmount);

            //    result.Add(new GarmentDebtBalanceSummaryDto(item.SupplierId, item.SupplierCode, item.SupplierName, item.SupplierIsImport, item.CurrencyId, item.CurrencyCode, initialBalanceAmount, item.PurchaseAmount, item.PaymentAmount, currentBalance, currencyInitialBalanceAmount, item.CurrencyPurchaseAmount, item.CurrencyPaymentAmount, currencyCurrentBalance));

            //}

            var supplierCurrencies = supplierCurrencyData.GroupBy(element => new { element.CurrencyId, element.SupplierId }).Select(element => new SupplierIdCurrencyIdDto(element.Key.SupplierId, element.Key.CurrencyId)).ToList();
            foreach (var supplierCurrency in supplierCurrencies)
            {
                var current = tempResult.FirstOrDefault(element => element.CurrencyId == supplierCurrency.CurrencyId && element.SupplierId == supplierCurrency.SupplierId);
                var initial = initialBalances.FirstOrDefault(element => element.CurrencyId == supplierCurrency.CurrencyId && element.SupplierId == supplierCurrency.SupplierId);
                var initialBalanceAmount = 0.0;
                var currencyInitialBalanceAmount = 0.0;

                if (initial != null)
                {
                    initialBalanceAmount = initial.PurchaseAmount - initial.PaymentAmount;
                    currencyInitialBalanceAmount = initial.CurrencyPurchaseAmount - initial.CurrencyPaymentAmount;
                }

                var currentBalanceAmount = 0.0;
                var currencyCurrentBalanceAmount = 0.0;

                if (current != null)
                {
                    currentBalanceAmount = initialBalanceAmount + (current.PurchaseAmount - current.PaymentAmount);
                    currencyCurrentBalanceAmount = currencyInitialBalanceAmount + (current.CurrencyPurchaseAmount - current.CurrencyPaymentAmount);
                }

                if (current != null && currentBalanceAmount != 0)
                    result.Add(new GarmentDebtBalanceSummaryDto(current.SupplierId, current.SupplierCode, current.SupplierName, current.SupplierIsImport, current.CurrencyId, current.CurrencyCode, initialBalanceAmount, current.PurchaseAmount, current.PaymentAmount, currentBalanceAmount, currencyInitialBalanceAmount, current.CurrencyPurchaseAmount, current.CurrencyPaymentAmount, currencyCurrentBalanceAmount));
                else if (current == null && initial != null && currentBalanceAmount != 0)
                    result.Add(new GarmentDebtBalanceSummaryDto(initial.SupplierId, initial.SupplierCode, initial.SupplierName, initial.SupplierIsImport, initial.CurrencyId, initial.CurrencyCode, initialBalanceAmount, 0, 0, currentBalanceAmount, currencyInitialBalanceAmount, 0, 0, currencyCurrentBalanceAmount));
            }

            var total = result.GroupBy(element => element.CurrencyId).Select(element => new GarmentDebtBalanceSummaryDto(0, "", "", false, element.Key, element.FirstOrDefault().CurrencyCode, element.Sum(sum => sum.InitialBalance), element.Sum(sum => sum.PurchaseAmount), element.Sum(sum => sum.PaymentAmount), element.Sum(sum => sum.CurrentBalance), element.Sum(sum => sum.CurrencyInitialBalance), element.Sum(sum => sum.CurrencyPurchaseAmount), element.Sum(sum => sum.CurrencyPaymentAmount), element.Sum(sum => sum.CurrencyCurrentBalance)));
            var firstRowTotal = total.FirstOrDefault();
            if (firstRowTotal != null)
                firstRowTotal.SetTotal();
            result.AddRange(total);

            return result;
        }

        public GarmentDebtBalanceSummaryAndTotalCurrencyDto GetDebtBalanceSummaryAndTotalCurrency(int supplierId, int month, int year, bool isForeignCurrency, bool supplierIsImport)
        {
            var datasummary = GetDebtBalanceSummary(supplierId, month, year, isForeignCurrency, supplierIsImport);

            var groupData = datasummary.GroupBy(
                key => key.CurrencyCode,
                val => val,
                (key, val) => new GarmentDebtBalanceSummaryTotalByCurrencyDto
                {
                    CurrencyCode = key,
                    TotalCurrencyCurrentBalance = val.Sum(s => s.CurrencyCurrentBalance),
                    TotalCurrencyInitialBalance = val.Sum(s => s.CurrencyInitialBalance),
                    TotalCurrencyPayment = val.Sum(s => s.CurrencyPaymentAmount),
                    TotalCurrencyPurchase = val.Sum(s => s.CurrencyPurchaseAmount),
                    TotalCurrentBalance = val.Sum(s => s.CurrentBalance),
                    TotalInitialBalance = val.Sum(s => s.InitialBalance),
                    TotalPayment = val.Sum(s => s.PaymentAmount),
                    TotalPurchase = val.Sum(s => s.PurchaseAmount)
                }
                ).ToList();
            return new GarmentDebtBalanceSummaryAndTotalCurrencyDto
            {
                Data = datasummary,
                GroupTotalCurrency = groupData
            };
        }

        public int RemoveBalance(int deliveryOrderId)
        {
            var model = _dbContext.GarmentDebtBalances.FirstOrDefault(element => element.GarmentDeliveryOrderId == deliveryOrderId);
            EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);
            _dbContext.GarmentDebtBalances.Update(model);

            _dbContext.SaveChanges();

            return model.Id;
        }

        public int EmptyInternalNoteValue(int deliveryOrderId)
        {
            var model = _dbContext.GarmentDebtBalances.FirstOrDefault(element => element.GarmentDeliveryOrderId == deliveryOrderId);
            model.SetInternalNote(0, null);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentDebtBalances.Update(model);

            _dbContext.SaveChanges();

            return model.Id;
        }

        public int EmptyInvoiceValue(int deliveryOrderId)
        {
            var model = _dbContext.GarmentDebtBalances.FirstOrDefault(element => element.GarmentDeliveryOrderId == deliveryOrderId);
            model.SetInvoice(0, DateTime.MinValue, null, 0, 0, false, false, 0, 0, null);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentDebtBalances.Update(model);

            _dbContext.SaveChanges();

            return model.Id;
        }

        public int EmptyBankExpenditureNoteValue(int deliveryOrderId)
        {
            var model = _dbContext.GarmentDebtBalances.FirstOrDefault(element => element.GarmentDeliveryOrderId == deliveryOrderId);
            model.SetBankExpenditureNote(0, null, 0, 0);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentDebtBalances.Update(model);

            _dbContext.SaveChanges();

            return model.Id;
        }

        public List<GarmentDebtBalanceDetailDto> GetDebtBalanceDetail(DateTimeOffset arrivalDate, GarmentDebtBalanceDetailFilterEnum supplierTypeFilter, int supplierId, int currencyId, string paymentType)
        {
            var query = _dbContext.GarmentDebtBalances.Where(entity => (entity.DPPAmount + entity.CurrencyDPPAmount + entity.VATAmount - entity.IncomeTaxAmount) - entity.BankExpenditureNoteInvoiceAmount != 0);

            query = query.Where(entity => entity.ArrivalDate != DateTimeOffset.MinValue && entity.ArrivalDate <= arrivalDate);

            if (supplierId > 0)
                query = query.Where(entity => entity.SupplierId == supplierId);

            if (supplierTypeFilter == GarmentDebtBalanceDetailFilterEnum.SupplierImport)
                query = query.Where(entity => entity.SupplierIsImport);

            if (supplierTypeFilter == GarmentDebtBalanceDetailFilterEnum.SupplierLocal)
                query = query.Where(entity => !entity.SupplierIsImport);

            if (currencyId > 0)
                query = query.Where(entity => entity.CurrencyId == currencyId);

            if (!string.IsNullOrWhiteSpace(paymentType))
                query = query.Where(entity => entity.PaymentType == paymentType);

            var queryResult = query.ToList();
            var result = new List<GarmentDebtBalanceDetailDto>();
            foreach (var element in queryResult)
            {
                var debtAging = Math.Abs((int)(arrivalDate - element.ArrivalDate).TotalDays);
                var total = element.DPPAmount + element.VATAmount - element.IncomeTaxAmount;
                var currencyTotal = element.CurrencyDPPAmount + element.CurrencyVATAmount - element.CurrencyIncomeTaxAmount;
                result.Add(new GarmentDebtBalanceDetailDto(element.SupplierId, element.SupplierCode, element.SupplierName, element.BillsNo, element.PaymentBills, element.GarmentDeliveryOrderId, element.GarmentDeliveryOrderNo, element.PaymentType, element.ArrivalDate, debtAging, element.InternalNoteId, element.InternalNoteNo, element.InvoiceId, element.InvoiceNo, element.DPPAmount, element.CurrencyDPPAmount, element.VATAmount, element.CurrencyVATAmount, element.IncomeTaxAmount, element.CurrencyIncomeTaxAmount, total, currencyTotal, element.CurrencyId, element.CurrencyCode, element.CurrencyRate, element.VATNo));
            }

            var totalResult = result.GroupBy(element => element.CurrencyId).Select(element => new GarmentDebtBalanceDetailDto(0, "", "", "", "", 0, "", "", null, 0, 0, "", 0, "", element.Sum(sum => sum.DPPAmount), element.Sum(sum => sum.CurrencyDPPAmount), element.Sum(sum => sum.VATAmount), element.Sum(sum => sum.CurrencyVATAmount), element.Sum(sum => sum.IncomeTaxAmount), element.Sum(sum => sum.CurrencyIncomeTaxAmount), element.Sum(sum => sum.Total), element.Sum(sum => sum.CurrencyTotal), element.FirstOrDefault().CurrencyId, element.FirstOrDefault().CurrencyCode, element.FirstOrDefault().CurrencyRate, "")).ToList();
            var firstTotal = totalResult.FirstOrDefault();
            if (firstTotal != null)
                firstTotal.SetTotal();

            result.AddRange(totalResult);
            return result;
        }

        //public int UpdateFromMemo(int memoDetailId, string memoNo, double memoAmount, double paymentRate)
        //{
        //    throw new NotImplementedException();
        //}

        public int UpdateFromMemo(int deliveryOrderId, int memoDetailId, string memoNo, double memoAmount, double paymentRate)
        {
            var balance = _dbContext.GarmentDebtBalances.FirstOrDefault(entity => entity.GarmentDeliveryOrderId == deliveryOrderId);

            if (balance != null)
            {
                balance.SetMemo(memoDetailId, memoNo, memoAmount, paymentRate);
                EntityExtension.FlagForUpdate(balance, _identityService.Username, UserAgent);
                _dbContext.GarmentDebtBalances.Update(balance);
            }

            return _dbContext.SaveChanges();
        }
    }
}

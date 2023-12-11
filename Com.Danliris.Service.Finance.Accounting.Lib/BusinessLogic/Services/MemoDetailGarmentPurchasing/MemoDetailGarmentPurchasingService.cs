using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseInterface;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.MemoDetailGarmentPurchasing
{
    public class MemoDetailGarmentPurchasingService : IMemoDetailGarmentPurchasingService
    {
        private const string UserAgent = "finance-service";
        public FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;
        protected DbSet<MemoDetailGarmentPurchasingModel> DbSet;
        protected DbSet<MemoDetailGarmentPurchasingDetailModel> DbSetDetail;
        private readonly IGarmentDebtBalanceService _debtBalance;

        public MemoDetailGarmentPurchasingService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;

            DbSet = _dbContext.Set<MemoDetailGarmentPurchasingModel>();
            DbSetDetail = _dbContext.Set<MemoDetailGarmentPurchasingDetailModel>();
            _debtBalance = serviceProvider.GetService<IGarmentDebtBalanceService>();
        }

        //public void CreateModel(MemoDetailGarmentPurchasingModel model)
        //{
        //    EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);


        //    foreach (var item in model.MemoDetailGarmentPurchasingDetail)
        //    {
        //        EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
        //    }
        //    DbSet.Add(model);
        //}

        public async Task<int> CreateAsync(MemoDetailGarmentPurchasingViewModel viewModel)
        {
            try
            {
                var memo = _dbContext.MemoGarmentPurchasings.FirstOrDefault(entity => entity.Id == viewModel.MemoId);
                var model = new MemoDetailGarmentPurchasingModel()
                {
                    AccountingBookId = memo.AccountingBookId,
                    AccountingBookType = memo.AccountingBookType,
                    GarmentCurrenciesCode = memo.GarmentCurrenciesCode,
                    GarmentCurrenciesId = memo.GarmentCurrenciesId,
                    GarmentCurrenciesRate = memo.GarmentCurrenciesRate,
                    MemoDate = memo.MemoDate,
                    MemoId = viewModel.MemoId,
                    MemoNo = memo.MemoNo,
                    Remarks = viewModel.Remarks
                };
                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                _dbContext.MemoDetailGarmentPurchasings.Add(model);
                await _dbContext.SaveChangesAsync();

                foreach (var item in viewModel.MemoDetailGarmentPurchasingDispositions)
                {
                    var memoItem = new MemoDetailGarmentPurchasingDispositionModel()
                    {
                        DispositionId = item.Disposition.DispositionId,
                        DispositionNo = item.Disposition.DispositionNo,
                        MemoDetailGarmentPurchasingId = model.Id
                    };
                    EntityExtension.FlagForCreate(memoItem, _identityService.Username, UserAgent);
                    _dbContext.MemoDetailGarmentPurchasingDispositions.Add(memoItem);
                    await _dbContext.SaveChangesAsync();

                    foreach (var detail in item.Disposition.MemoDetails)
                    {
                        var memoDetail = new MemoDetailGarmentPurchasingDetailModel()
                        {
                            GarmentDeliveryOrderId = detail.GarmentDeliveryOrderId,
                            GarmentDeliveryOrderNo = detail.GarmentDeliveryOrderNo,
                            MemoAmount = (int)detail.MemoAmount,
                            MemoDispositionId = memoItem.Id,
                            RemarksDetail = detail.RemarksDetail,
                            MemoDetailId = model.Id,
                            MemoId = memo.Id,
                            PaymentRate = detail.PaymentRate,
                            PurchasingRate = detail.PurchasingRate,
                            SupplierCode = detail.SupplierCode,
                            SupplierName = detail.SupplierName,
                            BillsNo = detail.BillsNo,
                            InternalNoteNo = detail.InternalNoteNo,
                            CurrencyCode = detail.CurrencyCode,
                            PaymentBills = detail.PaymentBills,
                            PurchaseAmount = detail.PurchaseAmount
                        };

                        EntityExtension.FlagForCreate(memoDetail, _identityService.Username, UserAgent);
                        _dbContext.MemoDetailGarmentPurchasingDetails.Add(memoDetail);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ReadResponse<ListMemoDetail> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            try
            {
                var query = _dbContext.MemoDetailGarmentPurchasings.AsQueryable();

                var searchAttributes = new List<string>
                {
                    "MemoNo", "AccountingBookType", "GarmentCurrenciesCode"
                };

                query = QueryHelper<MemoDetailGarmentPurchasingModel>.Search(query, searchAttributes, keyword);

                var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                query = QueryHelper<MemoDetailGarmentPurchasingModel>.Filter(query, filterDictionary);

                var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                query = QueryHelper<MemoDetailGarmentPurchasingModel>.Order(query, orderDictionary);

                var pageable = new Pageable<MemoDetailGarmentPurchasingModel>(query, page - 1, size);

                var data = pageable.Data.Select(entity => new ListMemoDetail
                {
                    Id = entity.Id,
                    MemoId = entity.MemoId,
                    MemoNo = entity.MemoNo,
                    MemoDate = entity.MemoDate,
                    AccountingBookId = entity.AccountingBookId,
                    AccountingBookType = entity.AccountingBookType,
                    GarmentCurrenciesId = entity.GarmentCurrenciesId,
                    GarmentCurrenciesCode = entity.GarmentCurrenciesCode,
                    GarmentCurrenciesRate = entity.GarmentCurrenciesRate,
                    Remarks = entity.Remarks,
                    IsPosted = entity.IsPosted
                }).ToList();
                //var data = pageable.Data.ToList();

                int totalData = pageable.TotalCount;
                return new ReadResponse<ListMemoDetail>(data, totalData, orderDictionary, new List<string>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ReadResponse<ReportRincian> GetReport(DateTimeOffset date, int page, int size, string order, List<string> select, string keyword, string filter, int valas)
        {
            try
            {
                var memoDetailsGarments = _dbContext.MemoDetailGarmentPurchasings.AsQueryable();
                var memoDetailsGarmentsDetails = _dbContext.MemoDetailGarmentPurchasingDetails.AsQueryable();
                var garmentDebts = _dbContext.GarmentDebtBalances.AsQueryable();

                var searchAttributes = new List<string>
                {
                    "AccountingBookType"
                };

                memoDetailsGarments = QueryHelper<MemoDetailGarmentPurchasingModel>.Search(memoDetailsGarments, searchAttributes, keyword);

                var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                memoDetailsGarments = QueryHelper<MemoDetailGarmentPurchasingModel>.Filter(memoDetailsGarments, filterDictionary);

                var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                memoDetailsGarments = QueryHelper<MemoDetailGarmentPurchasingModel>.Order(memoDetailsGarments, orderDictionary);

                var pageable = new Pageable<MemoDetailGarmentPurchasingModel>(memoDetailsGarments, page - 1, size);

                var memoDetails = pageable.Data.Where(s => s.MemoDate.Date.Year == date.Date.Year & s.MemoDate.Date.Month == date.Date.Month);

                var memoDetailDetails = from memoDetail in memoDetails
                                        join memoDetailsGarmentsDetail in memoDetailsGarmentsDetails
                           on memoDetail.Id equals memoDetailsGarmentsDetail.MemoDetailId
                                        select new
                                        {
                                            memoDetail.Id,
                                            memoDetail.MemoId,
                                            memoDetail.MemoNo,
                                            memoDetail.MemoDate,
                                            memoDetail.AccountingBookType,
                                            memoDetailsGarmentsDetail.MemoDetailId,
                                            memoDetailsGarmentsDetail.GarmentDeliveryOrderNo,
                                            memoDetailsGarmentsDetail.GarmentDeliveryOrderId,
                                            memoDetailsGarmentsDetail.RemarksDetail,
                                            memoDetailsGarmentsDetail.PaymentRate,
                                            memoDetailsGarmentsDetail.PurchasingRate,
                                            memoDetailsGarmentsDetail.MemoAmount,
                                            memoDetailsGarmentsDetail.MemoIdrAmount,
                                            memoDetailsGarmentsDetail.SupplierCode,
                                            memoDetailsGarmentsDetail.SupplierName
                                        };

                var reports = from memoDetailDetail in memoDetailDetails
                              join garmentDebt in garmentDebts
                              on memoDetailDetail.GarmentDeliveryOrderId equals garmentDebt.GarmentDeliveryOrderId
                              select new ReportRincian
                              {
                                  Id = memoDetailDetail.Id,
                                  MemoId = memoDetailDetail.MemoId,
                                  MemoNo = memoDetailDetail.MemoNo,
                                  MemoDate = memoDetailDetail.MemoDate,
                                  InternalNoteNo = garmentDebt.InternalNoteNo,
                                  BillsNo = garmentDebt.BillsNo,
                                  PaymentBills = garmentDebt.PaymentBills,
                                  GarmentDeliveryOrderNo = memoDetailDetail.GarmentDeliveryOrderNo,
                                  RemarksDetail = memoDetailDetail.RemarksDetail,
                                  CurrencyCode = garmentDebt.CurrencyCode,
                                  MemoAmount = memoDetailDetail.MemoAmount,
                                  MemoIdrAmount = memoDetailDetail.MemoIdrAmount,
                                  AccountingBookType = memoDetailDetail.AccountingBookType,
                                  PaymentRate = memoDetailDetail.PaymentRate,
                                  PurchasingRate = memoDetailDetail.PurchasingRate,
                                  SupplierCode = memoDetailDetail.SupplierCode,
                                  SupplierName = memoDetailDetail.SupplierName
                              };

                if (valas > -1)
                {
                    if (valas == 0)
                    {
                        reports = reports.Where(s => s.CurrencyCode == "IDR");
                    }
                    else
                    {
                        reports = reports.Where(s => s.CurrencyCode != "IDR");
                    }
                }

                int totalData = pageable.TotalCount;
                return new ReadResponse<ReportRincian>(reports.ToList(), totalData, orderDictionary, new List<string>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public DetailRincian GetDetailById(int Id)
        //{
        //    var memoGarments = _dbContext.MemoGarmentPurchasings.AsQueryable();
        //    var memoDetailsGarments = _dbContext.MemoDetailGarmentPurchasings.AsQueryable();
        //    var memoDetailsGarmentsDetails = _dbContext.MemoDetailGarmentPurchasingDetails.AsQueryable();
        //    var garmentDebts = _dbContext.GarmentDebtBalances.AsQueryable();

        //    var memoDetail = memoDetailsGarments.FirstOrDefault(s => s.Id == Id);
        //    var listDataDetails = memoDetailsGarmentsDetails.Where(m => m.MemoDetailId == Id).Select(m => new
        //    {
        //        m.Id,
        //        m.MemoDetailId,
        //        m.GarmentDeliveryOrderNo,
        //        m.GarmentDeliveryOrderId,
        //        m.RemarksDetail,
        //        m.PaymentRate,
        //        m.PurchasingRate,
        //        m.MemoAmount,
        //        m.MemoIdrAmount,
        //    });

        //    var listData = from listDataDetail in listDataDetails
        //                   join garmentDebt in garmentDebts on listDataDetail.GarmentDeliveryOrderId equals garmentDebt.GarmentDeliveryOrderId
        //                   select new DetailRincianItems
        //                   {
        //                       Id = listDataDetail.Id,
        //                       GarmentDeliveryOrderId = listDataDetail.GarmentDeliveryOrderId,
        //                       GarmentDeliveryOrderNo = listDataDetail.GarmentDeliveryOrderNo,
        //                       InternalNoteNo = garmentDebt.InternalNoteNo,
        //                       BillsNo = garmentDebt.BillsNo,
        //                       PaymentBills = garmentDebt.PaymentBills,
        //                       SupplierCode = garmentDebt.SupplierCode,
        //                       RemarksDetail = listDataDetail.RemarksDetail,
        //                       CurrencyCode = garmentDebt.CurrencyCode,
        //                       PaymentRate = listDataDetail.PaymentRate,
        //                       PurchasingRate = listDataDetail.PurchasingRate,
        //                       SaldoAkhir = garmentDebt.DPPAmount + garmentDebt.VATAmount - garmentDebt.IncomeTaxAmount,
        //                       MemoAmount = listDataDetail.MemoAmount,
        //                       MemoIdrAmount = listDataDetail.MemoIdrAmount,
        //                   };

        //    var queryJoin = memoDetailsGarments.Where(m => m.Id == Id).Select(memoDetailsGarment => new DetailRincian
        //    {
        //        Id = memoDetailsGarment.Id,
        //        MemoId = memoDetailsGarment.MemoId,
        //        IsPosted = memoDetailsGarment.IsPosted,
        //        MemoDate = memoDetailsGarment.MemoDate,
        //        MemoNo = memoDetailsGarment.MemoNo,
        //        AccountingBookId = memoDetailsGarment.AccountingBookId,
        //        AccountingBookType = memoDetailsGarment.AccountingBookType,
        //        GarmentCurrenciesId = memoDetailsGarment.GarmentCurrenciesId,
        //        GarmentCurrenciesCode = memoDetailsGarment.GarmentCurrenciesCode,
        //        GarmentCurrenciesRate = memoDetailsGarment.GarmentCurrenciesRate,
        //        Remarks = memoDetailsGarment.Remarks,
        //        Items = listData.ToList()
        //    });

        //    return queryJoin.FirstOrDefault(s => s.MemoId == memoDetail.MemoId);
        //}

        public async Task<int> UpdateAsync(int id, MemoDetailGarmentPurchasingViewModel viewModel)
        {
            DeleteDetails(id);

            var model = _dbContext.MemoDetailGarmentPurchasings.FirstOrDefault(entity => entity.Id == id);
            var memo = _dbContext.MemoGarmentPurchasings.FirstOrDefault(entity => entity.Id == model.MemoId);

            model.AccountingBookId = memo.AccountingBookId;
            model.AccountingBookType = memo.AccountingBookType;
            model.GarmentCurrenciesCode = memo.GarmentCurrenciesCode;
            model.GarmentCurrenciesId = memo.GarmentCurrenciesId;
            model.GarmentCurrenciesRate = memo.GarmentCurrenciesRate;
            model.MemoDate = memo.MemoDate;
            model.MemoId = viewModel.MemoId;
            model.MemoNo = memo.MemoNo;
            model.Remarks = viewModel.Remarks;

            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.MemoDetailGarmentPurchasings.Update(model);
            await _dbContext.SaveChangesAsync();

            foreach (var item in viewModel.MemoDetailGarmentPurchasingDispositions)
            {
                var memoItem = new MemoDetailGarmentPurchasingDispositionModel()
                {
                    DispositionId = item.Disposition.DispositionId,
                    DispositionNo = item.Disposition.DispositionNo,
                    MemoDetailGarmentPurchasingId = model.Id
                };
                EntityExtension.FlagForCreate(memoItem, _identityService.Username, UserAgent);
                _dbContext.MemoDetailGarmentPurchasingDispositions.Add(memoItem);
                await _dbContext.SaveChangesAsync();

                foreach (var detail in item.Disposition.MemoDetails)
                {
                    var memoDetail = new MemoDetailGarmentPurchasingDetailModel()
                    {
                        GarmentDeliveryOrderId = detail.GarmentDeliveryOrderId,
                        GarmentDeliveryOrderNo = detail.GarmentDeliveryOrderNo,
                        MemoAmount = (int)detail.MemoAmount,
                        MemoDispositionId = memoItem.Id,
                        RemarksDetail = detail.RemarksDetail,
                        MemoDetailId = model.Id,
                        MemoId = memo.Id,
                        PaymentRate = detail.PaymentRate,
                        PurchasingRate = detail.PurchasingRate,
                        SupplierCode = detail.SupplierCode,
                        SupplierName = detail.SupplierName,
                        BillsNo = detail.BillsNo,
                        InternalNoteNo = detail.InternalNoteNo,
                        CurrencyCode = detail.CurrencyCode,
                        PaymentBills = detail.PaymentBills,
                        PurchaseAmount = detail.PurchaseAmount
                    };

                    EntityExtension.FlagForCreate(memoDetail, _identityService.Username, UserAgent);
                    _dbContext.MemoDetailGarmentPurchasingDetails.Add(memoDetail);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return await _dbContext.SaveChangesAsync();
        }

        public void UpdateModel(int id, MemoDetailGarmentPurchasingViewModel viewModel)
        {
            //MemoDetailGarmentPurchasingModel existDb = DbSet.FirstOrDefault(entity => entity.Id == id);


            //existDb.MemoId = viewModel.MemoId;
            //existDb.Remarks = viewModel.Remarks;
            //existDb.MemoNo = viewModel.MemoNo;
            //existDb.MemoDate = viewModel.MemoDate;
            //existDb.AccountingBookId = viewModel.AccountingBookId;
            //existDb.AccountingBookType = viewModel.AccountingBookType;
            //existDb.GarmentCurrenciesId = viewModel.GarmentCurrenciesId;
            //existDb.GarmentCurrenciesCode = viewModel.GarmentCurrenciesCode;
            //existDb.GarmentCurrenciesRate = viewModel.GarmentCurrenciesRate;
            //existDb.IsPosted = viewModel.IsPosted;

            //EntityExtension.FlagForUpdate(existDb, _identityService.Username, UserAgent);
            //DbSet.Update(existDb);

            //var details = _dbContext.MemoDetailGarmentPurchasingDetails.Where(entity => entity.MemoId == id).ToList();
            //foreach (var existRow in details)
            //{
            //    EditDetailRincianItems itemModel = viewModel.Items.FirstOrDefault(p => p.Id.Equals(existRow.Id));
            //    if (itemModel == null)
            //    {
            //        EntityExtension.FlagForDelete(existRow, _identityService.Username, UserAgent);
            //    }
            //    else
            //    {
            //        existRow.GarmentDeliveryOrderId = itemModel.GarmentDeliveryOrderId;
            //        existRow.GarmentDeliveryOrderNo = itemModel.GarmentDeliveryOrderNo;
            //        existRow.RemarksDetail = itemModel.RemarksDetail;
            //        existRow.PaymentRate = itemModel.PaymentRate;
            //        existRow.PurchasingRate = itemModel.PurchasingRate;
            //        existRow.MemoAmount = itemModel.MemoAmount;
            //        existRow.MemoIdrAmount = itemModel.MemoIdrAmount;

            //        EntityExtension.FlagForUpdate(existRow, _identityService.Username, UserAgent);
            //    }

            //}

            //_dbContext.MemoDetailGarmentPurchasingDetails.UpdateRange(details);

            //foreach (var item in viewModel.Items)
            //{
            //    if (item.Id == 0)
            //    {
            //        MemoDetailGarmentPurchasingDetailModel model = new MemoDetailGarmentPurchasingDetailModel();

            //        model.GarmentDeliveryOrderId = item.GarmentDeliveryOrderId;
            //        model.GarmentDeliveryOrderNo = item.GarmentDeliveryOrderNo;
            //        model.RemarksDetail = item.RemarksDetail;
            //        model.PaymentRate = item.PaymentRate;
            //        model.PurchasingRate = item.PurchasingRate;
            //        model.MemoAmount = item.MemoAmount;
            //        model.MemoIdrAmount = item.MemoIdrAmount;
            //        model.MemoDetailId = id;

            //        EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
            //        DbSetDetail.Add(model);
            //    }
            //}
        }

        public async Task<MemoDetailGarmentPurchasingViewModel> ReadByIdAsync(int id)
        {
            var result = (MemoDetailGarmentPurchasingViewModel)null;
            var model = _dbContext.MemoDetailGarmentPurchasings.FirstOrDefault(entity => entity.Id == id);

            if (model != null)
            {
                result = new MemoDetailGarmentPurchasingViewModel()
                {
                    Id = model.Id,
                    IsPosted = model.IsPosted,
                    MemoId = model.MemoId,
                    Remarks = model.Remarks
                };

                var memoDispositions = _dbContext.MemoDetailGarmentPurchasingDispositions.Where(entity => entity.MemoDetailGarmentPurchasingId == id).ToList();
                var dispositionIds = memoDispositions.Select(element => element.Id).ToList();
                var memoDetails = _dbContext.MemoDetailGarmentPurchasingDetails.Where(entity => dispositionIds.Contains(entity.MemoDispositionId));
                var deliveryOrderIds = memoDetails.Select(element => element.GarmentDeliveryOrderId).ToList();
                //var debtBalances = _dbContext.GarmentDebtBalances.Where(entity => deliveryOrderIds.Contains(entity.GarmentDeliveryOrderId)).ToList();

                result.MemoDetailGarmentPurchasingDispositions = memoDispositions.Select(disposition =>
                {
                    var details = memoDetails.Where(detail => detail.MemoDispositionId == disposition.Id).ToList();

                    var mDetails = details.Select(detail =>
                    {
                        //var debtBalance = debtBalances.FirstOrDefault(element => element.GarmentDeliveryOrderId == detail.GarmentDeliveryOrderId);
                        return new MemoDetail()
                        {
                            GarmentDeliveryOrderId = detail.GarmentDeliveryOrderId,
                            GarmentDeliveryOrderNo = detail.GarmentDeliveryOrderNo,
                            RemarksDetail = detail.RemarksDetail,
                            PaymentRate = detail.PaymentRate,
                            PurchasingRate = detail.PurchasingRate,
                            MemoAmount = detail.MemoAmount,
                            MemoIdrAmount = detail.MemoIdrAmount,
                            BillsNo = detail.BillsNo,
                            CurrencyCode = detail.CurrencyCode,
                            InternalNoteNo = detail.InternalNoteNo,
                            PaymentBills = detail.PaymentBills,
                            SupplierCode = detail.SupplierCode,
                            SupplierName = detail.SupplierName,
                            PurchaseAmount = detail.PurchaseAmount
                        };
                    }).ToList();

                    return new MemoDetailGarmentPurchasingDispositionViewModel()
                    {
                        Disposition = new DispositionDto()
                        {
                            DispositionId = disposition.DispositionId,
                            DispositionNo = disposition.DispositionNo,
                            MemoDetails = mDetails
                        }
                    };
                }).ToList();
            }
            return result;
        }

        private void DeleteDetails(int memoId)
        {
            

            var dispositions = _dbContext.MemoDetailGarmentPurchasingDispositions.Where(entity => entity.MemoDetailGarmentPurchasingId == memoId).ToList();
            foreach (var disposition in dispositions)
            {
                EntityExtension.FlagForDelete(disposition, _identityService.Username, UserAgent, true);
                _dbContext.MemoDetailGarmentPurchasingDispositions.Update(disposition);
            }

            var dispositionIds = dispositions.Select(entity => entity.Id).ToList();
            var details = _dbContext.MemoDetailGarmentPurchasingDetails.Where(entity => dispositionIds.Contains(entity.MemoDispositionId)).ToList();
            foreach (var item in details)
            {
                EntityExtension.FlagForDelete(item, _identityService.Username, UserAgent, true);
                _dbContext.MemoDetailGarmentPurchasingDetails.Update(item);
            }

            _dbContext.SaveChanges();
        }

        public async Task DeleteModel(int id)
        {
            var model = await _dbContext.MemoDetailGarmentPurchasings.FirstOrDefaultAsync(entity => entity.Id == id);

            var details = _dbContext.MemoDetailGarmentPurchasingDetails.Where(entity => entity.MemoId == id).ToList();
            foreach (var item in details)
            {
                EntityExtension.FlagForDelete(item, _identityService.Username, UserAgent, true);
                _dbContext.MemoDetailGarmentPurchasingDetails.Update(item);
            }

            var dispositions = _dbContext.MemoDetailGarmentPurchasingDispositions.Where(entity => entity.MemoDetailGarmentPurchasingId == id).ToList();
            foreach (var disposition in dispositions)
            {
                EntityExtension.FlagForDelete(disposition, _identityService.Username, UserAgent, true);
                _dbContext.MemoDetailGarmentPurchasingDispositions.Update(disposition);
            }
            EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent, true);
            DbSet.Update(model);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            await DeleteModel(id);
            return await _dbContext.SaveChangesAsync();
        }

        public ReadResponse<ReportPDF> GetPDF(DateTimeOffset date, int page, int size, string order, List<string> select, string keyword, string filter, int valas)
        {
            try
            {
                var memoDetailsGarments = _dbContext.MemoDetailGarmentPurchasings.AsQueryable();
                var memoDetailsGarmentsDetails = _dbContext.MemoDetailGarmentPurchasingDetails.AsQueryable();
                var garmentDebts = _dbContext.GarmentDebtBalances.AsQueryable();

                var searchAttributes = new List<string>
                {
                    "AccountingBookType"
                };

                memoDetailsGarments = QueryHelper<MemoDetailGarmentPurchasingModel>.Search(memoDetailsGarments, searchAttributes, keyword);

                var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                memoDetailsGarments = QueryHelper<MemoDetailGarmentPurchasingModel>.Filter(memoDetailsGarments, filterDictionary);

                var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                memoDetailsGarments = QueryHelper<MemoDetailGarmentPurchasingModel>.Order(memoDetailsGarments, orderDictionary);

                var pageable = new Pageable<MemoDetailGarmentPurchasingModel>(memoDetailsGarments, page - 1, size);

                var memoDetails = pageable.Data.Where(s => s.MemoDate.Date.Year == date.Date.Year & s.MemoDate.Date.Month == date.Date.Month);

                var memoDetailDetails = from memoDetail in memoDetails
                                        join memoDetailsGarmentsDetail in memoDetailsGarmentsDetails
                           on memoDetail.Id equals memoDetailsGarmentsDetail.MemoDetailId
                                        select new
                                        {
                                            memoDetail.Id,
                                            memoDetail.MemoId,
                                            memoDetail.MemoNo,
                                            memoDetail.MemoDate,
                                            memoDetail.AccountingBookType,
                                            memoDetailsGarmentsDetail.MemoDetailId,
                                            memoDetailsGarmentsDetail.GarmentDeliveryOrderNo,
                                            memoDetailsGarmentsDetail.GarmentDeliveryOrderId,
                                            memoDetailsGarmentsDetail.RemarksDetail,
                                            memoDetailsGarmentsDetail.PaymentRate,
                                            memoDetailsGarmentsDetail.PurchasingRate,
                                            memoDetailsGarmentsDetail.MemoAmount,
                                            memoDetailsGarmentsDetail.MemoIdrAmount,
                                            memoDetailsGarmentsDetail.SupplierCode,
                                            memoDetailsGarmentsDetail.SupplierName,
                                        };

                var reports = from memoDetailDetail in memoDetailDetails
                              join garmentDebt in garmentDebts
                              on memoDetailDetail.GarmentDeliveryOrderId equals garmentDebt.GarmentDeliveryOrderId
                              select new ReportPDF
                              {
                                  Id = memoDetailDetail.Id,
                                  MemoId = memoDetailDetail.MemoId,
                                  MemoNo = memoDetailDetail.MemoNo,
                                  MemoDate = memoDetailDetail.MemoDate,
                                  InternalNoteNo = garmentDebt.InternalNoteNo,
                                  BillsNo = garmentDebt.BillsNo,
                                  PaymentBills = garmentDebt.PaymentBills,
                                  GarmentDeliveryOrderNo = memoDetailDetail.GarmentDeliveryOrderNo,
                                  RemarksDetail = memoDetailDetail.RemarksDetail,
                                  CurrencyCode = garmentDebt.CurrencyCode,
                                  MemoAmount = memoDetailDetail.MemoAmount,
                                  MemoIdrAmount = memoDetailDetail.MemoIdrAmount,
                                  AccountingBookType = memoDetailDetail.AccountingBookType,
                                  PurchasingRate = memoDetailDetail.PurchasingRate,
                                  SupplierCode = memoDetailDetail.SupplierCode,
                                  SupplierName = memoDetailDetail.SupplierName
                              };

                if (valas > -1)
                {
                    if (valas == 0)
                    {
                        reports = reports.Where(s => s.CurrencyCode == "IDR");
                    }
                    else
                    {
                        reports = reports.Where(s => s.CurrencyCode != "IDR");
                    }
                }

                int totalData = pageable.TotalCount;
                return new ReadResponse<ReportPDF>(reports.ToList(), totalData, orderDictionary, new List<string>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Posting(List<int> memoIds)
        {
            if (memoIds.Count > 0)
            {
                var memoDocuments = _dbContext.MemoDetailGarmentPurchasings.Where(entity => memoIds.Contains(entity.Id)).ToList();
                var memoDetails = _dbContext.MemoDetailGarmentPurchasingDetails.Where(entity => memoIds.Contains(entity.MemoDetailId)).Select(entity => new { entity.Id, entity.MemoDetailId, entity.MemoAmount, entity.GarmentDeliveryOrderId }).ToList();

                foreach (var detail in memoDetails)
                {
                    var memoDocument = memoDocuments.FirstOrDefault(element => element.Id == detail.MemoDetailId);


                    _debtBalance.UpdateFromMemo(detail.GarmentDeliveryOrderId, detail.Id, memoDocument.MemoNo, detail.MemoAmount, memoDocument.GarmentCurrenciesRate);

                    memoDocument.IsPosted = true;
                    EntityExtension.FlagForUpdate(memoDocument, _identityService.Username, UserAgent);
                    _dbContext.SaveChanges();
                }
            }

            return memoIds.Count;
        }
    }
}

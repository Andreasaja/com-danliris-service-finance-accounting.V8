using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public class RealizationVbWithPOService : IRealizationVbWithPOService
    {
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IVBRealizationDocumentExpeditionService _iVBRealizationDocumentExpeditionService;
        private const string UserAgent = "finance-service";
        protected DbSet<RealizationVbModel> _DbSet;
        protected DbSet<RealizationVbDetailModel> _DetailDbSet;

        public RealizationVbWithPOService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();
            _iVBRealizationDocumentExpeditionService = serviceProvider.GetService<IVBRealizationDocumentExpeditionService>();
            _DbSet = _dbContext.Set<RealizationVbModel>();
            _DetailDbSet = _dbContext.Set<RealizationVbDetailModel>();
        }

        public async Task<int> CreateAsync(RealizationVbModel model, RealizationVbWithPOViewModel viewmodel)
        {
            model.VBNoRealize = GetVbRealizePoNo(model);

            var updateTotalRequestVb = _dbContext.VbRequests.FirstOrDefault(x => x.VBNo == model.VBNo && x.IsDeleted == false);
            if (updateTotalRequestVb != null)
            {
                model.RequestVbName = viewmodel.numberVB.CreateBy;
                updateTotalRequestVb.Realization_Status = true;
            }

            decimal totalAmount = 0;

            foreach (var item1 in viewmodel.Items)
            {
                foreach (var item2 in item1.item)
                {
                    var temp = item2.unitReceiptNote;

                    foreach (var item3 in temp.items)
                    {
                        totalAmount += item3.PriceTotal;
                    }
                }
            };

            model.Amount = totalAmount;
            model.VBRealizeCategory = "PO";
            model.isVerified = false;
            model.isClosed = false;
            model.isNotVeridied = false;

            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);

            _dbContext.RealizationVbs.Add(model);

            //return _dbContext.SaveChangesAsync();

            await _dbContext.SaveChangesAsync();
            return await _iVBRealizationDocumentExpeditionService.InitializeExpedition(model.Id);
        }

        private string GetVbRealizePoNo(RealizationVbModel model)
        {
            var now = model.Date.LocalDateTime;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var documentNo = $"R-{month}{year}-";

            var countSameDocumentNo = _dbContext.RealizationVbs.Where(a => a.Date.Month == model.Date.Month).Count();

            if (countSameDocumentNo >= 0)
            {
                countSameDocumentNo += 1;

                documentNo += string.Format("{0:000}", countSameDocumentNo);
            }

            return documentNo;
        }

        public Task<int> DeleteAsync(int id)
        {
            var model = _dbContext.RealizationVbs.Where(entity => entity.Id == id).FirstOrDefault();

            var updateTotalRequestVb = _dbContext.VbRequests.FirstOrDefault(x => x.VBNo == model.VBNo && x.IsDeleted == false);
            updateTotalRequestVb.Realization_Status = false;

            if (model != null)
            {
                EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);

                _dbContext.RealizationVbs.Update(model);
            }

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> MappingData(RealizationVbWithPOViewModel viewmodel)
        {
            var result = new List<RealizationVbDetailModel>();

            int value = int.Parse(_DbSet.OrderByDescending(p => p.Id)
                            .Select(r => r.Id)
                            .First().ToString());

            foreach (var itm1 in viewmodel.Items)
            {
                foreach (var itm2 in itm1.item)
                {

                    var temp = itm2.unitReceiptNote;

                    foreach (var itm3 in temp.items)
                    {

                        var item = new RealizationVbDetailModel()
                        {
                            DivisionSPB = itm1.division,
                            NoSPB = itm1.no,
                            DateSPB = itm1.date,
                            SupplierCode = itm1.supplier.code,
                            SupplierName = itm1.supplier.name,
                            CurrencyId = itm1.currency._id,
                            CurrencyCode = itm1.currency.code,
                            CurrencyRate = itm1.currency.rate,
                            CurrencySymbol = itm1.currency.symbol,
                            NoPOSPB = temp.no,
                            PriceTotalSPB = itm3.PriceTotal,
                            IdProductSPB = itm3.Product._id,
                            CodeProductSPB = itm3.Product.code,
                            NameProductSPB = itm3.Product.name,
                            VBRealizationId = value
                        };

                        EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
                        _dbContext.RealizationVbDetails.Add(item);
                    }
                }
            }

            return _dbContext.SaveChangesAsync();
        }

        public ReadResponse<RealizationVbList> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.RealizationVbs.AsQueryable();

            var searchAttributes = new List<string>()
            {
                "VBNo",
                "VBNoRealize",
                "CreatedBy"
            };

            query = QueryHelper<RealizationVbModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<RealizationVbModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<RealizationVbModel>.Order(query, orderDictionary);

            var pageable = new Pageable<RealizationVbModel>(query, page - 1, size);
            var data = pageable.Data.Select(entity => new RealizationVbList()
            {
                Id = entity.Id,
                VBNo = entity.VBNo,
                VBNoRealize = entity.VBNoRealize,
                Date = entity.Date,
                DateEstimate = entity.DateEstimate,
                CreatedBy = entity.CreatedBy,
                isVerified = entity.isVerified,

            }).ToList();

            int totalData = pageable.TotalCount;

            return new ReadResponse<RealizationVbList>(data, totalData, orderDictionary, new List<string>());
        }

        public async Task<RealizationVbWithPOViewModel> ReadByIdAsync2(int id)
        {
            //throw new System.NotImplementedException();
            var model = await _dbContext.RealizationVbs.Include(entity => entity.RealizationVbDetail).Where(entity => entity.Id == id).FirstOrDefaultAsync();
            var result = new RealizationVbWithPOViewModel()
            {
                Id = model.Id,
                CreatedAgent = model.CreatedAgent,
                CreatedBy = model.CreatedBy,
                LastModifiedAgent = model.LastModifiedAgent,
                LastModifiedBy = model.LastModifiedBy,
                VBRealizationNo = model.VBNoRealize,
                Date = model.Date,
                numberVB = new DetailVB()
                {
                    VBNo = model.VBNo,
                    CreateBy = model.RequestVbName,
                    DateEstimate = model.DateEstimate,
                    UnitCode = model.UnitCode,
                    UnitName = model.UnitName,
                    Amount = model.Amount_VB
                },
                Items = model.RealizationVbDetail.Select(element => new DetailSPB()
                {
                    date = element.DateSPB,
                    division = element.DivisionSPB,
                    no = element.NoSPB,
                    supplier = new SupplierViewModel()
                    {
                        code = element.SupplierCode,
                        name = element.SupplierName
                    },
                    currency = new CurrencyViewModel()
                    {
                        _id = element.CurrencyId,
                        code = element.CurrencyCode,
                        rate = element.CurrencyRate,
                        symbol = element.CurrencySymbol
                    },
                    item = model.RealizationVbDetail.Select(s => new DetailItemSPB()
                    {
                        unitReceiptNote = new DetailunitReceiptNote()
                        {
                            no = s.NoPOSPB,
                            items = model.RealizationVbDetail.Select(v => new DetailitemunitReceiptNote()
                            {
                                PriceTotal = v.PriceTotalSPB,
                                Product = new Product_VB()
                                {
                                    _id = v.IdProductSPB,
                                    code = v.CodeProductSPB,
                                    name = v.NameProductSPB
                                }
                            }).ToList()
                        }
                    }).ToList()
                }).ToList()
            };
            return result;
            //return await _dbContext.RealizationVbs.Include(entity => entity.RealizationVbDetail).Where(entity => entity.Id == id)
            //    .Select(s =>
            //    new RealizationVbWithPOViewModel
            //    {
            //        Id = s.Id,
            //        CreatedAgent = s.CreatedAgent,
            //        CreatedBy = s.CreatedBy,
            //        LastModifiedAgent = s.LastModifiedAgent,
            //        LastModifiedBy = s.LastModifiedBy,
            //        VBRealizationNo = s.VBNoRealize,
            //        Date = s.Date,
            //        numberVB = new DetailVB()
            //        {
            //            VBNo = s.VBNo,
            //            CreateBy = s.RequestVbName,
            //            DateEstimate = s.DateEstimate,
            //            UnitCode = s.UnitCode,
            //            UnitName = s.UnitName,
            //        },
            //        Items = s.RealizationVbDetail.Select(
            //            t=> new DetailSPB 
            //            {
            //                date = t.DateSPB,
            //                division = t.DivisionSPB,
            //                no = t.NoSPB,
            //                item = s.RealizationVbDetail.Select(
            //            u => new DetailItemSPB
            //            {
            //                unitReceiptNote = new DetailunitReceiptNote()
            //                {
            //                    no = u.NoPOSPB,
            //                    items = s.RealizationVbDetail.Select(
            //                        v => new DetailitemunitReceiptNote()
            //                        {
            //                            PriceTotal = v.PriceTotalSPB,
            //                            Product = new Product_VB()
            //                            {
            //                                _id = v.IdProductSPB,
            //                                code = v.CodeProductSPB,
            //                                name = v.NameProductSPB
            //                            }
            //                        }
            //                        ).ToList()
            //                }
            //            }
            //            ).ToList()
            //            }



            //            ).ToList()
            //    }
            //).FirstOrDefaultAsync();
        }

        public Task<int> UpdateAsync(int id, RealizationVbWithPOViewModel viewmodel)
        {
            throw new System.NotImplementedException();
        }
    }
}
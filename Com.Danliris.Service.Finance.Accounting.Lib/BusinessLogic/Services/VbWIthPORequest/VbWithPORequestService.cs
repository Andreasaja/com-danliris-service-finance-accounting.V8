using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;
using Com.Moonlay.Models;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VbNonPORequest;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using System.Net.Http;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VbWIthPORequest
{
    public class VbWithPORequestService : IVbWithPORequestService
    {
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private const string UserAgent = "finance-service";
        protected DbSet<VbRequestModel> _DbSet;
        protected DbSet<VbRequestDetailModel> _DetailDbSet;
        private readonly IServiceProvider _serviceProvider;

        public VbWithPORequestService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();

            _DbSet = _dbContext.Set<VbRequestModel>();
            _DetailDbSet = _dbContext.Set<VbRequestDetailModel>();
            _serviceProvider = serviceProvider;
        }

        public ReadResponse<VbRequestWIthPOList> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.VbRequests.Where(entity => entity.VBRequestCategory == "PO").AsQueryable();

            var searchAttributes = new List<string>()
            {
                "VBNo",
                "UnitLoad",
                "CreatedBy",
                "UnitName"
            };

            query = QueryHelper<VbRequestModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<VbRequestModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VbRequestModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VbRequestModel>(query, page - 1, size);
            var data = query.Include(s => s.VbRequestDetail).Select(entity => new VbRequestWIthPOList
            {
                Id = entity.Id,
                VBNo = entity.VBNo,
                Date = entity.Date,
                DateEstimate = entity.DateEstimate,
                ApproveDate = entity.ApproveDate,
                UnitLoad = entity.UnitLoad,
                UnitId = entity.UnitId,
                UnitCode = entity.UnitCode,
                UnitName = entity.UnitName,
                CurrencyId = entity.CurrencyId,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                CurrencySymbol = entity.CurrencySymbol,
                CreatedBy = entity.CreatedBy,
                Amount = entity.Amount,
                Apporve_Status = entity.Apporve_Status,
                Complete_Status = entity.Complete_Status,
                VBRequestCategory = entity.VBRequestCategory,
                PONo = entity.VbRequestDetail.Select(s => new ModelVbPONumber
                {
                    PONo = s.PONo,
                    VBId = s.VBId,
                    DealQuantity = s.DealQuantity,
                    Price = s.Price,
                    POId = s.POId
                }).ToList()
            }).ToList();

            int totalData = pageable.TotalCount;

            return new ReadResponse<VbRequestWIthPOList>(data, totalData, orderDictionary, new List<string>());
        }

        public ReadResponse<VbRequestWIthPOList> ReadWithDateFilter(DateTimeOffset? dateFilter, int offSet, int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.VbRequests.Where(entity => entity.VBRequestCategory == "PO").AsQueryable();

            if (dateFilter.HasValue)
            {
                query = query.Where(s => dateFilter.Value.Date == s.Date.AddHours(offSet).Date);
                //query = query.Where(s => dateFilter.Value.Date == s.Date.Date);
            }

            var searchAttributes = new List<string>()
            {
                "VBNo",
                "UnitLoad",
                "CreatedBy"
            };

            query = QueryHelper<VbRequestModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<VbRequestModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VbRequestModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VbRequestModel>(query, page - 1, size);
            var data = query.Include(s => s.VbRequestDetail).Select(entity => new VbRequestWIthPOList
            {
                Id = entity.Id,
                VBNo = entity.VBNo,
                Date = entity.Date,
                DateEstimate = entity.DateEstimate,
                ApproveDate = entity.ApproveDate,
                UnitLoad = entity.UnitLoad,
                UnitId = entity.UnitId,
                UnitCode = entity.UnitCode,
                UnitName = entity.UnitName,
                CurrencyId = entity.CurrencyId,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                CurrencySymbol = entity.CurrencySymbol,
                CreatedBy = entity.CreatedBy,
                Amount = entity.VBMoney,
                Apporve_Status = entity.Apporve_Status,
                Complete_Status = entity.Complete_Status,
                VBRequestCategory = entity.VBRequestCategory,
                PONo = entity.VbRequestDetail.Select(s => new ModelVbPONumber
                {
                    PONo = s.PONo,
                    VBId = s.VBId,
                    DealQuantity = s.DealQuantity,
                    Price = s.Price,
                    POId = s.POId
                }).ToList()
            }).ToList();

            int totalData = pageable.TotalCount;

            return new ReadResponse<VbRequestWIthPOList>(data, totalData, orderDictionary, new List<string>());
        }

        public Task<int> CreateAsync(VbRequestModel model, VbWithPORequestViewModel viewmodel)
        {
            decimal Amt = 0;

            model.VBNo = GetVbNonPoNo(model);

            model.VBRequestCategory = "PO";

            model.Apporve_Status = false;
            model.Complete_Status = false;
            model.Usage_Input = viewmodel.Usage;
            string temp = "";

            foreach (var itm in viewmodel.Items)
            {
                temp += itm.unit.Name + ", ";
                temp = temp.Remove(temp.Length - 2);

                if (string.IsNullOrEmpty(itm.IncomeTax._id))
                {
                    model.IncomeTaxId = "";
                }
                else
                {
                    model.IncomeTaxId = itm.IncomeTax._id;
                }

                if (string.IsNullOrEmpty(itm.IncomeTax.Name))
                {
                    model.IncomeTaxName = "";
                }
                else
                {
                    model.IncomeTaxName = itm.IncomeTax.Name;
                }

                if (string.IsNullOrEmpty(itm.IncomeTax.Rate))
                {
                    model.IncomeTaxRate = "";
                }
                else
                {
                    model.IncomeTaxRate = itm.IncomeTax.Rate;
                }

                if (string.IsNullOrEmpty(itm.IncomeTaxBy))
                {
                    model.IncomeTaxBy = "";
                }
                else
                {
                    model.IncomeTaxBy = itm.IncomeTaxBy;
                }

                foreach (var itm2 in itm.Details)
                {
                    var price = itm2.priceBeforeTax * itm2.dealQuantity;
                    if (!itm2.includePpn && itm2.useVat)
                        price += price * (decimal)0.1;

                    Amt += price;
                }
            }

            model.Amount = Amt;
            model.UnitLoad = temp;

            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);

            _dbContext.VbRequests.Add(model);

            //foreach (var itm1 in viewmodel.Items)
            //{
            //    var updateModel = new POExternalUpdateModel()
            //    {
            //        IsCreateOnVBRequest = true
            //    };

            //    UpdateToPOExternal(itm1.no, updateModel);
            //}

            return _dbContext.SaveChangesAsync();

        }

        private string GetVbNonPoNo(VbRequestModel model)
        {
            var now = model.Date;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var documentNo = $"VB-{month}{year}-";

            var countSameDocumentNo = _dbContext.VbRequests.Where(a => a.Date.Month == model.Date.Month).Count();

            if (countSameDocumentNo >= 0)
            {
                countSameDocumentNo += 1;

                documentNo += string.Format("{0:000}", countSameDocumentNo);
            }

            return documentNo;
        }

        //private void UpdateToPOExternal(string PONo, POExternalUpdateModel model)
        //{
        //    string PurchasingUri = "external-purchase-orders/update-from-vb-with-po-req-finance/";

        //    string Uri = $"{APIEndpoint.Purchasing}{PurchasingUri}{PONo}";
        //    var data = new
        //    {
        //        model.IsCreateOnVBRequest
        //    };

        //    IHttpClientService httpClient = (IHttpClientService)this._serviceProvider.GetService(typeof(IHttpClientService));
        //    var response = httpClient.PutAsync(Uri, new StringContent(JsonConvert.SerializeObject(data).ToString(), Encoding.UTF8, General.JsonMediaType)).Result;
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception(string.Format("{0}, {1}, {2}", response.StatusCode, response.Content, "failed"));
        //    }
        //}

        public async Task<VbWithPORequestViewModel> ReadByIdAsync2(int id)
        {
            return await _dbContext.VbRequests.Include(entity => entity.VbRequestDetail).Where(entity => entity.Id == id)
                .Select(s =>
                new VbWithPORequestViewModel
                {
                    Id = s.Id,
                    CreatedAgent = s.CreatedAgent,
                    CreatedBy = s.CreatedBy,
                    LastModifiedAgent = s.LastModifiedAgent,
                    LastModifiedBy = s.LastModifiedBy,
                    VBNo = s.VBNo,
                    Date = s.Date,
                    DateEstimate = s.DateEstimate,
                    VBMoney = s.VBMoney,
                    Usage = s.Usage_Input,
                    Approve_Status = s.Apporve_Status,
                    Currency = new CurrencyVB()
                    {
                        Id = s.CurrencyId,
                        Code = s.CurrencyCode,
                        Rate = s.CurrencyRate,
                        Symbol = s.CurrencySymbol,
                        Description = s.CurrencyDescription
                    },
                    Unit = new Unit()
                    {
                        Id = s.UnitId,
                        Code = s.UnitCode,
                        Name = s.UnitName
                    },
                    Division = new Division()
                    {
                        Id = s.UnitDivisionId,
                        Name = s.UnitDivisionName
                    },
                    Items = s.VbRequestDetail.GroupBy(
                            groupkey => new { groupkey.PONo, groupkey.UnitName },
                            item => item,
                            (grpkey, item) => new { Group = grpkey, Item = item }
                        ).Select(
                        t => new VbWithPORequestDetailViewModel
                        {
                            no = t.Group.PONo,
                            unit = new Unit()
                            {
                                Name = t.Group.UnitName
                            },
                            IncomeTax = new IncomeTax()
                            {
                                _id = s.IncomeTaxId,
                                Rate = s.IncomeTaxRate,
                                Name = s.IncomeTaxName
                            },
                            IncomeTaxBy = s.IncomeTaxBy,

                            Details = t.Item.Select(
                                u => new VbWithPORequestDetailItemsViewModel
                                {
                                    Id = u.Id,
                                    Conversion = u.Conversion,
                                    dealQuantity = u.DealQuantity,
                                    dealUom = new dealUom()
                                    {
                                        _id = u.DealUOMId,
                                        unit = u.DealUOMUnit
                                    },
                                    defaultQuantity = u.DefaultQuantity,
                                    defaultUom = new defaultUom()
                                    {
                                        _id = u.DefaultUOMId,
                                        unit = u.DefaultUOMUnit
                                    },
                                    priceBeforeTax = u.Price,
                                    product = new Product_VB()
                                    {
                                        _id = u.ProductId,
                                        code = u.ProductCode,
                                        name = u.ProductName
                                    },
                                    productRemark = u.ProductRemark,
                                    includePpn = u.IsUseVat,
                                    useVat = u.POExtUseVat
                                }
                                ).ToList()
                        }
                        ).ToList()

                }
                ).FirstOrDefaultAsync();
        }

        public Task<int> UpdateAsync(int id, VbWithPORequestViewModel viewmodel)
        {
            var model = MappingData2(id, viewmodel);

            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);

            var itemIds = _dbContext.VbRequestsDetails.Where(entity => entity.VBId == id).Select(entity => entity.Id).ToList();

            foreach (var itemId in itemIds)
            {
                var item = model.VbRequestDetail.FirstOrDefault(element => element.Id == itemId);
                //if (item == null)
                //{
                    var itemToDelete = _dbContext.VbRequestsDetails.FirstOrDefault(entity => entity.Id == itemId);
                    EntityExtension.FlagForDelete(itemToDelete, _identityService.Username, UserAgent);
                    _dbContext.VbRequestsDetails.Update(itemToDelete);
                //}
                //else
                //{
                //    EntityExtension.FlagForUpdate(item, _identityService.Username, UserAgent);
                //    _dbContext.VbRequestsDetails.Update(item);
                //}
            }

            foreach (var item in model.VbRequestDetail)
            {
                if (item.Id <= 0)
                {
                    EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
                    _dbContext.VbRequestsDetails.Add(item);
                }
            }


            _dbContext.VbRequests.Update(model);

            return _dbContext.SaveChangesAsync();
        }

        public VbRequestModel MappingData2(int id, VbWithPORequestViewModel viewModel)
        {
            var listDetail = new List<VbRequestDetailModel>();

            foreach (var itm1 in viewModel.Items)
            {
                foreach (var itm2 in itm1.Details)
                {
                    var item = new VbRequestDetailModel()
                    {
                        //Id = itm2.Id,
                        VBId = id,
                        POId = itm1._id.GetValueOrDefault(),
                        PONo = itm1.no,
                        VBNo = viewModel.VBNo,
                        UnitName = itm1.unit.Name,
                        UnitId = itm1.unit.Id,
                        Conversion = itm2.Conversion,
                        DealQuantity = itm2.dealQuantity,
                        DealUOMId = itm2.dealUom._id,
                        DealUOMUnit = itm2.dealUom.unit,
                        DefaultQuantity = itm2.defaultQuantity,
                        DefaultUOMId = itm2.defaultUom._id,
                        DefaultUOMUnit = itm2.defaultUom.unit,
                        Price = itm2.priceBeforeTax,
                        ProductCode = itm2.product.code,
                        ProductId = itm2.product._id,
                        ProductName = itm2.product.name,
                        ProductRemark = itm2.productRemark,
                        LastModifiedBy = viewModel.LastModifiedBy,
                        LastModifiedAgent = viewModel.LastModifiedAgent,
                        DeletedBy = "",
                        DeletedAgent = "",
                        CreatedBy = viewModel.CreatedBy,
                        CreatedAgent = viewModel.CreatedAgent,
                        IsUseVat = itm2.includePpn,
                    };

                    listDetail.Add(item);
                }
            }

            string IncomeTaxBy = "";
            string IncomeTaxId = "";
            string IncomeTaxName = "";
            string IncomeTaxRate = "";
            string temp = "";
            decimal Amount = 0;

            foreach (var itm in viewModel.Items)
            {
                if (string.IsNullOrEmpty(itm.IncomeTax._id))
                {
                    IncomeTaxId = "";
                }
                else
                {
                    IncomeTaxId = itm.IncomeTax._id;
                }

                if (string.IsNullOrEmpty(itm.IncomeTax.Name))
                {
                    IncomeTaxName = "";
                }
                else
                {
                    IncomeTaxName = itm.IncomeTax.Name;
                }

                if (string.IsNullOrEmpty(itm.IncomeTax.Rate))
                {
                    IncomeTaxRate = "";
                }
                else
                {
                    IncomeTaxRate = itm.IncomeTax.Rate;
                }

                if (string.IsNullOrEmpty(itm.IncomeTaxBy))
                {
                    IncomeTaxBy = "";
                }
                else
                {
                    IncomeTaxBy = itm.IncomeTaxBy;
                }

                foreach (var itm2 in itm.Details)
                {
                    Amount += itm2.priceBeforeTax * itm2.dealQuantity;
                }

                IncomeTaxBy = itm.IncomeTaxBy;
                IncomeTaxId = itm.IncomeTax._id;
                IncomeTaxName = itm.IncomeTax.Name;
                IncomeTaxRate = itm.IncomeTax.Rate;

                temp += itm.unit.Name + ", ";
                temp = temp.Remove(temp.Length - 2);
            }

            var result = new VbRequestModel()
            {
                VbRequestDetail = listDetail,
                Active = viewModel.Active,

                Id = viewModel.Id,
                Date = (DateTimeOffset)viewModel.Date,
                DateEstimate = (DateTimeOffset)viewModel.DateEstimate,
                UnitId = viewModel.Unit.Id,
                UnitCode = viewModel.Unit.Code,
                UnitName = viewModel.Unit.Name,
                VBNo = viewModel.VBNo,
                CreatedUtc = viewModel.CreatedUtc,
                CreatedBy = viewModel.CreatedBy,
                CreatedAgent = viewModel.CreatedAgent,
                LastModifiedUtc = DateTime.Now,
                LastModifiedAgent = viewModel.LastModifiedAgent,
                LastModifiedBy = viewModel.LastModifiedBy,
                VBMoney = viewModel.VBMoney,
                Usage_Input = viewModel.Usage,
                CurrencyId = viewModel.Currency.Id,
                CurrencyCode = viewModel.Currency.Code,
                CurrencyRate = viewModel.Currency.Rate,
                CurrencySymbol = viewModel.Currency.Symbol,
                CurrencyDescription = viewModel.Currency.Symbol,
                Amount = Amount,
                UnitDivisionId = viewModel.Division.Id,
                UnitDivisionName = viewModel.Division.Name,
                IncomeTaxBy = IncomeTaxBy,
                IncomeTaxId = IncomeTaxId,
                IncomeTaxName = IncomeTaxName,
                IncomeTaxRate = IncomeTaxRate,
                UnitLoad = temp,
                VBRequestCategory = "PO",
                Apporve_Status = false,
                Complete_Status = false
            };

            return result;

        }

        public Task<int> DeleteAsync(int id)
        {
            var model = _dbContext.VbRequests.Include(en => en.VbRequestDetail).Where(entity => entity.Id == id).FirstOrDefault();

            //var modeldetail = _dbContext.VbRequestsDetails.Where(entity => entity.VBId == id).FirstOrDefault();

            if (model != null)
            {
                EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);

                foreach (var item in model.VbRequestDetail)
                    EntityExtension.FlagForDelete(item, _identityService.Username, UserAgent);

                _dbContext.VbRequests.Update(model);
            }

            //foreach (var itm1 in model.VbRequestDetail)
            //{
            //    var updateModel = new POExternalUpdateModel()
            //    {
            //        IsCreateOnVBRequest = false
            //    };

            //    UpdateToPOExternal(itm1.PONo.ToString(), updateModel);
            //}

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> MappingData(VbWithPORequestViewModel viewmodel)
        {
            var result = new List<VbRequestDetailModel>();

            int value = int.Parse(_DbSet.OrderByDescending(p => p.Id)
                            .Select(r => r.Id)
                            .First().ToString());

            string VbNo = _DbSet.OrderByDescending(p => p.Id)
                            .Select(r => r.VBNo)
                            .First().ToString();

            foreach (var itm1 in viewmodel.Items)
            {

                foreach (var itm2 in itm1.Details)
                {
                    var item = new VbRequestDetailModel()
                    {
                        VBNo = VbNo,
                        PONo = itm1.no,
                        //POId = itm1._id.GetValueOrDefault(),
                        UnitName = itm1.unit.Name,
                        VBId = value,
                        ProductId = itm2.product._id,
                        ProductCode = itm2.product.code,
                        ProductName = itm2.product.name,
                        DefaultQuantity = itm2.defaultQuantity,
                        DefaultUOMId = itm2.defaultUom._id,
                        DefaultUOMUnit = itm2.defaultUom.unit,
                        DealQuantity = itm2.dealQuantity,
                        DealUOMId = itm2.dealUom._id,
                        DealUOMUnit = itm2.dealUom.unit,
                        Conversion = itm2.Conversion,
                        Price = itm2.priceBeforeTax,
                        ProductRemark = itm2.productRemark,
                        IsUseVat = itm2.includePpn,
                        POExtUseVat = itm2.useVat,
                        POId = itm1._id.GetValueOrDefault(),
                        UnitId = itm1.unit.Id
                    };
                    EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
                    _dbContext.VbRequestsDetails.Add(item);
                }
            }

            return _dbContext.SaveChangesAsync();

        }
    }
}

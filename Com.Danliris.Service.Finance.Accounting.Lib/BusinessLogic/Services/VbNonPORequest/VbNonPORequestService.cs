using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Newtonsoft.Json;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.NetCore.Lib;
using Com.Moonlay.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VbNonPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VbNonPORequest;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VbNonPORequest
{
    public class VbNonPORequestService : IVbNonPORequestService
    {
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private const string UserAgent = "finance-service";
        private readonly DbSet<VbRequestModel> dbSet;

        public VbNonPORequestService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();

            dbSet = _dbContext.Set<VbRequestModel>();
        }

        public ReadResponse<VbRequestList> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.VbRequests.Include(entity => entity.VbRequestDetail).Where(entity => entity.VBRequestCategory == "NONPO").AsQueryable();

            var searchAttributes = new List<string>()
            {
                "VBNo",
                "UnitLoad",
                "CreatedBy",
                "CurrencyCode",
                "UnitName",
            };

            query = QueryHelper<VbRequestModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<VbRequestModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VbRequestModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VbRequestModel>(query, page - 1, size);
            var data = pageable.Data.Select(entity => new VbRequestList()
            {
                Id = entity.Id,
                VBNo = entity.VBNo,
                Date = entity.Date,
                DateEstimate = entity.DateEstimate,
                UnitLoad = entity.UnitLoad,
                Amount = entity.Amount,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                CurrencySymbol = entity.CurrencySymbol,
                CurrencyDescription = entity.CurrencyDescription,
                UnitId = entity.UnitId,
                UnitCode = entity.UnitCode,
                UnitName = entity.UnitName,
                UnitDivisionId = entity.UnitDivisionId,
                UnitDivisionName = entity.UnitDivisionName,
                CreatedBy = entity.CreatedBy,
                Apporve_Status = entity.Apporve_Status,
                Complete_Status = entity.Complete_Status,
                VBRequestCategory = entity.VBRequestCategory,
                Usage = entity.Usage,
                RealizationStatus = entity.Realization_Status,
                PONo = entity.VbRequestDetail.Select(en => new VbRequestDetailModel()
                {
                    UnitName = en.UnitName
                }).ToList()

            }).Where(entity => entity.VBRequestCategory == "NONPO").ToList();

            int totalData = pageable.TotalCount;

            return new ReadResponse<VbRequestList>(data, totalData, orderDictionary, new List<string>());
        }

        public ReadResponse<VbRequestList> ReadWithDateFilter(DateTimeOffset? dateFilter, int offSet, int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.VbRequests.Where(entity => entity.VBRequestCategory == "NONPO").AsQueryable();

            if(dateFilter.HasValue)
            {
                query = query.Where(s => dateFilter.Value.Date == s.Date.AddHours(offSet).Date);
                //query = query.Where(s => dateFilter.Value.Date == s.Date.Date);
            }

            var searchAttributes = new List<string>()
            {
                "VBNo",
                "UnitLoad",
                "CreatedBy",
                "CurrencyCode",
                "UnitName",
            };

            query = QueryHelper<VbRequestModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<VbRequestModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VbRequestModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VbRequestModel>(query, page - 1, size);
            var data = pageable.Data.Select(entity => new VbRequestList()
            {
                Id = entity.Id,
                VBNo = entity.VBNo,
                Date = entity.Date,
                DateEstimate = entity.DateEstimate,
                UnitLoad = entity.UnitLoad,
                Amount = entity.Amount,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                UnitId = entity.UnitId,
                UnitCode = entity.UnitCode,
                UnitName = entity.UnitName,
                UnitDivisionId = entity.UnitDivisionId,
                UnitDivisionName = entity.UnitDivisionName,
                CreatedBy = entity.CreatedBy,
                Apporve_Status = entity.Apporve_Status,
                Complete_Status = entity.Complete_Status,
                VBRequestCategory = entity.VBRequestCategory,
                Usage = entity.Usage,
                RealizationStatus = entity.Realization_Status

            }).Where(entity => entity.VBRequestCategory == "NONPO").ToList();

            int totalData = pageable.TotalCount;

            return new ReadResponse<VbRequestList>(data, totalData, orderDictionary, new List<string>());
        }

        public Task<int> CreateAsync(VbRequestModel model, VbNonPORequestViewModel viewmodel)
        {
            model.VBNo = GetVbNonPoNo(model);

            model.VBRequestCategory = "NONPO";

            model.Apporve_Status = false;
            model.Complete_Status = false;

            model.UnitLoad = GetUnitLoad(viewmodel);

            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);

            _dbContext.VbRequests.Add(model);

            return _dbContext.SaveChangesAsync();
        }
        public Task<int> MappingData(VbNonPORequestViewModel viewModel)
        {
            var result = new List<VbRequestDetailModel>();
            var myProperty = viewModel.GetType().GetProperties();

            int value = int.Parse(dbSet.OrderByDescending(p => p.Id)
                            .Select(r => r.Id)
                            .First().ToString());

            foreach (var prop in myProperty.Where(x => x.PropertyType == typeof(bool)))
            {
                if ((bool)prop.GetValue(viewModel))
                {
                    var item = new VbRequestDetailModel() { VBId = value, UnitName = prop.Name };
                    if (prop.Name.ToUpper() == "OTHERS")
                        item.DetailOthers = viewModel.DetailOthers;

                    EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
                    _dbContext.VbRequestsDetails.Add(item);
                    //_dbContext.SaveChangesAsync();

                }
            }

            return _dbContext.SaveChangesAsync();

        }

        public VbRequestModel MappingData2(VbNonPORequestViewModel viewModel)
        {
            var listDetail = new List<VbRequestDetailModel>();

            var myProperty = viewModel.GetType().GetProperties();

            int value = int.Parse(dbSet.OrderByDescending(p => p.Id)
                            .Select(r => r.Id)
                            .First().ToString());

            foreach (var prop in myProperty.Where(x => x.PropertyType == typeof(bool)))
            {
                if ((bool)prop.GetValue(viewModel))
                {
                    var item = new VbRequestDetailModel()
                    {
                        VBId = value,
                        UnitName = prop.Name,
                        LastModifiedBy = viewModel.LastModifiedBy,
                        LastModifiedAgent = viewModel.LastModifiedAgent,
                        DeletedBy = "",
                        DeletedAgent = "",
                        CreatedBy = viewModel.CreatedBy,
                        CreatedAgent = viewModel.CreatedAgent,
                    };
                    if (prop.Name.ToUpper() == "OTHERS")
                        item.DetailOthers = viewModel.DetailOthers;
                    listDetail.Add(item);
                    //EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
                    //_dbContext.VbRequestsDetails.Add(item);
                    //_dbContext.SaveChangesAsync();

                }
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
                UnitDivisionId = viewModel.Division.Id,
                UnitDivisionName = viewModel.Division.Name,
                VBNo = viewModel.VBNo,
                CurrencyId = viewModel.Currency.Id,
                CurrencyCode = viewModel.Currency.Code,
                CurrencyRate = viewModel.Currency.Rate,
                CurrencySymbol = viewModel.Currency.Symbol,
                CurrencyDescription = viewModel.Currency.Description,
                Amount = viewModel.Amount,
                Usage = viewModel.Usage,
                UnitLoad = viewModel.UnitLoad,
                CreatedBy = viewModel.CreatedBy,
                CreatedAgent = viewModel.CreatedAgent,
                LastModifiedAgent = viewModel.LastModifiedAgent,
                LastModifiedBy = viewModel.LastModifiedBy,
                LastModifiedUtc = DateTime.Now,
                CreatedUtc = viewModel.CreatedUtc
            };

            return result;

        }

        private string GetUnitLoad(VbNonPORequestViewModel viewmodel)
        {
            var UnitLoad = "";
            if (viewmodel.Spinning1 == true)
            {
                UnitLoad += "Spinning 1, ";
            }

            if (viewmodel.Spinning2 == true)
            {
                UnitLoad += "Spinning 2, ";
            }

            if (viewmodel.Spinning3 == true)
            {
                UnitLoad += "Spinning 3, ";
            }

            if (viewmodel.Weaving1 == true)
            {
                UnitLoad += "Weaving 1, ";
            }

            if (viewmodel.Weaving2 == true)
            {
                UnitLoad += "Weaving 2, ";
            }

            if (viewmodel.Finishing == true)
            {
                UnitLoad += "Finishing, ";
            }

            if (viewmodel.Printing == true)
            {
                UnitLoad += "Printing, ";
            }

            if (viewmodel.Konfeksi1A == true)
            {
                UnitLoad += "Konfeksi 1A, ";
            }

            if (viewmodel.Konfeksi1B == true)
            {
                UnitLoad += "Konfeksi 1B, ";
            }

            if (viewmodel.Konfeksi2A == true)
            {
                UnitLoad += "Konfeksi 2A, ";
            }

            if (viewmodel.Konfeksi2B == true)
            {
                UnitLoad += "Konfeksi 2B, ";
            }

            if (viewmodel.Konfeksi2C == true)
            {
                UnitLoad += "Konfeksi 2C, ";
            }

            if (viewmodel.Umum == true)
            {
                UnitLoad += "Umum, ";
            }

            if (viewmodel.Others == true)
            {
                UnitLoad += $"{viewmodel.DetailOthers}, ";
            }

            UnitLoad = UnitLoad.Remove(UnitLoad.Length - 2);

            return UnitLoad;
        }

        private string GetVbNonPoNo(VbRequestModel model)
        {
            var now = model.Date;
            var year = now.ToString("yy");
            var month = now.ToString("MM");


            //var unit = model.UnitCode.ToString().Split(" - ");


            var documentNo = $"VB-{month}{year}-";

            var countSameDocumentNo = _dbContext.VbRequests.Where(a => a.Date.Month == model.Date.Month).Count();

            if (countSameDocumentNo >= 0)
            {
                countSameDocumentNo += 1;

                documentNo += string.Format("{0:000}", countSameDocumentNo);
            }

            return documentNo;
        }

        public Task<VbRequestModel> ReadByIdAsync(int id)
        {
            return _dbContext.VbRequests.Where(entity => entity.Id == id).FirstOrDefaultAsync();
            //return _dbContext.VbRequests.Include(entity => entity.VbRequestDetail).Where(entity => entity.Id == id).FirstOrDefaultAsync();
        }

        public Task<VbNonPORequestViewModel> ReadByIdAsync2(int id)
        {
            //return _dbContext.VbRequests.Where(entity => entity.Id == id).FirstOrDefaultAsync();
            return _dbContext.VbRequests.Include(entity => entity.VbRequestDetail).Where(entity => entity.Id == id)
                .Select(s =>
                   new VbNonPORequestViewModel
                   {
                       Id = s.Id,
                       CreatedAgent = s.CreatedAgent,
                       CreatedBy = s.CreatedBy,
                       LastModifiedAgent = s.LastModifiedAgent,
                       LastModifiedBy = s.LastModifiedBy,
                       UnitLoad = s.UnitLoad,
                       VBNo = s.VBNo,
                       Date = s.Date,
                       DateEstimate = s.DateEstimate,
                       //VBCode = s.VBCode,
                       Unit = new Unit()
                       {
                           Id = s.UnitId,
                           Code = s.UnitCode,
                           Name = s.UnitName,
                           
                       },
                       Division = new Division()
                       {
                           Id = s.UnitDivisionId,
                           Name = s.UnitDivisionName
                       },
                       Currency = new CurrencyVBRequest()
                       {
                           Id = s.CurrencyId,
                           Code = s.CurrencyCode,
                           Rate = s.CurrencyRate,
                           Symbol = s.CurrencySymbol,
                           Description = s.CurrencyDescription,
                       },
                       Amount = s.Amount,
                       Usage = s.Usage,
                       Active = s.Active,
                       Approve_Status = s.Apporve_Status,
                       Realization_Status = s.Realization_Status,
                       Complete_Status = s.Complete_Status,
                       Spinning1 = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "SPINNING1").Count() >= 1,
                       Spinning2 = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "SPINNING2").Count() >= 1,
                       Spinning3 = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "SPINNING3").Count() >= 1,
                       Weaving1 = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "WEAVING1").Count() >= 1,
                       Weaving2 = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "WEAVING2").Count() >= 1,
                       Finishing = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "FINISHING").Count() >= 1,
                       Printing = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "PRINTING").Count() >= 1,
                       Konfeksi1A = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "KONFEKSI1A").Count() >= 1,
                       Konfeksi1B = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "KONFEKSI1B").Count() >= 1,
                       Konfeksi2A = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "KONFEKSI2A").Count() >= 1,
                       Konfeksi2B = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "KONFEKSI2B").Count() >= 1,
                       Konfeksi2C = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "KONFEKSI2C").Count() >= 1,
                       Umum = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "UMUM").Count() >= 1,
                       Others = s.VbRequestDetail.Where(t => t.UnitName.ToUpper() == "OTHERS").Count() >= 1,
                       DetailOthers = s.VbRequestDetail.FirstOrDefault(t => t.UnitName.ToUpper() == "OTHERS") == null ?
                       string.Empty : s.VbRequestDetail.FirstOrDefault(t => t.UnitName.ToUpper() == "OTHERS").DetailOthers
                   }
                )
                .FirstOrDefaultAsync();
        }

        public Task<int> UpdateAsync(int id, VbNonPORequestViewModel viewModel)
        {
            var model = MappingData2(viewModel);
            model.VBRequestCategory = "NONPO";
            model.UnitLoad = GetUnitLoad(viewModel);

            model.Apporve_Status = false;
            model.Complete_Status = false;

            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);

            var itemIds = _dbContext.VbRequestsDetails.Where(entity => entity.VBId == id).Select(entity => entity.Id).ToList();

            foreach (var itemId in itemIds)
            {
                var item = model.VbRequestDetail.FirstOrDefault(element => element.Id == itemId);
                if (item == null)
                {
                    var itemToDelete = _dbContext.VbRequestsDetails.FirstOrDefault(entity => entity.Id == itemId);
                    EntityExtension.FlagForDelete(itemToDelete, _identityService.Username, UserAgent);
                    _dbContext.VbRequestsDetails.Update(itemToDelete);
                }
                //else
                //{
                //    EntityExtension.FlagForUpdate(item, _identityService.Username, UserAgent);
                //    _dbContext.VbRequestsDetails.Update(item);
                //}//
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

        public Task<int> UpdateStatusAsync(List<VbRequestModel> ListVbRequestViewModel, string user)
        {

            int Updated = 0;
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var Ids = ListVbRequestViewModel.Select(d => d.Id).ToList();
                    var listData = dbSet
                        .Where(m => Ids.Contains(m.Id) && !m.IsDeleted)
                        .ToList();
                    listData.ForEach(m =>
                    {
                        EntityExtension.FlagForUpdate(m, user, UserAgent);
                        //m.Status_Post = true;
                    });

                    Updated = _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(int id)
        {
            var model = _dbContext.VbRequests.Where(entity => entity.Id == id).FirstOrDefault();

            if (model != null)
            {
                EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);

                _dbContext.VbRequests.Update(model);
            }

            return _dbContext.SaveChangesAsync();
        }
    }
}

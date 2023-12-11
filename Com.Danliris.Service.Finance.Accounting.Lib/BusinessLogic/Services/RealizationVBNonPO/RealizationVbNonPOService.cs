using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBNonPO;
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
using System.Net.Http.Headers;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VbNonPORequest;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.RealizationVBNonPO
{
    public class RealizationVbNonPOService : IRealizationVbNonPOService
    {
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IVBRealizationDocumentExpeditionService _iVBRealizationDocumentExpeditionService;
        private const string UserAgent = "finance-service";
        protected DbSet<RealizationVbModel> _DbSet;
        protected DbSet<RealizationVbDetailModel> _DetailDbSet;

        public RealizationVbNonPOService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();
            _iVBRealizationDocumentExpeditionService = serviceProvider.GetService<IVBRealizationDocumentExpeditionService>();
            _DbSet = _dbContext.Set<RealizationVbModel>();
            _DetailDbSet = _dbContext.Set<RealizationVbDetailModel>();
        }

        public async Task<int> CreateAsync(RealizationVbModel model, RealizationVbNonPOViewModel viewmodel)
        {
            if (viewmodel.TypeVBNonPO == "Dengan Nomor VB")
            {
                var updateTotalRequestVb = _dbContext.VbRequests.FirstOrDefault(x => x.VBNo == model.VBNo && x.IsDeleted == false);
                updateTotalRequestVb.Realization_Status = true;
            }

            model.TypeWithOrWithoutVB = viewmodel.TypeVBNonPO;
            model.VBNoRealize = GetVbRealizeNo(model);
            model.isVerified = false;
            model.isClosed = false;
            model.isNotVeridied = false;
            model.VBId = viewmodel.numberVB != null ? viewmodel.numberVB.Id.GetValueOrDefault() : 0;
            
            decimal temp_total = 0;
            decimal convert_total = 0;
            decimal total_vat = 0;
            decimal temp_income_amount = 0;

            foreach (var item2 in viewmodel.Items)
            {
                decimal count_total;

                if (item2.isGetPPn == true)
                {
                    decimal temp = item2.Amount.GetValueOrDefault() * 0.1m;
                    total_vat += temp;
                    count_total = item2.Amount.GetValueOrDefault() + temp;
                    convert_total += count_total;
                    temp_total += count_total;
                }
                else
                {
                    count_total = item2.Amount.GetValueOrDefault();
                    convert_total += count_total;
                    temp_total += item2.Amount.GetValueOrDefault();
                }
            }

            temp_income_amount += GetIncomeTaxAmount(viewmodel);

            model.Amount = temp_total - temp_income_amount;
            model.VatAmount = total_vat;
            model.IncomeTaxAmount = temp_income_amount;

            decimal ResultDiffReqReal = 0;
            if (viewmodel.TypeVBNonPO == "Tanpa Nomor VB")
            {
                ResultDiffReqReal = viewmodel.AmountVB - convert_total;
            }
            else
            {
                ResultDiffReqReal = viewmodel.numberVB.Amount - convert_total;
            }


            if (ResultDiffReqReal > 0)
            {
                model.DifferenceReqReal = ResultDiffReqReal;
                model.StatusReqReal = "Sisa";
            }
            else if (ResultDiffReqReal == 0)
            {
                model.DifferenceReqReal = ResultDiffReqReal;
                model.StatusReqReal = "Sesuai";
            }
            else
            {
                model.DifferenceReqReal = ResultDiffReqReal * -1;
                model.StatusReqReal = "Kurang";
            }

            string result = "";
            string val_result = "";
            if (viewmodel.TypeVBNonPO == "Tanpa Nomor VB")
            {
                if (viewmodel.Spinning1 == true)
                {
                    result += "Spinning 1,";
                    val_result += viewmodel.AmountSpinning1.ToString() + ",";
                }

                if (viewmodel.Spinning2 == true)
                {
                    result += "Spinning 2,";
                    val_result += viewmodel.AmountSpinning2.ToString() + ",";
                }

                if (viewmodel.Spinning3 == true)
                {
                    result += "Spinning 3,";
                    val_result += viewmodel.AmountSpinning3.ToString() + ",";
                }

                if (viewmodel.Weaving1 == true)
                {
                    result += "Weaving 1,";
                    val_result += viewmodel.AmountWeaving1.ToString() + ",";
                }

                if (viewmodel.Weaving2 == true)
                {
                    result += "Weaving 2,";
                    val_result += viewmodel.AmountWeaving2.ToString() + ",";
                }

                if (viewmodel.Finishing == true)
                {
                    result += "Finishing,";
                    val_result += viewmodel.AmountFinishing.ToString() + ",";
                }

                if (viewmodel.Printing == true)
                {
                    result += "Printing,";
                    val_result += viewmodel.AmountPrinting.ToString() + ",";
                }

                if (viewmodel.Konfeksi1A == true)
                {
                    result += "Konfeksi 1A,";
                    val_result += viewmodel.AmountKonfeksi1A.ToString() + ",";
                }

                if (viewmodel.Konfeksi1B == true)
                {
                    result += "Konfeksi 1B,";
                    val_result += viewmodel.AmountKonfeksi1B.ToString() + ",";
                }

                if (viewmodel.Konfeksi2A == true)
                {
                    result += "Konfeksi 2A,";
                    val_result += viewmodel.AmountKonfeksi2A.ToString() + ",";
                }

                if (viewmodel.Konfeksi2B == true)
                {
                    result += "Konfeksi 2B,";
                    val_result += viewmodel.AmountKonfeksi2B.ToString() + ",";
                }

                if (viewmodel.Konfeksi2C == true)
                {
                    result += "Konfeksi 2C,";
                    val_result += viewmodel.AmountKonfeksi2C.ToString() + ",";
                }

                if (viewmodel.Umum == true)
                {
                    result += "Umum,";
                    val_result += viewmodel.AmountUmum.ToString() + ",";
                }

                if (viewmodel.Others == true)
                {
                    result += viewmodel.DetailOthers + ",";
                    val_result += viewmodel.AmountOthers.ToString() + ",";
                }

                result = result.Remove(result.Length - 1);
                val_result = val_result.Remove(val_result.Length - 1);

                //model.VBNo = "";
                model.DateEstimate = viewmodel.DateEstimateVB.GetValueOrDefault();
                model.RequestVbName = "";
                model.UnitId = viewmodel.Unit.Id;
                model.UnitCode = viewmodel.Unit.Code;
                model.UnitName = viewmodel.Unit.Name;
                model.DateVB = viewmodel.DateVB.GetValueOrDefault();
                model.Amount_VB = 0;
                model.CurrencyCode = viewmodel.Currency.Code;
                model.CurrencyRate = viewmodel.Currency.Rate;
                model.CurrencyDescription = viewmodel.Currency.Description;
                model.CurrencySymbol = viewmodel.Currency.Symbol;
                model.VBRealizeCategory = "NONPO";
                model.UsageVBRequest = "";
                model.DivisionId = viewmodel.Division.Id;
                model.DivisionName = viewmodel.Division.Name;
            }
            else
            {
                if (viewmodel.Spinning1VB == true)
                {
                    result += "Spinning 1,";
                    val_result += viewmodel.AmountSpinning1VB.ToString() + ",";
                }

                if (viewmodel.Spinning2VB == true)
                {
                    result += "Spinning 2,";
                    val_result += viewmodel.AmountSpinning2VB.ToString() + ",";
                }

                if (viewmodel.Spinning3VB == true)
                {
                    result += "Spinning 3,";
                    val_result += viewmodel.AmountSpinning3VB.ToString() + ",";
                }

                if (viewmodel.Weaving1VB == true)
                {
                    result += "Weaving 1,";
                    val_result += viewmodel.AmountWeaving1VB.ToString() + ",";
                }

                if (viewmodel.Weaving2VB == true)
                {
                    result += "Weaving 2,";
                    val_result += viewmodel.AmountWeaving2VB.ToString() + ",";
                }

                if (viewmodel.FinishingVB == true)
                {
                    result += "Finishing,";
                    val_result += viewmodel.AmountFinishingVB.ToString() + ",";
                }

                if (viewmodel.PrintingVB == true)
                {
                    result += "Printing,";
                    val_result += viewmodel.AmountPrintingVB.ToString() + ",";
                }

                if (viewmodel.Konfeksi1AVB == true)
                {
                    result += "Konfeksi 1A,";
                    val_result += viewmodel.AmountKonfeksi1AVB.ToString() + ",";
                }

                if (viewmodel.Konfeksi1BVB == true)
                {
                    result += "Konfeksi 1B,";
                    val_result += viewmodel.AmountKonfeksi1BVB.ToString() + ",";
                }

                if (viewmodel.Konfeksi2AVB == true)
                {
                    result += "Konfeksi 2A,";
                    val_result += viewmodel.AmountKonfeksi2AVB.ToString() + ",";
                }

                if (viewmodel.Konfeksi2BVB == true)
                {
                    result += "Konfeksi 2B,";
                    val_result += viewmodel.AmountKonfeksi2BVB.ToString() + ",";
                }

                if (viewmodel.Konfeksi2CVB == true)
                {
                    result += "Konfeksi 2C,";
                    val_result += viewmodel.AmountKonfeksi2CVB.ToString() + ",";
                }

                if (viewmodel.UmumVB == true)
                {
                    result += "Umum,";
                    val_result += viewmodel.AmountUmumVB.ToString() + ",";
                }

                if (viewmodel.OthersVB == true)
                {
                    result += viewmodel.DetailOthersVB + ",";
                    val_result += viewmodel.AmountOthersVB.ToString() + ",";
                }

                result = result.Remove(result.Length - 1);
                val_result = val_result.Remove(val_result.Length - 1);
                model.DateEstimate = viewmodel.DateEstimateVB.GetValueOrDefault();
                model.RequestVbName = "";
                model.UnitId = (viewmodel.Unit?.Id).GetValueOrDefault();
                model.UnitCode = viewmodel.Unit?.Code;
                model.UnitName = viewmodel.Unit?.Name;
                model.DateVB = viewmodel.DateVB.GetValueOrDefault();
                model.Amount_VB = 0;
                model.CurrencyCode = viewmodel.Currency?.Code;
                model.CurrencyRate = (viewmodel.Currency?.Rate).GetValueOrDefault();
                model.CurrencyDescription = viewmodel.Currency?.Description;
                model.CurrencySymbol = viewmodel.Currency?.Symbol;
                model.VBRealizeCategory = "NONPO";
                model.UsageVBRequest = "";
                model.DivisionId = (viewmodel.Division?.Id).GetValueOrDefault();
                model.DivisionName = viewmodel.Division?.Name;
            }

            model.UnitLoad = result;
            model.AmountUnitLoadNoVB = val_result;


            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);

            _dbContext.RealizationVbs.Add(model);

            //return await _dbContext.SaveChangesAsync();

            await _dbContext.SaveChangesAsync();

            int value = int.Parse(_DbSet.OrderByDescending(p => p.Id)
                            .Select(r => r.Id)
                            .First().ToString());

            return await _iVBRealizationDocumentExpeditionService.InitializeExpedition(value);
        }

        //private decimal ConvertRate(decimal count, RealizationVbNonPOViewModel viewmodel)
        //{
        //    double convertCurrency;
        //    //if (viewmodel.numberVB.CurrencyCode == "IDR")
        //    //{
        //    //    convertCurrency = (double)count;
        //    //}
        //    //else
        //    //{
        //    //    convertCurrency = (Math.Round((double)count * (double)viewmodel.numberVB.CurrencyRate));
        //    //}
        //    convertCurrency = (double)count;

        //    return (decimal)convertCurrency;
        //}

        private decimal GetIncomeTaxAmount(RealizationVbNonPOViewModel viewmodel)
        {
            decimal amount = 0;

            foreach (var itm in viewmodel.Items)
            {
                if (itm.isGetPPh == true && itm.IncomeTaxBy == "Supplier")
                {
                    amount += itm.Amount.GetValueOrDefault() * (Convert.ToDecimal(itm.IncomeTax.rate.GetValueOrDefault()) / 100);
                }

            }

            return amount;
        }

        private string GetVbRealizeNo(RealizationVbModel model)
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

            if (model.TypeWithOrWithoutVB == "Dengan Nomor VB")
            {
                var updateTotalRequestVb = _dbContext.VbRequests.FirstOrDefault(x => x.VBNo == model.VBNo && x.IsDeleted == false);
                updateTotalRequestVb.Realization_Status = false;
            }

            if (model != null)
            {
                EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);

                _dbContext.RealizationVbs.Update(model);
            }

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> MappingData(RealizationVbNonPOViewModel viewmodel)
        {
            var result = new List<RealizationVbDetailModel>();

            int value = int.Parse(_DbSet.OrderByDescending(p => p.Id)
                            .Select(r => r.Id)
                            .First().ToString());

            foreach (var itm1 in viewmodel.Items)
            {
                var incometaxid = 0;
                var incometaxname = "";
                var incometaxrate = 0.0;

                if (itm1.isGetPPh == true)
                {
                    incometaxid = itm1.IncomeTax.Id.GetValueOrDefault();
                    incometaxname = itm1.IncomeTax.name;
                    incometaxrate = itm1.IncomeTax.rate.GetValueOrDefault();
                }

                var item = new RealizationVbDetailModel()
                {
                    VBRealizationId = value,
                    AmountNonPO = itm1.Amount.GetValueOrDefault(),
                    DateNonPO = itm1.DateDetail,
                    Remark = itm1.Remark,
                    isGetPPn = itm1.isGetPPn.GetValueOrDefault(),
                    isGetPPh = itm1.isGetPPh.GetValueOrDefault(),
                    IncomeTaxId = incometaxid,
                    IncomeTaxName = incometaxname,
                    IncomeTaxRate = incometaxrate,
                    IncomeTaxBy = itm1.IncomeTaxBy
                };

                EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
                _dbContext.RealizationVbDetails.Add(item);
            }

            return _dbContext.SaveChangesAsync();
        }

        public ReadResponse<RealizationVbList> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.RealizationVbs.Where(entity => entity.VBRealizeCategory == "NONPO").AsQueryable();

            var searchAttributes = new List<string>()
            {
                "VBNo",
                "VBNoRealize",
                "RequestVbName"
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
                VBRealizeCategory = entity.VBRealizeCategory

            }).Where(entity => entity.VBRealizeCategory == "NONPO").ToList();

            int totalData = pageable.TotalCount;

            return new ReadResponse<RealizationVbList>(data, totalData, orderDictionary, new List<string>());
        }

        public async Task<RealizationVbNonPOViewModel> ReadByIdAsync2(int id)
        {
            var model = await _dbContext.RealizationVbs.Include(entity => entity.RealizationVbDetail).Where(entity => entity.Id == id).FirstOrDefaultAsync();

            string res_unit = "";
            string res_amount = "";

            bool spinning1 = false;
            bool spinning2 = false;
            bool spinning3 = false;
            bool weaving1 = false;
            bool weaving2 = false;
            bool finishing = false;
            bool printing = false;
            bool konfeksi1a = false;
            bool konfeksi1b = false;
            bool konfeksi2a = false;
            bool konfeksi2b = false;
            bool konfeksi2c = false;
            bool umum = false;
            bool other = false;

            decimal amountspinning1 = 0;
            decimal amountspinning2 = 0;
            decimal amountspinning3 = 0;
            decimal amountweaving1 = 0;
            decimal amountweaving2 = 0;
            decimal amountfinishing = 0;
            decimal amountprinting = 0;
            decimal amountkonfeksi1a = 0;
            decimal amountkonfeksi1b = 0;
            decimal amountkonfeksi2a = 0;
            decimal amountkonfeksi2b = 0;
            decimal amountkonfeksi2c = 0;
            decimal amountumum = 0;
            decimal amountother = 0;

            bool spinning1VB = false;
            bool spinning2VB = false;
            bool spinning3VB = false;
            bool weaving1VB = false;
            bool weaving2VB = false;
            bool finishingVB = false;
            bool printingVB = false;
            bool konfeksi1aVB = false;
            bool konfeksi1bVB = false;
            bool konfeksi2aVB = false;
            bool konfeksi2bVB = false;
            bool konfeksi2cVB = false;
            bool umumVB = false;
            bool otherVB = false;

            decimal amountspinning1VB = 0;
            decimal amountspinning2VB = 0;
            decimal amountspinning3VB = 0;
            decimal amountweaving1VB = 0;
            decimal amountweaving2VB = 0;
            decimal amountfinishingVB = 0;
            decimal amountprintingVB = 0;
            decimal amountkonfeksi1aVB = 0;
            decimal amountkonfeksi1bVB = 0;
            decimal amountkonfeksi2aVB = 0;
            decimal amountkonfeksi2bVB = 0;
            decimal amountkonfeksi2cVB = 0;
            decimal amountumumVB = 0;
            decimal amountotherVB = 0;

            var otherUnit = "";

            if (model.TypeWithOrWithoutVB == "Tanpa Nomor VB")
            {
                res_unit = model.UnitLoad;
                res_amount = model.AmountUnitLoadNoVB;

                string[] unitload = res_unit.Split(",");
                string[] amountload = res_amount.Split(",");

                for (int i = 0; i < unitload.Length; i++)
                {
                    if (unitload[i] == "Spinning 1")
                    {
                        spinning1 = true;
                        amountspinning1 = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Spinning 2")
                    {
                        spinning2 = true;
                        amountspinning2 = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Spinning 3")
                    {
                        spinning3 = true;
                        amountspinning3 = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Weaving 1")
                    {
                        weaving1 = true;
                        amountweaving1 = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Weaving 2")
                    {
                        weaving2 = true;
                        amountweaving2 = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Finishing")
                    {
                        finishing = true;
                        amountfinishing = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Printing")
                    {
                        printing = true;
                        amountprinting = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Konfeksi 1A")
                    {
                        konfeksi1a = true;
                        amountkonfeksi1a = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Konfeksi 1B")
                    {
                        konfeksi1b = true;
                        amountkonfeksi1b = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Konfeksi 2A")
                    {
                        konfeksi2a = true;
                        amountkonfeksi2a = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Konfeksi 2B")
                    {
                        konfeksi2b = true;
                        amountkonfeksi2b = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Konfeksi 2C")
                    {
                        konfeksi2c = true;
                        amountkonfeksi2c = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Umum")
                    {
                        umum = true;
                        amountumum = Convert.ToDecimal(amountload[i]);
                    }
                    else
                    {
                        otherUnit = unitload[i];
                        other = true;
                        amountother = Convert.ToDecimal(amountload[i]);
                    }
                }

            }
            else
            {
                res_unit = model.UnitLoad;
                res_amount = model.AmountUnitLoadNoVB;

                string[] unitload = res_unit.Split(",");
                string[] amountload = res_amount.Split(",");

                for (int i = 0; i < unitload.Length; i++)
                {
                    if (unitload[i] == "Spinning 1")
                    {
                        spinning1VB = true;
                        amountspinning1VB = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Spinning 2")
                    {
                        spinning2VB = true;
                        amountspinning2VB = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Spinning 3")
                    {
                        spinning3VB = true;
                        amountspinning3VB = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Weaving 1")
                    {
                        weaving1VB = true;
                        amountweaving1VB = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Weaving 2")
                    {
                        weaving2VB = true;
                        amountweaving2 = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Finishing")
                    {
                        finishingVB = true;
                        amountfinishingVB = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Printing")
                    {
                        printingVB = true;
                        amountprintingVB = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Konfeksi 1A")
                    {
                        konfeksi1aVB = true;
                        amountkonfeksi1aVB = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Konfeksi 1B")
                    {
                        konfeksi1bVB = true;
                        amountkonfeksi1bVB = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Konfeksi 2A")
                    {
                        konfeksi2aVB = true;
                        amountkonfeksi2aVB = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Konfeksi 2B")
                    {
                        konfeksi2bVB = true;
                        amountkonfeksi2bVB = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Konfeksi 2C")
                    {
                        konfeksi2cVB = true;
                        amountkonfeksi2cVB = Convert.ToDecimal(amountload[i]);
                    }
                    else if (unitload[i] == "Umum")
                    {
                        umumVB = true;
                        amountumumVB = Convert.ToDecimal(amountload[i]);
                    }
                    else
                    {
                        otherUnit = unitload[i];
                        otherVB = true;
                        amountotherVB = Convert.ToDecimal(amountload[i]);
                    }
                }
            }

            var result = new RealizationVbNonPOViewModel()
            {
                Id = model.Id,
                CreatedAgent = model.CreatedAgent,
                CreatedBy = model.CreatedBy,
                LastModifiedAgent = model.LastModifiedAgent,
                LastModifiedBy = model.LastModifiedBy,
                VBRealizationNo = model.VBNoRealize,
                Date = model.Date,
                TypeVBNonPO = model.TypeWithOrWithoutVB,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = model.Amount_VB,
                    CreatedBy = model.RequestVbName,
                    CurrencyCode = model.CurrencyCode,
                    CurrencyRate = model.CurrencyRate,
                    CurrencyDescription = model.CurrencyDescription,
                    CurrencySymbol = model.CurrencySymbol,
                    Date = model.DateVB,
                    DateEstimate = model.DateEstimate,
                    UnitCode = model.UnitCode,
                    UnitLoad = res_unit,
                    UnitName = model.UnitName,
                    VBNo = model.VBNo,
                    VBRequestCategory = model.VBRealizeCategory
                },

                Unit = new Unit()
                {
                    Id = model.UnitId,
                    Code = model.UnitCode,
                    Name = model.UnitName
                },
                Division = new Division()
                {
                    Id = model.DivisionId,
                    Name = model.DivisionName
                },
                Currency = new CurrencyVBRequest()
                {
                    //Id = 0,
                    Code = model.CurrencyCode,
                    Rate = model.CurrencyRate,
                    Description = model.CurrencyDescription,
                    Symbol = model.CurrencySymbol
                },

                AmountVB = model.Amount,
                DateVB = model.DateVB,
                DateEstimateVB = model.DateEstimate,
                DetailOthers = otherUnit,

                Spinning1 = spinning1,
                Spinning2 = spinning2,
                Spinning3 = spinning3,
                Weaving1 = weaving1,
                Weaving2 = weaving2,
                Finishing = finishing,
                Printing = printing,
                Konfeksi1A = konfeksi1a,
                Konfeksi1B = konfeksi1b,
                Konfeksi2A = konfeksi2a,
                Konfeksi2B = konfeksi2b,
                Konfeksi2C = konfeksi2c,
                Umum = umum,
                Others = other,

                AmountSpinning1 = amountspinning1,
                AmountSpinning2 = amountspinning2,
                AmountSpinning3 = amountspinning3,
                AmountWeaving1 = amountweaving1,
                AmountWeaving2 = amountweaving2,
                AmountFinishing = amountfinishing,
                AmountPrinting = amountprinting,
                AmountKonfeksi1A = amountkonfeksi1a,
                AmountKonfeksi1B = amountkonfeksi1b,
                AmountKonfeksi2A = amountkonfeksi2a,
                AmountKonfeksi2B = amountkonfeksi2b,
                AmountKonfeksi2C = amountkonfeksi2c,
                AmountUmum = amountumum,
                AmountOthers = amountother,

                Spinning1VB = spinning1VB,
                Spinning2VB = spinning2VB,
                Spinning3VB = spinning3VB,
                Weaving1VB = weaving1VB,
                Weaving2VB = weaving2VB,
                FinishingVB = finishingVB,
                PrintingVB = printingVB,
                Konfeksi1AVB = konfeksi1aVB,
                Konfeksi1BVB = konfeksi1bVB,
                Konfeksi2AVB = konfeksi2aVB,
                Konfeksi2BVB = konfeksi2bVB,
                Konfeksi2CVB = konfeksi2cVB,
                UmumVB = umumVB,
                OthersVB = otherVB,

                AmountSpinning1VB = amountspinning1VB,
                AmountSpinning2VB = amountspinning2VB,
                AmountSpinning3VB = amountspinning3VB,
                AmountWeaving1VB = amountweaving1VB,
                AmountWeaving2VB = amountweaving2VB,
                AmountFinishingVB = amountfinishingVB,
                AmountPrintingVB = amountprintingVB,
                AmountKonfeksi1AVB = amountkonfeksi1aVB,
                AmountKonfeksi1BVB = amountkonfeksi1bVB,
                AmountKonfeksi2AVB = amountkonfeksi2aVB,
                AmountKonfeksi2BVB = amountkonfeksi2bVB,
                AmountKonfeksi2CVB = amountkonfeksi2cVB,
                AmountUmumVB = amountumumVB,
                AmountOthersVB = amountotherVB,

                Items = model.RealizationVbDetail.Select(s => new VbNonPORequestDetailViewModel()
                {

                    DateDetail = s.DateNonPO,
                    Remark = s.Remark,
                    Amount = s.AmountNonPO,
                    isGetPPn = s.isGetPPn,
                    isGetPPh = s.isGetPPh,
                    Total = s.Total,
                    IncomeTax = new IncomeTaxNew()
                    {
                        Id = s.IncomeTaxId,
                        name = s.IncomeTaxName,
                        rate = s.IncomeTaxRate,
                    },
                    IncomeTaxBy = s.IncomeTaxBy

                }).ToList()
            };

            return result;
        }

        public Task<int> UpdateAsync(int id, RealizationVbNonPOViewModel viewModel)
        {
            var model = MappingData2(id, viewModel);
            model.VBId = viewModel.numberVB != null ? viewModel.numberVB.Id.GetValueOrDefault() : 0;


            if (viewModel.TypeVBNonPO == "Dengan Nomor VB")
            {
                var updateTotalRequestVb = _dbContext.VbRequests.FirstOrDefault(x => x.VBNo == model.VBNo && x.IsDeleted == false);
                updateTotalRequestVb.Realization_Status = true;
            }

            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);

            var itemIds = _dbContext.RealizationVbDetails.Where(entity => entity.VBRealizationId == id).Select(entity => entity.Id).ToList();

            foreach (var itemId in itemIds)
            {
                var item = model.RealizationVbDetail.FirstOrDefault(element => element.Id == itemId);

                if (item == null)
                {
                    var itemToDelete = _dbContext.RealizationVbDetails.FirstOrDefault(entity => entity.Id == itemId);
                    EntityExtension.FlagForDelete(itemToDelete, _identityService.Username, UserAgent);
                    _dbContext.RealizationVbDetails.Update(itemToDelete);
                }
                //else
                //{
                //    EntityExtension.FlagForUpdate(item, _identityService.Username, UserAgent);
                //    _dbContext.RealizationVbDetails.Update(item);
                //}
            }

            foreach (var item in model.RealizationVbDetail)
            {
                if (item.Id <= 0)
                {
                    EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
                    _dbContext.RealizationVbDetails.Add(item);
                }
            }

            model.DateEstimate = viewModel.DateEstimateVB.GetValueOrDefault();
            model.RequestVbName = "";
            model.UnitId = (viewModel.Unit?.Id).GetValueOrDefault();
            model.UnitCode = viewModel.Unit?.Code;
            model.UnitName = viewModel.Unit?.Name;
            model.DateVB = viewModel.DateVB.GetValueOrDefault();
            model.Amount_VB = 0;
            model.CurrencyCode = viewModel.Currency?.Code;
            model.CurrencyRate = (viewModel.Currency?.Rate).GetValueOrDefault();
            model.CurrencyDescription = viewModel.Currency?.Description;
            model.CurrencySymbol = viewModel.Currency?.Symbol;
            model.VBRealizeCategory = "NONPO";
            model.UsageVBRequest = "";
            model.DivisionId = (viewModel.Division?.Id).GetValueOrDefault();
            model.DivisionName = viewModel.Division?.Name;

            _dbContext.RealizationVbs.Update(model);
            _dbContext.SaveChanges();

            return _iVBRealizationDocumentExpeditionService.UpdateExpeditionByRealizationId(model.Id);
        }

        public RealizationVbModel MappingData2(int id, RealizationVbNonPOViewModel viewModel)
        {
            var listDetail = new List<RealizationVbDetailModel>();

            decimal temp_total = 0;
            decimal convert_total = 0;
            decimal total_vat = 0;

            foreach (var itm in viewModel.Items)
            {

                decimal count_total;
                if (itm.isGetPPn == true)
                {
                    decimal temp = itm.Amount.GetValueOrDefault() * 0.1m;
                    total_vat += temp;
                    count_total = itm.Amount.GetValueOrDefault() + temp;
                    convert_total += count_total;
                    temp_total += count_total;
                }
                else
                {
                    count_total = itm.Amount.GetValueOrDefault();
                    convert_total += count_total;
                    temp_total += itm.Amount.GetValueOrDefault();
                }

                var item = new RealizationVbDetailModel()
                {
                    VBRealizationId = id,
                    LastModifiedBy = viewModel.LastModifiedBy,
                    LastModifiedAgent = viewModel.LastModifiedAgent,
                    DeletedBy = "",
                    DeletedAgent = "",
                    CreatedBy = viewModel.CreatedBy,
                    CreatedAgent = viewModel.CreatedAgent,
                    DateNonPO = itm.DateDetail,
                    Remark = itm.Remark,
                    AmountNonPO = itm.Amount.GetValueOrDefault(),
                    isGetPPn = itm.isGetPPn.GetValueOrDefault(),
                    isGetPPh = itm.isGetPPh.GetValueOrDefault(),
                    IncomeTaxId = itm.IncomeTax.Id.GetValueOrDefault(),
                    IncomeTaxName = itm.IncomeTax.name,
                    IncomeTaxRate = itm.IncomeTax.rate.GetValueOrDefault(),
                    IncomeTaxBy = itm.IncomeTaxBy,
                    Total = itm.Total.GetValueOrDefault()
                };

                listDetail.Add(item);
            }

            var ResultDiffReqReal = viewModel.numberVB.Amount - convert_total;
            string StatusReqReal;


            decimal DifferenceReqReal;
            if (ResultDiffReqReal > 0)
            {
                DifferenceReqReal = ResultDiffReqReal;
                StatusReqReal = "Sisa";
            }
            else if (ResultDiffReqReal == 0)
            {
                DifferenceReqReal = ResultDiffReqReal;
                StatusReqReal = "Sesuai";
            }
            else
            {
                DifferenceReqReal = ResultDiffReqReal * -1;
                StatusReqReal = "Kurang";
            }

            string res = "";
            string val_result = "";
            if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
            {
                if (viewModel.Spinning1 == true)
                {
                    res += "Spinning 1,";
                    val_result += viewModel.AmountSpinning1.ToString() + ",";
                }

                if (viewModel.Spinning2 == true)
                {
                    res += "Spinning 2,";
                    val_result += viewModel.AmountSpinning2.ToString() + ",";
                }

                if (viewModel.Spinning3 == true)
                {
                    res += "Spinning 3,";
                    val_result += viewModel.AmountSpinning3.ToString() + ",";
                }

                if (viewModel.Weaving1 == true)
                {
                    res += "Weaving 1,";
                    val_result += viewModel.AmountWeaving1.ToString() + ",";
                }

                if (viewModel.Weaving2 == true)
                {
                    res += "Weaving 2,";
                    val_result += viewModel.AmountWeaving2.ToString() + ",";
                }

                if (viewModel.Finishing == true)
                {
                    res += "Finishing,";
                    val_result += viewModel.AmountFinishing.ToString() + ",";
                }

                if (viewModel.Printing == true)
                {
                    res += "Printing,";
                    val_result += viewModel.AmountPrinting.ToString() + ",";
                }

                if (viewModel.Konfeksi1A == true)
                {
                    res += "Konfeksi 1A,";
                    val_result += viewModel.AmountKonfeksi1A.ToString() + ",";
                }

                if (viewModel.Konfeksi1B == true)
                {
                    res += "Konfeksi 1B,";
                    val_result += viewModel.AmountKonfeksi1B.ToString() + ",";
                }

                if (viewModel.Konfeksi2A == true)
                {
                    res += "Konfeksi 2A,";
                    val_result += viewModel.AmountKonfeksi2A.ToString() + ",";
                }

                if (viewModel.Konfeksi2B == true)
                {
                    res += "Konfeksi 2B,";
                    val_result += viewModel.AmountKonfeksi2B.ToString() + ",";
                }

                if (viewModel.Konfeksi2C == true)
                {
                    res += "Konfeksi 2C,";
                    val_result += viewModel.AmountKonfeksi2C.ToString() + ",";
                }

                if (viewModel.Umum == true)
                {
                    res += "Umum,";
                    val_result += viewModel.AmountUmum.ToString() + ",";
                }

                if (viewModel.Others == true)
                {
                    res += viewModel.DetailOthers + ",";
                    val_result += viewModel.AmountOthers.ToString() + ",";
                }

                res = res.Remove(res.Length - 1);
                val_result = val_result.Remove(val_result.Length - 1);

            }
            else
            {
                if (viewModel.Spinning1VB == true)
                {
                    res += "Spinning 1,";
                    val_result += viewModel.AmountSpinning1VB.ToString() + ",";
                }

                if (viewModel.Spinning2VB == true)
                {
                    res += "Spinning 2,";
                    val_result += viewModel.AmountSpinning2VB.ToString() + ",";
                }

                if (viewModel.Spinning3VB == true)
                {
                    res += "Spinning 3,";
                    val_result += viewModel.AmountSpinning3VB.ToString() + ",";
                }

                if (viewModel.Weaving1VB == true)
                {
                    res += "Weaving 1,";
                    val_result += viewModel.AmountWeaving1VB.ToString() + ",";
                }

                if (viewModel.Weaving2VB == true)
                {
                    res += "Weaving 2,";
                    val_result += viewModel.AmountWeaving2VB.ToString() + ",";
                }

                if (viewModel.FinishingVB == true)
                {
                    res += "Finishing,";
                    val_result += viewModel.AmountFinishingVB.ToString() + ",";
                }

                if (viewModel.PrintingVB == true)
                {
                    res += "Printing,";
                    val_result += viewModel.AmountPrintingVB.ToString() + ",";
                }

                if (viewModel.Konfeksi1AVB == true)
                {
                    res += "Konfeksi 1A,";
                    val_result += viewModel.AmountKonfeksi1AVB.ToString() + ",";
                }

                if (viewModel.Konfeksi1BVB == true)
                {
                    res += "Konfeksi 1B,";
                    val_result += viewModel.AmountKonfeksi1BVB.ToString() + ",";
                }

                if (viewModel.Konfeksi2AVB == true)
                {
                    res += "Konfeksi 2A,";
                    val_result += viewModel.AmountKonfeksi2AVB.ToString() + ",";
                }

                if (viewModel.Konfeksi2BVB == true)
                {
                    res += "Konfeksi 2B,";
                    val_result += viewModel.AmountKonfeksi2BVB.ToString() + ",";
                }

                if (viewModel.Konfeksi2CVB == true)
                {
                    res += "Konfeksi 2C,";
                    val_result += viewModel.AmountKonfeksi2CVB.ToString() + ",";
                }

                if (viewModel.UmumVB == true)
                {
                    res += "Umum,";
                    val_result += viewModel.AmountUmumVB.ToString() + ",";
                }

                if (viewModel.OthersVB == true)
                {
                    res += viewModel.DetailOthersVB + ",";
                    val_result += viewModel.AmountOthersVB.ToString() + ",";
                }

                res = res.Remove(res.Length - 1);
                val_result = val_result.Remove(val_result.Length - 1);
            }

            string unitload = "";
            string amountunitloadnovb = "";
            string vbno = "";
            DateTimeOffset dateestimate;
            string requestvbname = "";
            int unitid = 0;
            string unitcode = "";
            string unitname = "";
            DateTimeOffset datevb;
            decimal amount_vb = 0;
            string currencycode = "";
            decimal currencyrate = 0;
            string currencydescription = "";
            string currencysymbol = "";
            string vbrealizecategory = "";
            string usagevbrequest = "";
            int divisionid = 0;
            string divisioname = "";

            if (viewModel.TypeVBNonPO == "Tanpa Nomor VB")
            {
                unitload = res;
                amountunitloadnovb = val_result;
                vbno = viewModel.numberVB != null ? viewModel.numberVB.VBNo : "";
                dateestimate = viewModel.numberVB != null ? viewModel.numberVB.DateEstimate.GetValueOrDefault() : DateTimeOffset.MinValue;
                datevb = viewModel.numberVB != null ? viewModel.numberVB.Date.GetValueOrDefault() : DateTimeOffset.MinValue;
                //dateestimate = (DateTimeOffset)viewModel.DateEstimateVB;
                requestvbname = "";
                unitid = viewModel.Unit.Id;
                unitcode = viewModel.Unit.Code;
                unitname = viewModel.Unit.Name;
                amount_vb = 0;
                currencycode = viewModel.Currency.Code;
                currencyrate = viewModel.Currency.Rate;
                currencydescription = viewModel.Currency.Description;
                currencysymbol = viewModel.Currency.Symbol;
                vbrealizecategory = "NONPO";
                usagevbrequest = "";
                divisionid = viewModel.Division.Id;
                divisioname = viewModel.Division.Name;                

            }
            else
            {
                unitload = res;
                amountunitloadnovb = val_result;
                vbno = viewModel.numberVB.VBNo;
                dateestimate = (DateTimeOffset)viewModel.numberVB.DateEstimate;
                requestvbname = viewModel.numberVB.CreatedBy;
                unitid = viewModel.numberVB.UnitId;
                unitcode = viewModel.numberVB.UnitCode;
                unitname = viewModel.numberVB.UnitName;
                datevb = (DateTimeOffset)viewModel.numberVB.Date;
                amount_vb = viewModel.numberVB.Amount;
                currencycode = viewModel.numberVB.CurrencyCode;
                currencyrate = viewModel.numberVB.CurrencyRate;
                currencydescription = viewModel.numberVB.CurrencyDescription;
                currencysymbol = viewModel.numberVB.CurrencySymbol;
                vbrealizecategory = viewModel.numberVB.VBRequestCategory;
                usagevbrequest = viewModel.numberVB.Usage;
                divisionid = viewModel.numberVB.UnitDivisionId;
                divisioname = viewModel.numberVB.UnitDivisionName;
            }

            var result = new RealizationVbModel()
            {
                RealizationVbDetail = listDetail,
                Active = viewModel.Active,
                Id = viewModel.Id,
                Date = (DateTimeOffset)viewModel.Date,
                UnitId = unitid,
                UnitCode = unitcode,
                UnitName = unitname,
                DivisionId = divisionid,
                DivisionName = divisioname,
                VBNo = vbno,
                VBNoRealize = viewModel.VBRealizationNo,
                DateEstimate = dateestimate,
                DateVB = datevb,
                CurrencyCode = currencycode,
                CurrencyRate = currencyrate,
                CurrencyDescription = currencydescription,
                CurrencySymbol = currencysymbol,
                UnitLoad = unitload,
                RequestVbName = requestvbname,
                UsageVBRequest = usagevbrequest,
                Amount = temp_total,
                Amount_VB = amount_vb,
                isVerified = false,
                isClosed = false,
                isNotVeridied = false,
                CreatedUtc = viewModel.CreatedUtc,
                CreatedBy = viewModel.CreatedBy,
                CreatedAgent = viewModel.CreatedAgent,
                LastModifiedAgent = viewModel.LastModifiedAgent,
                LastModifiedBy = viewModel.LastModifiedBy,
                LastModifiedUtc = DateTime.Now,
                VBRealizeCategory = vbrealizecategory,
                DifferenceReqReal = DifferenceReqReal,
                VatAmount = total_vat,
                StatusReqReal = StatusReqReal,
                //UnitLoad = res,
                AmountUnitLoadNoVB = amountunitloadnovb,
                TypeWithOrWithoutVB = viewModel.TypeVBNonPO,
                //VBNo = "",
                //DateEstimate = (DateTimeOffset)viewModel.DateEstimateVB,
                //RequestVbName = "",
                //UnitCode = viewModel.Unit.Code,
                //UnitName = viewModel.Unit.Name,
                //DateVB = (DateTimeOffset)viewModel.Date,
                //Amount_VB = 0,
                //CurrencyCode = viewModel.Currency.Code,
                //CurrencyRate = viewModel.Currency.Rate,
                //CurrencyDescription = viewModel.Currency.Description,
                //CurrencySymbol = viewModel.Currency.Symbol,
                //VBRealizeCategory = "NONPO",
                //UsageVBRequest = "",
                //DivisionId = viewModel.Division.Id,
                //DivisionName = viewModel.Division.Name,

            };

            return result;
        }
    }
}

using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Com.Moonlay.Models;
using System.Threading.Tasks;
using System.Linq;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using System.Net.Http;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using System.IO;
using System.Data;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.PaymentDispositionNote
{
    public class PaymentDispositionNoteService : IPaymentDispositionNoteService
    {
        private const string UserAgent = "finance-service";
        protected DbSet<PaymentDispositionNoteModel> DbSet;
        public IIdentityService IdentityService;
        private readonly IAutoDailyBankTransactionService _autoDailyBankTransactionService;
        private readonly IAutoJournalService _autoJournalService;
        public readonly IServiceProvider ServiceProvider;
        public FinanceDbContext DbContext;

        public PaymentDispositionNoteService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            DbContext = dbContext;
            ServiceProvider = serviceProvider;
            DbSet = dbContext.Set<PaymentDispositionNoteModel>();
            IdentityService = serviceProvider.GetService<IIdentityService>();
            _autoDailyBankTransactionService = serviceProvider.GetService<IAutoDailyBankTransactionService>();
            _autoJournalService = serviceProvider.GetService<IAutoJournalService>();
        }

        public void CreateModel(PaymentDispositionNoteModel model)
        {
            EntityExtension.FlagForCreate(model, IdentityService.Username, UserAgent);
            foreach (var item in model.Items)
            {
                PurchasingDispositionExpeditionModel expedition = DbContext.PurchasingDispositionExpeditions.FirstOrDefault(ex => ex.Id.Equals(item.PurchasingDispositionExpeditionId));
                EntityExtension.FlagForCreate(item, IdentityService.Username, UserAgent);
                var paidFlag = true;
                var query = DbContext.PaymentDispositionNoteItems.Where(x => x.DispositionNo == item.DispositionNo).ToList();

                if (query.Count == 0)
                {
                    if (item.SupplierPayment != item.PayToSupplier)
                    {
                        paidFlag = false;
                    }
                }
                else
                {
                    if ((query.Sum(x => x.SupplierPayment) + item.SupplierPayment) != item.PayToSupplier)
                    {
                        paidFlag = false;
                    }
                }

                var pdeDisposition = DbContext.PurchasingDispositionExpeditions.Include(entity => entity.Items).LastOrDefault(entity => entity.DispositionNo == item.DispositionNo);

                if (pdeDisposition != null && string.IsNullOrWhiteSpace(pdeDisposition.BankExpenditureNoteNo))
                {
                    pdeDisposition.IsPaid = paidFlag;
                    pdeDisposition.BankExpenditureNoteNo = model.PaymentDispositionNo;
                    pdeDisposition.BankExpenditureNoteDate = model.PaymentDate;
                    pdeDisposition.AmountPaid = item.AmountPaid;
                    pdeDisposition.SupplierPayment = item.SupplierPayment;
                }
                else
                {
                    pdeDisposition.IsPaid = true;
                    
                    PurchasingDispositionExpeditionModel pde = new PurchasingDispositionExpeditionModel
                    {
                        Active = pdeDisposition.Active,
                        AmountPaid = item.AmountPaid,
                        CategoryId = pdeDisposition.CategoryId,
                        CurrencyCode = pdeDisposition.CurrencyCode,
                        CurrencyId = pdeDisposition.CurrencyId,
                        CreatedAgent = pdeDisposition.CreatedAgent,
                        BankExpenditureNoteDate = model.PaymentDate,
                        BankExpenditureNoteNo = model.PaymentDispositionNo,
                        BankExpenditureNotePPHDate = pdeDisposition.BankExpenditureNotePPHDate,
                        BankExpenditureNotePPHNo = pdeDisposition.BankExpenditureNotePPHNo,
                        CashierDivisionBy = pdeDisposition.CashierDivisionBy,
                        CashierDivisionDate = pdeDisposition.CashierDivisionDate,
                        CategoryCode = pdeDisposition.CategoryCode,
                        CategoryName = pdeDisposition.CategoryName,
                        CreatedBy = pdeDisposition.CreatedBy,
                        CreatedUtc = model.CreatedUtc,
                        DispositionDate = pdeDisposition.DispositionDate,
                        DispositionId = pdeDisposition.DispositionId,
                        DispositionNo = pdeDisposition.DispositionNo,
                        DivisionId = pdeDisposition.DivisionId,
                        DivisionCode = pdeDisposition.DivisionCode,
                        DivisionName = pdeDisposition.DivisionName,
                        DPP = pdeDisposition.DPP,
                        IncomeTaxId = pdeDisposition.IncomeTaxId,
                        IncomeTaxName = pdeDisposition.IncomeTaxName,
                        IncomeTaxRate = pdeDisposition.IncomeTaxRate,
                        IncomeTaxValue = pdeDisposition.IncomeTaxValue,
                        IsDeleted = pdeDisposition.IsDeleted,
                        IsPaid = paidFlag,
                        IsPaidPPH = pdeDisposition.IsPaidPPH,
                        NotVerifiedReason = pdeDisposition.NotVerifiedReason,
                        PaymentDueDate = pdeDisposition.PaymentDueDate,
                        PaymentMethod = pdeDisposition.PaymentMethod,
                        PayToSupplier = pdeDisposition.PayToSupplier,
                        Position = pdeDisposition.Position,
                        ProformaNo = pdeDisposition.ProformaNo,
                        SendToCashierDivisionBy = pdeDisposition.SendToCashierDivisionBy,
                        SendToCashierDivisionDate = pdeDisposition.SendToCashierDivisionDate,
                        SendToPurchasingDivisionBy = pdeDisposition.SendToPurchasingDivisionBy,
                        SendToPurchasingDivisionDate = pdeDisposition.SendToPurchasingDivisionDate,
                        SupplierCode = pdeDisposition.SupplierCode,
                        SupplierId = pdeDisposition.SupplierId,
                        SupplierName = pdeDisposition.SupplierName,
                        SupplierPayment = item.SupplierPayment,
                        TotalPaid = pdeDisposition.TotalPaid,
                        UseIncomeTax = pdeDisposition.UseIncomeTax,
                        UseVat = pdeDisposition.UseVat,
                        VatValue = pdeDisposition.VatValue,
                        VerificationDivisionBy = pdeDisposition.VerificationDivisionBy,
                        VerificationDivisionDate = pdeDisposition.VerificationDivisionDate,
                        VerifyDate = pdeDisposition.VerifyDate,
                    };

                    List<PurchasingDispositionExpeditionItemModel> pdeItems = new List<PurchasingDispositionExpeditionItemModel>();

                    foreach (var element in pdeDisposition.Items)
                    {
                        PurchasingDispositionExpeditionItemModel expeditionItem = new PurchasingDispositionExpeditionItemModel
                        {
                            Active = element.Active,
                            CreatedAgent = element.CreatedAgent,
                            CreatedBy = element.CreatedBy,
                            CreatedUtc = element.CreatedUtc,
                            DeletedAgent = element.DeletedAgent,
                            DeletedBy = element.DeletedBy,
                            DeletedUtc = element.DeletedUtc,
                            EPOId = element.EPOId,
                            EPONo = element.EPONo,
                            IsDeleted = element.IsDeleted,
                            LastModifiedAgent = element.LastModifiedAgent,
                            LastModifiedBy = element.LastModifiedBy,
                            LastModifiedUtc = element.LastModifiedUtc,
                            Price = element.Price,
                            ProductCode = element.ProductCode,
                            ProductId = element.ProductId,
                            ProductName = element.ProductName,
                            PurchasingDispositionExpedition = pde,
                            PurchasingDispositionDetailId = element.PurchasingDispositionDetailId,
                            Quantity = element.Quantity,
                            UnitCode = element.UnitCode,
                            UnitId = element.UnitId,
                            UnitName = element.UnitName,
                            UomId = element.UomId,
                            UomUnit = element.UomUnit
                        };

                        pdeItems.Add(expeditionItem);
                    }

                    pde.Items = pdeItems;

                    EntityExtension.FlagForCreate(pde, IdentityService.Username, UserAgent);

                    DbContext.PurchasingDispositionExpeditions.Add(pde);
                }
                //expedition.IsPaid = paidFlag;
                //expedition.BankExpenditureNoteNo = model.PaymentDispositionNo;
                //expedition.BankExpenditureNoteDate = model.PaymentDate;
                foreach (var detail in item.Details)
                {
                    EntityExtension.FlagForCreate(detail, IdentityService.Username, UserAgent);
                }
            }
            DbSet.Add(model);
        }

        public async Task<int> CreateAsync(PaymentDispositionNoteModel model)
        {
            var timeOffset = new TimeSpan(IdentityService.TimezoneOffset, 0, 0);
            model.PaymentDispositionNo = await GetDocumentNo("K", model.BankCode, IdentityService.Username, model.PaymentDate.ToOffset(timeOffset).Date);

            if (model.CurrencyCode != "IDR")
            {
                if (model.BankCurrencyCode != "IDR")
                {
                    var BICurrency = await GetBICurrency(model.BankCurrencyCode, model.PaymentDate);
                    model.CurrencyRate = BICurrency.Rate.GetValueOrDefault();
                }
            }

            CreateModel(model);
            //await _autoDailyBankTransactionService.AutoCreateFromPaymentDisposition(model);
            return await DbContext.SaveChangesAsync();
        }

        //private async Task<string> GetDocumentNo(string type, string bankCode, string username)
        //{
        //    var jsonSerializerSettings = new JsonSerializerSettings
        //    {
        //        MissingMemberHandling = MissingMemberHandling.Ignore
        //    };

        //    var http = ServiceProvider.GetService<IHttpClientService>();
        //    var uri = APIEndpoint.Purchasing + $"bank-expenditure-notes/bank-document-no?type={type}&bankCode={bankCode}&username={username}";
        //    var response = await http.GetAsync(uri);

        //    var result = new BaseResponse<string>();

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        result = JsonConvert.DeserializeObject<BaseResponse<string>>(responseContent, jsonSerializerSettings);
        //    }
        //    return result.data;
        //}
        private async Task<string> GetDocumentNo(string type, string bankCode, string username,DateTime date)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var http = ServiceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.Purchasing + $"bank-expenditure-notes/bank-document-no-date?type={type}&bankCode={bankCode}&username={username}&date={date}";
            var response = await http.GetAsync(uri);

            var result = new BaseResponse<string>();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<BaseResponse<string>>(responseContent, jsonSerializerSettings);
            }
            return result.data;
        }
        //private async Task<GarmentCurrency> GetGarmentCurrency(string codeCurrency)
        //{
        //    var date = DateTimeOffset.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
        //    var queryString = $"code={codeCurrency}&stringDate={date}";

        //    var http = ServiceProvider.GetService<IHttpClientService>();
        //    var response = await http.GetAsync(APIEndpoint.Core + $"master/garment-currencies/single-by-code-date?{queryString}");

        //    var responseString = await response.Content.ReadAsStringAsync();
        //    var jsonSerializationSetting = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore };

        //    var result = JsonConvert.DeserializeObject<APIDefaultResponse<GarmentCurrency>>(responseString, jsonSerializationSetting);

        //    return result.data;
        //}

        private async Task<GarmentCurrency> GetBICurrency(string codeCurrency, DateTimeOffset date)
        {
            string stringDate = date.ToString("yyyy/MM/dd HH:mm:ss");
            string queryString = $"code={codeCurrency}&stringDate={stringDate}";

            var http = ServiceProvider.GetService<IHttpClientService>();
            var response = await http.GetAsync(APIEndpoint.Core + $"master/bi-currencies/single-by-code-date?{queryString}");

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonSerializationSetting = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore };

            var result = JsonConvert.DeserializeObject<APIDefaultResponse<GarmentCurrency>>(responseString, jsonSerializationSetting);

            return result.data;
        }

        //async Task<string> GenerateNo(PaymentDispositionNoteModel model, int clientTimeZoneOffset)
        //{
        //    DateTimeOffset Now = model.PaymentDate;
        //    string Year = Now.ToOffset(new TimeSpan(clientTimeZoneOffset, 0, 0)).ToString("yy");
        //    string Month = Now.ToOffset(new TimeSpan(clientTimeZoneOffset, 0, 0)).ToString("MM");
        //    string Day = Now.ToOffset(new TimeSpan(clientTimeZoneOffset, 0, 0)).ToString("dd");
        //    //PD + 2 digit tahun + 2 digit bulan + 2 digit tgl + 3 digit urut
        //    string no = $"PD-{Year}-{Month}-{Day}-";
        //    int Padding = 3;

        //    var lastNo = await this.DbSet.Where(w => w.PaymentDispositionNo.StartsWith(no) && !w.IsDeleted).OrderByDescending(o => o.PaymentDispositionNo).FirstOrDefaultAsync();
        //    no = $"{no}";

        //    if (lastNo == null)
        //    {
        //        return no + "1".PadLeft(Padding, '0');
        //    }
        //    else
        //    {
        //        int lastNoNumber = int.Parse(lastNo.PaymentDispositionNo.Replace(no, "")) + 1;
        //        return no + lastNoNumber.ToString().PadLeft(Padding, '0');
        //    }
        //}

        public async Task DeleteModel(int id)
        {
            PaymentDispositionNoteModel model = await ReadByIdAsync(id);
            foreach (var item in model.Items)
            {
                PurchasingDispositionExpeditionModel expedition = DbContext.PurchasingDispositionExpeditions.FirstOrDefault(ex => ex.Id.Equals(item.PurchasingDispositionExpeditionId));
                EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
                expedition.IsPaid = false;
                expedition.BankExpenditureNoteNo = null;
                expedition.BankExpenditureNoteDate = DateTimeOffset.MinValue;
                foreach (var detail in item.Details)
                {
                    EntityExtension.FlagForDelete(detail, IdentityService.Username, UserAgent, true);
                }
            }
            EntityExtension.FlagForDelete(model, IdentityService.Username, UserAgent, true);
            DbSet.Update(model);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var existingModel = DbSet
                        .Include(d => d.Items)
                        .ThenInclude(d => d.Details)
                        .Single(dispo => dispo.Id == id && !dispo.IsDeleted);

            //await _autoDailyBankTransactionService.AutoCreateFromPaymentDisposition(existingModel);
            await DeleteModel(id);
            return await DbContext.SaveChangesAsync();
        }

        public Task<PaymentDispositionNoteModel> ReadByIdAsync(int id)
        {
            return DbSet.Include(m => m.Items).ThenInclude(m => m.Details).FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public async Task<int> UpdateAsync(int id, PaymentDispositionNoteModel model)
        {
            var existingModel = DbSet
                        .Include(d => d.Items)
                        .ThenInclude(d => d.Details)
                        .Single(dispo => dispo.Id == id && !dispo.IsDeleted);
            UpdateModel(id, model);
            //await _autoDailyBankTransactionService.AutoRevertFromPaymentDisposition(existingModel);
            //await _autoDailyBankTransactionService.AutoCreateFromPaymentDisposition(model);
            return await DbContext.SaveChangesAsync();
        }

        public void UpdateModel(int id, PaymentDispositionNoteModel model)
        {
            PaymentDispositionNoteModel exist = DbSet
                        .Include(d => d.Items)
                        .ThenInclude(d => d.Details)
                        .Single(dispo => dispo.Id == id && !dispo.IsDeleted);


            exist.BGCheckNumber = model.BGCheckNumber;
            exist.PaymentDate = model.PaymentDate;
            exist.Amount = model.Amount;

            foreach (var item in exist.Items)
            {
                PaymentDispositionNoteItemModel itemModel = model.Items.FirstOrDefault(prop => prop.Id.Equals(item.Id));

                if (itemModel == null)
                {
                    PurchasingDispositionExpeditionModel expedition = DbContext.PurchasingDispositionExpeditions.FirstOrDefault(ex => ex.Id.Equals(item.PurchasingDispositionExpeditionId));
                    expedition.IsPaid = false;
                    expedition.BankExpenditureNoteNo = null;
                    expedition.BankExpenditureNoteDate = DateTimeOffset.MinValue;
                    expedition.AmountPaid = 0;
                    expedition.SupplierPayment = 0;

                    EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);

                    foreach (var detail in item.Details)
                    {
                        EntityExtension.FlagForDelete(detail, IdentityService.Username, UserAgent, true);
                        //DbContext.PaymentDispositionNoteDetails.Update(detail);
                    }

                    //DbContext.PaymentDispositionNoteItems.Update(item);
                }
                else
                {
                    PurchasingDispositionExpeditionModel expedition = DbContext.PurchasingDispositionExpeditions.FirstOrDefault(ex => ex.Id.Equals(item.PurchasingDispositionExpeditionId));

                    var paidFlag = true;
                    var query = DbContext.PaymentDispositionNoteItems.Where(x => x.PaymentDispositionNoteId == item.PaymentDispositionNoteId).ToList();

                    if (query.Count == 0)
                    {
                        if (item.SupplierPayment != item.PayToSupplier)
                        {
                            paidFlag = false;
                        }
                    }
                    else
                    {
                        if ((query.Sum(x => x.SupplierPayment) + item.SupplierPayment) != item.PayToSupplier)
                        {
                            paidFlag = false;
                        }
                    }

                    item.SupplierPayment = itemModel.SupplierPayment;

                    expedition.IsPaid = paidFlag;
                    expedition.BankExpenditureNoteNo = model.PaymentDispositionNo;
                    expedition.BankExpenditureNoteDate = model.PaymentDate;
                    expedition.AmountPaid = item.AmountPaid;
                    expedition.SupplierPayment = item.SupplierPayment;

                    EntityExtension.FlagForUpdate(item, IdentityService.Username, UserAgent);

                    foreach (var detail in DbContext.PaymentDispositionNoteDetails.AsNoTracking().Where(p => p.PaymentDispositionNoteItemId == item.Id))
                    {
                        EntityExtension.FlagForUpdate(detail, IdentityService.Username, UserAgent);

                    }
                }
            }

            EntityExtension.FlagForUpdate(exist, IdentityService.Username, UserAgent);
            //DbSet.Update(exist);
        }

        public ReadResponse<PaymentDispositionNoteModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<PaymentDispositionNoteModel> Query = this.DbSet.Include(m => m.Items).ThenInclude(m => m.Details);
            List<string> searchAttributes = new List<string>()
            {
                "PaymentDispositionNo", "Items.DispositionNo",  "SupplierName", "BankCurrencyCode","BankName"
            };

            Query = QueryHelper<PaymentDispositionNoteModel>.Search(Query, searchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<PaymentDispositionNoteModel>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<PaymentDispositionNoteModel>.Order(Query, OrderDictionary);

            Pageable<PaymentDispositionNoteModel> pageable = new Pageable<PaymentDispositionNoteModel>(Query, page - 1, size);
            List<PaymentDispositionNoteModel> Data = pageable.Data.ToList();
            int TotalData = pageable.TotalCount;

            return new ReadResponse<PaymentDispositionNoteModel>(Data, TotalData, OrderDictionary, new List<string>());
        }

        public ReadResponse<PaymentDispositionNoteItemModel> ReadDetailsByEPOId(string epoId)
        {
            List<PaymentDispositionNoteItemModel> paymentDispositionNoteDetails = DbContext.PaymentDispositionNoteItems.Where(a => a.Details.Any(b => b.EPOId == epoId)).Distinct().ToList();

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>("{ }");

            int TotalData = paymentDispositionNoteDetails.Count;

            return new ReadResponse<PaymentDispositionNoteItemModel>(paymentDispositionNoteDetails, TotalData, OrderDictionary, new List<string>());
        }

        private async Task SetTrueDisposition(string dispositionNo)
        {
            var http = ServiceProvider.GetService<IHttpClientService>();
            await http.PutAsync(APIEndpoint.Purchasing + $"purchasing-dispositions/update/is-paid-true/{dispositionNo}", new StringContent("{}", Encoding.UTF8, General.JsonMediaType) );
        }

        public async Task<int> Post(PaymentDispositionNotePostDto form)
        {
            List<int> listIds = form.ListIds.Select(x => x.Id).ToList();

            foreach (int id in listIds)
            {
                var model = await ReadByIdAsync(id);

                if (model != null)
                {
                    model.SetIsPosted(IdentityService.Username, UserAgent);

                    foreach (var item in model.Items)
                    {
                        await SetTrueDisposition(item.DispositionNo);
                    }

                    await _autoDailyBankTransactionService.AutoCreateFromPaymentDisposition(model);
                    await _autoJournalService.AutoJournalFromDisposition(model, IdentityService.Username, UserAgent);
                }
            }

            var result = await DbContext.SaveChangesAsync();

            return result;
        }

        public List<ReportDto> GetReport(int bankExpenditureId, int dispositionId, int supplierId, int divisionId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var expenditureQuery = DbContext.PaymentDispositionNotes.AsQueryable();
            var expenditureItemQuery = DbContext.PaymentDispositionNoteItems.AsQueryable();
            var query = from expenditure in expenditureQuery
                        join expenditureItem in expenditureItemQuery on expenditure.Id equals expenditureItem.PaymentDispositionNoteId into items
                        from item in items.DefaultIfEmpty()
                        where expenditure.IsPosted
                        select new
                        {
                            expenditure.Id,
                            expenditure.PaymentDispositionNo,
                            expenditure.PaymentDate,
                            item.DispositionId,
                            item.DispositionNo,
                            item.DispositionDate,
                            item.PaymentDueDate,
                            expenditure.BankId,
                            expenditure.BankName,
                            expenditure.BankCurrencyCode,
                            expenditure.CurrencyId,
                            expenditure.CurrencyCode,
                            expenditure.CurrencyRate,
                            expenditure.SupplierId,
                            expenditure.SupplierName,
                            expenditure.SupplierImport,
                            item.ProformaNo,
                            item.CategoryId,
                            item.CategoryName,
                            item.DivisionId,
                            item.DivisionName,
                            item.VatValue,
                            item.DPP,
                            expenditure.TransactionType,
                            expenditure.BankAccountNumber,
                            item.IncomeTaxValue,
                            item.PayToSupplier,
                            expenditure.Amount,
                            item.AmountPaid,
                            item.SupplierPayment
                        };

            query = query.Where(entity => entity.PaymentDate >= startDate && entity.PaymentDate <= endDate);
            if (bankExpenditureId > 0)
                query = query.Where(entity => entity.Id == bankExpenditureId);

            if (dispositionId > 0)
                query = query.Where(entity => entity.DispositionId == dispositionId);

            if (supplierId > 0)
                query = query.Where(entity => entity.SupplierId == supplierId);

            if (divisionId > 0)
                query = query.Where(entity => entity.DivisionId == divisionId);

            var result = query.OrderBy(entity => entity.PaymentDate).ToList();

            return result.Select(element => new ReportDto(element.Id, element.PaymentDispositionNo, element.PaymentDate, element.DispositionId, element.DispositionNo, element.DispositionDate, element.PaymentDueDate, element.BankId, element.BankName, element.CurrencyId, element.CurrencyCode, element.SupplierId, element.SupplierName, element.SupplierImport, element.ProformaNo, element.CategoryId, element.CategoryName, element.DivisionId, element.DivisionName, element.VatValue, element.PayToSupplier, element.TransactionType, element.BankAccountNumber, element.CurrencyRate, element.BankCurrencyCode, element.AmountPaid, element.SupplierPayment, element.DPP, element.VatValue, element.IncomeTaxValue)).ToList();
        }

        public MemoryStream GetXls(List<ReportDto> data)
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Bukti Pembayaran Disposisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Bayar Disposisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Disposisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Disposisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Jatuh Tempo", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Bank Bayar", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mata Uang", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Supplier", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Proforma Invoice", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kategori", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Divisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "PPN", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Total Disposisi", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Jumlah dibayar ke Supplier", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Sisa yang Belum Dibayar", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Jenis Transaksi", DataType = typeof(string) });

            if (data.Count() == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, "");
            }
            else
            {
                foreach (var item in data)
                {
                    dt.Rows.Add(
                        item.ExpenditureNo,
                        item.ExpenditureDate.AddHours(IdentityService.TimezoneOffset).ToString("dd/MM/yyyy"),
                        item.DispositionNo,
                        item.DispositionDate.AddHours(IdentityService.TimezoneOffset).ToString("dd/MM/yyyy"),
                        item.DispositionDueDate.AddHours(IdentityService.TimezoneOffset).ToString("dd/MM/yyyy"),
                        item.BankName,
                        item.CurrencyCode,
                        item.SupplierName,
                        item.ProformaNo,
                        item.CategoryName,
                        item.DivisionName,
                        item.VATAmount,
                        item.PaidAmount,
                        item.DispositionNominal,
                        item.DifferenceAmount,
                        item.TransactionType
                        );
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Laporan Pembayaran Disposisi") }, true);
        }

        public ReadResponse<PurchasingDispositionExpeditionModel> GetAllByPosition(int page, int size, string order, List<string> select, string keyword, string filter)
        {

            IQueryable<PurchasingDispositionExpeditionModel> Query = DbContext.PurchasingDispositionExpeditions.Include(m => m.Items);
            List<string> searchAttributes = new List<string>()
            {
                "BankExpenditureNoteNo","DispositionId", "DispositionNo",  "SupplierName", "CurrencyCode"
            };

            Query = QueryHelper<PurchasingDispositionExpeditionModel>.Search(Query, searchAttributes, keyword);

            if (filter.Contains("verificationFilter"))
            {
                filter = "{}";
                List<ExpeditionPosition> positions = new List<ExpeditionPosition> { ExpeditionPosition.SEND_TO_PURCHASING_DIVISION, ExpeditionPosition.SEND_TO_ACCOUNTING_DIVISION, ExpeditionPosition.SEND_TO_CASHIER_DIVISION };
                Query = Query.Where(p => positions.Contains(p.Position));
            }

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<PurchasingDispositionExpeditionModel>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<PurchasingDispositionExpeditionModel>.Order(Query, OrderDictionary);

            Pageable<PurchasingDispositionExpeditionModel> pageable = new Pageable<PurchasingDispositionExpeditionModel>(Query, page - 1, size);
            List<PurchasingDispositionExpeditionModel> Data = pageable.Data.ToList();
            int TotalData = pageable.TotalCount;

            return new ReadResponse<PurchasingDispositionExpeditionModel>(Data, TotalData, OrderDictionary, new List<string>());
        }

        public ResponseAmountPaidandIsPosted GetAmountPaidAndIsPosted(string DispositionNo)
        {
            var paymentDispositionNote = DbContext.PaymentDispositionNoteItems.Where(x => x.DispositionNo == DispositionNo).ToList();
            double AmountPaid = 0;
            bool IsPosted = true;

            if (paymentDispositionNote.Count > 0)
            {
                AmountPaid = paymentDispositionNote.Sum(x => x.SupplierPayment);
                IsPosted = DbSet.Where(p => p.Id == (paymentDispositionNote.LastOrDefault().PaymentDispositionNoteId)).LastOrDefault().IsPosted;
            }

            ResponseAmountPaidandIsPosted response = new ResponseAmountPaidandIsPosted();
            response.AmountPaid = AmountPaid;
            response.IsPosted = IsPosted;

            return response;
        }

        public MemoryStream GeneratePdfTemplate(PaymentDispositionNoteViewModel viewModel, int clientTimeZoneOffset)
        {
            const int MARGIN = 15;

            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 18);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

            Document document = new Document(PageSize.A4, MARGIN, MARGIN, MARGIN, MARGIN);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            #region Header

            PdfPTable headerTable = new PdfPTable(2);
            headerTable.SetWidths(new float[] { 10f, 10f });
            headerTable.WidthPercentage = 100;
            PdfPTable headerTable1 = new PdfPTable(1);
            PdfPTable headerTable2 = new PdfPTable(2);
            headerTable2.SetWidths(new float[] { 15f, 40f });
            headerTable2.WidthPercentage = 100;

            PdfPCell cellHeader1 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeader2 = new PdfPCell() { Border = Rectangle.NO_BORDER };
            PdfPCell cellHeaderBody = new PdfPCell() { Border = Rectangle.NO_BORDER };

            PdfPCell cellHeaderCS2 = new PdfPCell() { Border = Rectangle.NO_BORDER, Colspan = 2 };


            cellHeaderCS2.Phrase = new Phrase("BUKTI PENGELUARAN BANK - DISPOSISI", bold_font);
            cellHeaderCS2.HorizontalAlignment = Element.ALIGN_CENTER;
            headerTable.AddCell(cellHeaderCS2);

            cellHeaderCS2.Phrase = new Phrase("", bold_font);
            cellHeaderCS2.HorizontalAlignment = Element.ALIGN_CENTER;
            headerTable.AddCell(cellHeaderCS2);

            cellHeaderBody.Phrase = new Phrase("PT. DANLIRIS", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Kel. Banaran, Kec. Grogol", normal_font);
            headerTable1.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase("Sukoharjo - 57100", normal_font);
            headerTable1.AddCell(cellHeaderBody);

            cellHeader1.AddElement(headerTable1);
            headerTable.AddCell(cellHeader1);

            cellHeaderCS2.Phrase = new Phrase("", bold_font);
            headerTable2.AddCell(cellHeaderCS2);

            cellHeaderBody.Phrase = new Phrase("Tanggal", normal_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(": " + viewModel.PaymentDate.AddHours(clientTimeZoneOffset).ToString("dd MMMM yyyy", new CultureInfo("id-ID")), normal_font);
            headerTable2.AddCell(cellHeaderBody);

            cellHeaderBody.Phrase = new Phrase("NO", normal_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(": " + viewModel.PaymentDispositionNo, normal_font);
            headerTable2.AddCell(cellHeaderBody);

            //List<string> supplier = model.Details.Select(m => m.SupplierName).Distinct().ToList();
            cellHeaderBody.Phrase = new Phrase("Dibayarkan ke", normal_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(": " + viewModel.Supplier.Name, normal_font);
            headerTable2.AddCell(cellHeaderBody);


            cellHeaderBody.Phrase = new Phrase("Bank", normal_font);
            headerTable2.AddCell(cellHeaderBody);
            cellHeaderBody.Phrase = new Phrase(": " + viewModel.AccountBank.BankName + " - A/C : " + viewModel.AccountBank.AccountNumber, normal_font);
            headerTable2.AddCell(cellHeaderBody);

            cellHeader2.AddElement(headerTable2);
            headerTable.AddCell(cellHeader2);

            cellHeaderCS2.Phrase = new Phrase("", normal_font);
            headerTable.AddCell(cellHeaderCS2);

            document.Add(headerTable);

            #endregion Header

            var purchasingDispositionId = viewModel.Items.Select(detail => detail.purchasingDispositionExpeditionId).ToList();
            var purchasingDispositions = DbContext.PurchasingDispositionExpeditions.Where(x => purchasingDispositionId.Contains(x.Id)).ToList();
            var currency = GetBICurrency(viewModel.CurrencyCode, viewModel.PaymentDate).Result;

            if (currency == null)
            {
                currency = new GarmentCurrency() { Rate = viewModel.CurrencyRate };
            }

            Dictionary<string, double> units = new Dictionary<string, double>();
            Dictionary<string, double> percentageUnits = new Dictionary<string, double>();


            int index = 1;
            double total = 0;
            double totalPay = 0;
            bool sameCurrency = true;

            if (viewModel.AccountBank.Currency.Code != "IDR" || viewModel.CurrencyCode == "IDR")
            {
                #region BodyNonIDR

                PdfPTable bodyNonIDRTable = new PdfPTable(6);
                PdfPCell bodyNonIDRCell = new PdfPCell();

                float[] widthsBodyNonIDR = new float[] { 5f, 10f, 10f, 10f, 7f, 15f };
                bodyNonIDRTable.SetWidths(widthsBodyNonIDR);
                bodyNonIDRTable.WidthPercentage = 100;

                bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_CENTER;
                bodyNonIDRCell.Phrase = new Phrase("No.", bold_font);
                bodyNonIDRTable.AddCell(bodyNonIDRCell);

                bodyNonIDRCell.Phrase = new Phrase("No. Disposisi", bold_font);
                bodyNonIDRTable.AddCell(bodyNonIDRCell);

                bodyNonIDRCell.Phrase = new Phrase("Kategori Barang", bold_font);
                bodyNonIDRTable.AddCell(bodyNonIDRCell);

                bodyNonIDRCell.Phrase = new Phrase("Divisi", bold_font);
                bodyNonIDRTable.AddCell(bodyNonIDRCell);

                bodyNonIDRCell.Phrase = new Phrase("Mata Uang", bold_font);
                bodyNonIDRTable.AddCell(bodyNonIDRCell);

                bodyNonIDRCell.Phrase = new Phrase("Jumlah", bold_font);
                bodyNonIDRTable.AddCell(bodyNonIDRCell);

                foreach (PaymentDispositionNoteItemViewModel item in viewModel.Items)
                {
                    var purchasingDisposition = purchasingDispositions.FirstOrDefault(element => element.Id == item.purchasingDispositionExpeditionId);

                    if (purchasingDisposition == null)
                        purchasingDisposition = new PurchasingDispositionExpeditionModel();

                    double remaining = item.SupplierPayment;
                    double previousPayment = item.AmountPaid;

                    var unitSummaries = item.Details.GroupBy(g => g.unit.code).Select(s => new
                    {
                        UnitCode = s.Key
                    });

                    //var details = item.Details
                    //    .GroupBy(m => new { m.unit.code, m.unit.name })
                    //    .Select(s => new
                    //    {
                    //        s.First().unit.code,
                    //        s.First().unit.name,
                    //        Total = s.Sum(d => d.price)
                    //    });

                    if (unitSummaries.Count() > 1)
                    {
                        foreach (var detail in item.Details)
                        {
                            var vatAmount = purchasingDisposition.UseVat ? detail.price * 0.1 : 0;
                            var incomeTaxAmount = purchasingDisposition.UseIncomeTax ? detail.price * purchasingDisposition.IncomeTaxRate / 100 : 0;
                            var dpp = detail.price + vatAmount - incomeTaxAmount;

                            if (remaining <= 0)
                            {
                                continue;
                            }

                            if (previousPayment > 0)
                            {
                                if (previousPayment >= dpp)
                                {
                                    previousPayment -= dpp;

                                    continue;
                                }
                                else
                                {
                                    dpp -= previousPayment;

                                    previousPayment -= dpp;
                                }
                            }

                            bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            bodyNonIDRCell.VerticalAlignment = Element.ALIGN_TOP;
                            bodyNonIDRCell.Phrase = new Phrase((index++).ToString(), normal_font);
                            bodyNonIDRTable.AddCell(bodyNonIDRCell);

                            bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            bodyNonIDRCell.Phrase = new Phrase(item.dispositionNo, normal_font);
                            bodyNonIDRTable.AddCell(bodyNonIDRCell);

                            bodyNonIDRCell.Phrase = new Phrase(item.category.name, normal_font);
                            bodyNonIDRTable.AddCell(bodyNonIDRCell);

                            bodyNonIDRCell.Phrase = new Phrase(item.division.name, normal_font);
                            bodyNonIDRTable.AddCell(bodyNonIDRCell);

                            bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            bodyNonIDRCell.Phrase = new Phrase(viewModel.AccountBank.Currency.Code, normal_font);
                            bodyNonIDRTable.AddCell(bodyNonIDRCell);

                            if (remaining >= dpp)
                            {
                                bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                bodyNonIDRCell.Phrase = new Phrase(string.Format("{0:n4}", dpp), normal_font);
                                bodyNonIDRTable.AddCell(bodyNonIDRCell);

                                if (units.ContainsKey(detail.unit.code))
                                {
                                    units[detail.unit.code] += dpp;
                                }
                                else
                                {
                                    units.Add(detail.unit.code, dpp);
                                }

                                totalPay += dpp;
                                total += dpp;
                                remaining -= dpp;
                            }
                            else
                            {
                                bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                bodyNonIDRCell.Phrase = new Phrase(string.Format("{0:n4}", remaining), normal_font);
                                bodyNonIDRTable.AddCell(bodyNonIDRCell);

                                if (units.ContainsKey(detail.unit.code))
                                {
                                    units[detail.unit.code] += remaining;
                                }
                                else
                                {
                                    units.Add(detail.unit.code, remaining);
                                }

                                totalPay += remaining;
                                total += remaining;
                                remaining -= remaining;
                            }
                        }
                    }
                    else
                    {
                        foreach (var detail in unitSummaries)
                        {
                            bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            bodyNonIDRCell.VerticalAlignment = Element.ALIGN_TOP;
                            bodyNonIDRCell.Phrase = new Phrase((index++).ToString(), normal_font);
                            bodyNonIDRTable.AddCell(bodyNonIDRCell);

                            bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            bodyNonIDRCell.Phrase = new Phrase(item.dispositionNo, normal_font);
                            bodyNonIDRTable.AddCell(bodyNonIDRCell);

                            bodyNonIDRCell.Phrase = new Phrase(item.category.name, normal_font);
                            bodyNonIDRTable.AddCell(bodyNonIDRCell);

                            bodyNonIDRCell.Phrase = new Phrase(item.division.name, normal_font);
                            bodyNonIDRTable.AddCell(bodyNonIDRCell);

                            bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            bodyNonIDRCell.Phrase = new Phrase(viewModel.AccountBank.Currency.Code, normal_font);
                            bodyNonIDRTable.AddCell(bodyNonIDRCell);

                            bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            bodyNonIDRCell.Phrase = new Phrase(string.Format("{0:n4}", remaining), normal_font);
                            bodyNonIDRTable.AddCell(bodyNonIDRCell);

                            if (units.ContainsKey(detail.UnitCode))
                            {
                                units[detail.UnitCode] += remaining;
                            }
                            else
                            {
                                units.Add(detail.UnitCode, remaining);
                            }

                            totalPay += remaining;
                            total += remaining;
                        }
                    }
                }

                foreach (var un in units)
                {
                    percentageUnits[un.Key] = un.Value * 100 / totalPay;
                }

                bodyNonIDRCell.Colspan = 3;
                bodyNonIDRCell.Border = Rectangle.NO_BORDER;
                bodyNonIDRCell.Phrase = new Phrase("", normal_font);
                bodyNonIDRTable.AddCell(bodyNonIDRCell);

                bodyNonIDRCell.Colspan = 1;
                bodyNonIDRCell.Border = Rectangle.BOX;
                bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_LEFT;
                bodyNonIDRCell.Phrase = new Phrase("Total", bold_font);
                bodyNonIDRTable.AddCell(bodyNonIDRCell);

                bodyNonIDRCell.Colspan = 1;
                bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_CENTER;
                bodyNonIDRCell.Phrase = new Phrase(viewModel.AccountBank.Currency.Code, bold_font);
                bodyNonIDRTable.AddCell(bodyNonIDRCell);

                bodyNonIDRCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                bodyNonIDRCell.Phrase = new Phrase(string.Format("{0:n4}", total), bold_font);
                bodyNonIDRTable.AddCell(bodyNonIDRCell);

                document.Add(bodyNonIDRTable);

                #endregion BodyNonIDR
            }
            else
            {
                #region Body
                sameCurrency = false;
                PdfPTable bodyTable = new PdfPTable(7);
                PdfPCell bodyCell = new PdfPCell();

                float[] widthsBody = new float[] { 5f, 10f, 10f, 10f, 7f, 10f, 10f };
                bodyTable.SetWidths(widthsBody);
                bodyTable.WidthPercentage = 100;

                bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                bodyCell.Phrase = new Phrase("No.", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("No. Disposisi", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Kategori Barang", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Divisi", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Mata Uang", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Jumlah", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Phrase = new Phrase("Jumlah (IDR)", bold_font);
                bodyTable.AddCell(bodyCell);

                foreach (PaymentDispositionNoteItemViewModel item in viewModel.Items)
                {
                    var purchasingDisposition = purchasingDispositions.FirstOrDefault(element => element.Id == item.purchasingDispositionExpeditionId);

                    if (purchasingDisposition == null)
                        purchasingDisposition = new PurchasingDispositionExpeditionModel();

                    double remaining = item.SupplierPayment;
                    double previousPayment = item.AmountPaid;

                    var unitSummaries = item.Details.GroupBy(g => g.unit.code).Select(s => new
                    {
                        UnitCode = s.Key
                    });

                    //var details = item.Details
                    //    .GroupBy(m => new { m.unit.code, m.unit.name })
                    //    .Select(s => new
                    //    {
                    //        s.First().unit.code,
                    //        s.First().unit.name,
                    //        Total = s.Sum(d => d.price)
                    //    });

                    if (unitSummaries.Count() > 1)
                    {
                        foreach (var detail in item.Details)
                        {
                            var vatAmount = purchasingDisposition.UseVat ? detail.price * 0.1 : 0;
                            var incomeTaxAmount = purchasingDisposition.UseIncomeTax ? detail.price * purchasingDisposition.IncomeTaxRate / 100 : 0;
                            var dpp = detail.price + vatAmount - incomeTaxAmount;

                            if (remaining <= 0)
                            {
                                continue;
                            }

                            if (previousPayment > 0)
                            {
                                if (previousPayment >= dpp)
                                {
                                    previousPayment -= dpp;

                                    continue;
                                }
                                else
                                {
                                    dpp -= previousPayment;

                                    previousPayment -= dpp;
                                }
                            }

                            bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            bodyCell.VerticalAlignment = Element.ALIGN_TOP;
                            bodyCell.Phrase = new Phrase((index++).ToString(), normal_font);
                            bodyTable.AddCell(bodyCell);

                            bodyCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            bodyCell.Phrase = new Phrase(item.dispositionNo, normal_font);
                            bodyTable.AddCell(bodyCell);

                            bodyCell.Phrase = new Phrase(item.category.name, normal_font);
                            bodyTable.AddCell(bodyCell);

                            bodyCell.Phrase = new Phrase(item.division.name, normal_font);
                            bodyTable.AddCell(bodyCell);

                            bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            bodyCell.Phrase = new Phrase(viewModel.CurrencyCode, normal_font);
                            bodyTable.AddCell(bodyCell);

                            if (remaining >= dpp)
                            {
                                bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                bodyCell.Phrase = new Phrase(string.Format("{0:n4}", dpp), normal_font);
                                bodyTable.AddCell(bodyCell);

                                bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                bodyCell.Phrase = new Phrase(string.Format("{0:n4}", (dpp * viewModel.CurrencyRate)), normal_font);
                                bodyTable.AddCell(bodyCell);

                                if (units.ContainsKey(detail.unit.code))
                                {
                                    units[detail.unit.code] += dpp;
                                }
                                else
                                {
                                    units.Add(detail.unit.code, dpp);
                                }

                                totalPay += dpp;
                                total += dpp;
                                remaining -= dpp;
                            }
                            else
                            {
                                bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                bodyCell.Phrase = new Phrase(string.Format("{0:n4}", remaining), normal_font);
                                bodyTable.AddCell(bodyCell);

                                bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                bodyCell.Phrase = new Phrase(string.Format("{0:n4}", (remaining * viewModel.CurrencyRate)), normal_font);
                                bodyTable.AddCell(bodyCell);

                                if (units.ContainsKey(detail.unit.code))
                                {
                                    units[detail.unit.code] += remaining;
                                }
                                else
                                {
                                    units.Add(detail.unit.code, remaining);
                                }

                                totalPay += remaining;
                                total += remaining;
                                remaining -= remaining;
                            }
                        }
                    }
                    else
                    {
                        foreach (var detail in unitSummaries)
                        {
                            bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            bodyCell.VerticalAlignment = Element.ALIGN_TOP;
                            bodyCell.Phrase = new Phrase((index++).ToString(), normal_font);
                            bodyTable.AddCell(bodyCell);

                            bodyCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            bodyCell.Phrase = new Phrase(item.dispositionNo, normal_font);
                            bodyTable.AddCell(bodyCell);

                            bodyCell.Phrase = new Phrase(item.category.name, normal_font);
                            bodyTable.AddCell(bodyCell);

                            bodyCell.Phrase = new Phrase(item.division.name, normal_font);
                            bodyTable.AddCell(bodyCell);

                            bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            bodyCell.Phrase = new Phrase(viewModel.CurrencyCode, normal_font);
                            bodyTable.AddCell(bodyCell);

                            bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            bodyCell.Phrase = new Phrase(string.Format("{0:n4}", remaining), normal_font);
                            bodyTable.AddCell(bodyCell);

                            bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            bodyCell.Phrase = new Phrase(string.Format("{0:n4}", (remaining * viewModel.CurrencyRate)), normal_font);
                            bodyTable.AddCell(bodyCell);

                            if (units.ContainsKey(detail.UnitCode))
                            {
                                units[detail.UnitCode] += remaining;
                            }
                            else
                            {
                                units.Add(detail.UnitCode, remaining);
                            }

                            totalPay += remaining;
                            total += remaining;
                        }
                    }
                }

                foreach (var un in units)
                {
                    percentageUnits[un.Key] = (un.Value * viewModel.CurrencyRate) * 100 / (totalPay * viewModel.CurrencyRate);
                }

                bodyCell.Colspan = 3;
                bodyCell.Border = Rectangle.NO_BORDER;
                bodyCell.Phrase = new Phrase("", normal_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Colspan = 1;
                bodyCell.Border = Rectangle.BOX;
                bodyCell.HorizontalAlignment = Element.ALIGN_LEFT;
                bodyCell.Phrase = new Phrase("Total", bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.Colspan = 1;
                bodyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                bodyCell.Phrase = new Phrase(viewModel.AccountBank.Currency.Code, bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                bodyCell.Phrase = new Phrase(string.Format("{0:n4}", total), bold_font);
                bodyTable.AddCell(bodyCell);

                bodyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                bodyCell.Phrase = new Phrase(string.Format("{0:n4}", total * viewModel.CurrencyRate), bold_font);
                bodyTable.AddCell(bodyCell);

                document.Add(bodyTable);

                #endregion Body
            }



            #region BodyFooter

            PdfPTable bodyFooterTable = new PdfPTable(6);
            bodyFooterTable.SetWidths(new float[] { 3f, 6f, 2f, 6f, 10f, 10f });
            bodyFooterTable.WidthPercentage = 100;

            PdfPCell bodyFooterCell = new PdfPCell() { Border = Rectangle.NO_BORDER };

            bodyFooterCell.Colspan = 1;
            bodyFooterCell.Phrase = new Phrase("");
            bodyFooterTable.AddCell(bodyFooterCell);

            bodyFooterCell.Colspan = 1;
            bodyFooterCell.HorizontalAlignment = Element.ALIGN_LEFT;
            bodyFooterCell.Phrase = new Phrase("Rincian per bagian:", normal_font);
            bodyFooterTable.AddCell(bodyFooterCell);

            bodyFooterCell.Colspan = 4;
            bodyFooterCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            bodyFooterCell.Phrase = new Phrase("");
            bodyFooterTable.AddCell(bodyFooterCell);

            total = viewModel.CurrencyId > 0 && sameCurrency == false ? total * viewModel.CurrencyRate : total;

            foreach (var unit in percentageUnits)
            {
                bodyFooterCell.Colspan = 1;
                bodyFooterCell.Phrase = new Phrase("");
                bodyFooterTable.AddCell(bodyFooterCell);

                bodyFooterCell.Phrase = new Phrase(unit.Key, normal_font);
                bodyFooterTable.AddCell(bodyFooterCell);

                bodyFooterCell.Phrase = new Phrase(viewModel.AccountBank.Currency.Code, normal_font);
                bodyFooterTable.AddCell(bodyFooterCell);

                //bodyFooterCell.Phrase = new Phrase(string.Format("{0:n4}", unit.Value), normal_font);
                //bodyFooterTable.AddCell(bodyFooterCell);



                bodyFooterCell.Phrase = new Phrase(string.Format("{0:n4}", unit.Value * total / 100), normal_font);
                bodyFooterTable.AddCell(bodyFooterCell);


                bodyFooterCell.Colspan = 2;
                bodyFooterCell.Phrase = new Phrase("");
                bodyFooterTable.AddCell(bodyFooterCell);
            }

            bodyFooterCell.Colspan = 6;
            bodyFooterCell.HorizontalAlignment = Element.ALIGN_LEFT;
            bodyFooterCell.Phrase = new Phrase("");
            bodyFooterTable.AddCell(bodyFooterCell);


            bodyFooterCell.Colspan = 1;
            bodyFooterCell.HorizontalAlignment = Element.ALIGN_LEFT;
            bodyFooterCell.Phrase = new Phrase("");
            bodyFooterTable.AddCell(bodyFooterCell);

            bodyFooterCell.Phrase = new Phrase("Terbilang", normal_font);
            bodyFooterTable.AddCell(bodyFooterCell);

            bodyFooterCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            bodyFooterCell.Phrase = new Phrase(": " + viewModel.AccountBank.Currency.Code, normal_font);
            bodyFooterTable.AddCell(bodyFooterCell);

            bodyFooterCell.Colspan = 3;
            bodyFooterCell.HorizontalAlignment = Element.ALIGN_LEFT;
            bodyFooterCell.Phrase = new Phrase(NumberToTextIDN.terbilang(total), normal_font);
            bodyFooterTable.AddCell(bodyFooterCell);


            document.Add(bodyFooterTable);
            document.Add(new Paragraph("\n"));

            #endregion BodyFooter

            #region Footer

            PdfPTable footerTable = new PdfPTable(2);
            PdfPCell cellFooter = new PdfPCell() { Border = Rectangle.NO_BORDER };

            float[] widthsFooter = new float[] { 10f, 5f };
            footerTable.SetWidths(widthsFooter);
            footerTable.WidthPercentage = 100;

            cellFooter.Phrase = new Phrase("Dikeluarkan dengan cek/BG No. : " + viewModel.BGCheckNumber, normal_font);
            footerTable.AddCell(cellFooter);

            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);

            PdfPTable signatureTable = new PdfPTable(3);
            PdfPCell signatureCell = new PdfPCell() { HorizontalAlignment = Element.ALIGN_CENTER };
            signatureCell.Phrase = new Phrase("Bag. Keuangan", normal_font);
            signatureTable.AddCell(signatureCell);

            signatureCell.Colspan = 2;
            signatureCell.HorizontalAlignment = Element.ALIGN_CENTER;
            signatureCell.Phrase = new Phrase("Direksi", normal_font);
            signatureTable.AddCell(signatureCell);

            signatureTable.AddCell(new PdfPCell()
            {
                Phrase = new Phrase("---------------------------", normal_font),
                FixedHeight = 40,
                VerticalAlignment = Element.ALIGN_BOTTOM,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            signatureTable.AddCell(new PdfPCell()
            {
                Phrase = new Phrase("---------------------------", normal_font),
                FixedHeight = 40,
                Border = Rectangle.NO_BORDER,
                VerticalAlignment = Element.ALIGN_BOTTOM,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            signatureTable.AddCell(new PdfPCell()
            {
                Phrase = new Phrase("---------------------------", normal_font),
                FixedHeight = 40,
                Border = Rectangle.NO_BORDER,
                VerticalAlignment = Element.ALIGN_BOTTOM,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            footerTable.AddCell(new PdfPCell(signatureTable));

            cellFooter.Phrase = new Phrase("", normal_font);
            footerTable.AddCell(cellFooter);
            document.Add(footerTable);

            #endregion Footer

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }

    public class ResponseAmountPaidandIsPosted
    {
        public double AmountPaid { get; set; }
        public bool IsPosted { get; set; }
    }
}

using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionAcceptance;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionReport;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionVerification;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.PurchasingDispositionExpedition
{
    public class PurchasingDispositionExpeditionService : IPurchasingDispositionExpeditionService
    {
        private const string UserAgent = "finance-service";
        protected DbSet<PurchasingDispositionExpeditionModel> DbSet;
        public IIdentityService IdentityService;
        public readonly IServiceProvider ServiceProvider;
        public FinanceDbContext DbContext;

        public PurchasingDispositionExpeditionService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            DbContext = dbContext;
            ServiceProvider = serviceProvider;
            DbSet = dbContext.Set<PurchasingDispositionExpeditionModel>();
            IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        public void CreateModel(PurchasingDispositionExpeditionModel m)
        {
            EntityExtension.FlagForCreate(m, IdentityService.Username, UserAgent);
            m.Position = ExpeditionPosition.SEND_TO_VERIFICATION_DIVISION;
            foreach (var item in m.Items)
            {
                EntityExtension.FlagForCreate(item, IdentityService.Username, UserAgent);
            }
            List<string> dispoNo = new List<string>();
            dispoNo.Add(m.DispositionNo);
            UpdateDispositionPosition(dispoNo, ExpeditionPosition.SEND_TO_VERIFICATION_DIVISION);

            DbSet.Add(m);
        }

        public async Task<int> CreateAsync(PurchasingDispositionExpeditionModel m)
        {
            CreateModel(m);

            return await DbContext.SaveChangesAsync();
        }

        public async Task DeleteModel(int id)
        {
            PurchasingDispositionExpeditionModel model = await ReadByIdAsync(id);
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
            }

            List<string> dispoNo = new List<string>();
            dispoNo.Add(model.DispositionNo);

            var dispoCount = this.DbSet.Count(x => x.DispositionNo == model.DispositionNo && x.IsDeleted == false && x.Id != model.Id);
            if (dispoCount > 0)
            {
                UpdateDispositionPosition(dispoNo, ExpeditionPosition.SEND_TO_PURCHASING_DIVISION);
            }
            else
            {
                UpdateDispositionPosition(dispoNo, ExpeditionPosition.PURCHASING_DIVISION);
            }

            EntityExtension.FlagForDelete(model, IdentityService.Username, UserAgent, true);
            DbSet.Update(model);
        }

        public async Task<int> DeleteAsync(int id)
        {
            await DeleteModel(id);
            return await DbContext.SaveChangesAsync();
        }

        public ReadResponse<PurchasingDispositionExpeditionModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {

            IQueryable<PurchasingDispositionExpeditionModel> Query = this.DbSet.Include(m => m.Items);
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

        public Task<PurchasingDispositionExpeditionModel> ReadByIdAsync(int id)
        {
            return DbSet.Include(m => m.Items).FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public Task<int> Update(int id, PurchasingDispositionExpeditionModel m, string user, int clientTimeZoneOffset = 7)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(int id, PurchasingDispositionExpeditionModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<int> PurchasingDispositionAcceptance(PurchasingDispositionAcceptanceViewModel data)
        {

            int updated = 0;

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    List<string> dispositions = new List<string>();

                    if (data.Role.Equals("VERIFICATION"))
                    {
                        foreach (var item in data.PurchasingDispositionExpedition)
                        {
                            dispositions.Add(item.DispositionNo);
                            PurchasingDispositionExpeditionModel model = DbContext.PurchasingDispositionExpeditions.Single(x => x.Id == item.Id);
                            model.VerificationDivisionBy = IdentityService.Username;
                            model.VerificationDivisionDate = DateTimeOffset.UtcNow;
                            model.Position = ExpeditionPosition.VERIFICATION_DIVISION;

                            EntityExtension.FlagForUpdate(model, IdentityService.Username, UserAgent);
                        }

                        updated = await DbContext.SaveChangesAsync();
                        UpdateDispositionPosition(dispositions, ExpeditionPosition.VERIFICATION_DIVISION);
                    }
                    else if (data.Role.Equals("CASHIER"))
                    {
                        foreach (var item in data.PurchasingDispositionExpedition)
                        {
                            dispositions.Add(item.DispositionNo);
                            PurchasingDispositionExpeditionModel model = DbContext.PurchasingDispositionExpeditions.Single(x => x.Id == item.Id);
                            model.CashierDivisionBy = IdentityService.Username;
                            model.CashierDivisionDate = DateTimeOffset.UtcNow;
                            model.Position = ExpeditionPosition.CASHIER_DIVISION;

                            EntityExtension.FlagForUpdate(model, IdentityService.Username, UserAgent);
                        }

                        updated = await DbContext.SaveChangesAsync();
                        UpdateDispositionPosition(dispositions, ExpeditionPosition.CASHIER_DIVISION);
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }

            return updated;
        }

        public async Task<int> DeletePurchasingDispositionAcceptance(int id)
        {
            int count = 0;

            if (DbContext.PurchasingDispositionExpeditions.Count(x => x.Id == id && !x.IsDeleted).Equals(0))
            {
                return 0;
            }

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    PurchasingDispositionExpeditionModel purchasingDispositionExpedition = DbContext.PurchasingDispositionExpeditions.Single(x => x.Id == id);

                    if (purchasingDispositionExpedition.Position == ExpeditionPosition.VERIFICATION_DIVISION)
                    {
                        purchasingDispositionExpedition.VerificationDivisionBy = null;
                        purchasingDispositionExpedition.VerificationDivisionDate = null;
                        purchasingDispositionExpedition.Position = ExpeditionPosition.SEND_TO_VERIFICATION_DIVISION;

                        EntityExtension.FlagForUpdate(purchasingDispositionExpedition, IdentityService.Username, UserAgent);

                        count = await DbContext.SaveChangesAsync();
                        UpdateDispositionPosition(new List<string>() { purchasingDispositionExpedition.DispositionNo }, ExpeditionPosition.SEND_TO_VERIFICATION_DIVISION);
                    }
                    else if (purchasingDispositionExpedition.Position == ExpeditionPosition.CASHIER_DIVISION)
                    {
                        purchasingDispositionExpedition.CashierDivisionBy = null;
                        purchasingDispositionExpedition.CashierDivisionDate = null;
                        purchasingDispositionExpedition.Position = ExpeditionPosition.SEND_TO_CASHIER_DIVISION;

                        EntityExtension.FlagForUpdate(purchasingDispositionExpedition, IdentityService.Username, UserAgent);

                        count = await DbContext.SaveChangesAsync();
                        UpdateDispositionPosition(new List<string>() { purchasingDispositionExpedition.DispositionNo }, ExpeditionPosition.SEND_TO_CASHIER_DIVISION);
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }

            return count;
        }

        public async Task<int> PurchasingDispositionVerification(PurchasingDispositionVerificationViewModel data)
        {
            int updated = 0;

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    PurchasingDispositionExpeditionModel model;
                    if (data.Id == 0)
                    {
                        model = DbContext.PurchasingDispositionExpeditions.OrderByDescending(x => x.LastModifiedUtc).First(x => x.DispositionNo == data.DispositionNo);
                    }
                    else
                    {
                        model = DbContext.PurchasingDispositionExpeditions.Single(x => x.Id == data.Id);
                    }


                    if (data.SubmitPosition == ExpeditionPosition.SEND_TO_PURCHASING_DIVISION)
                    {
                        model.DispositionNo = data.DispositionNo;
                        model.VerifyDate = data.VerifyDate;
                        model.SendToPurchasingDivisionBy = IdentityService.Username;
                        model.SendToPurchasingDivisionDate = data.VerifyDate;
                        model.Position = ExpeditionPosition.SEND_TO_PURCHASING_DIVISION;
                        model.Active = false;
                        model.NotVerifiedReason = data.Reason;

                        model.SendToCashierDivisionBy = null;
                        model.SendToCashierDivisionDate = DateTimeOffset.MinValue;


                        EntityExtension.FlagForUpdate(model, IdentityService.Username, UserAgent);
                        updated = await DbContext.SaveChangesAsync();
                        UpdateDispositionPosition(new List<string>() { model.DispositionNo }, ExpeditionPosition.SEND_TO_PURCHASING_DIVISION);
                    }
                    else if (data.SubmitPosition == ExpeditionPosition.SEND_TO_CASHIER_DIVISION)
                    {
                        model.DispositionNo = data.DispositionNo;
                        model.VerifyDate = data.VerifyDate;
                        model.SendToCashierDivisionBy = IdentityService.Username;
                        model.SendToCashierDivisionDate = data.VerifyDate;
                        model.Position = ExpeditionPosition.SEND_TO_CASHIER_DIVISION;
                        model.Active = true;

                        model.SendToPurchasingDivisionBy = null;
                        model.SendToPurchasingDivisionDate = DateTimeOffset.MinValue;
                        model.NotVerifiedReason = null;

                        EntityExtension.FlagForUpdate(model, IdentityService.Username, UserAgent);
                        updated = await DbContext.SaveChangesAsync();
                        UpdateDispositionPosition(new List<string>() { model.DispositionNo }, ExpeditionPosition.SEND_TO_CASHIER_DIVISION);
                    }


                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }

            return updated;
        }

        private void UpdateDispositionPosition(List<string> dispositions, ExpeditionPosition position)
        {
            string dispositionUri = "purchasing-dispositions/update/position";

            var data = new
            {
                Position = position,
                PurchasingDispositionNoes = dispositions
            };

            IHttpClientService httpClient = (IHttpClientService)this.ServiceProvider.GetService(typeof(IHttpClientService));
            var response = httpClient.PutAsync($"{APIEndpoint.Purchasing}{dispositionUri}", new StringContent(JsonConvert.SerializeObject(data).ToString(), Encoding.UTF8, General.JsonMediaType)).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("{0}, {1}, {2}", response.StatusCode, response.Content, APIEndpoint.Purchasing));
            }
        }

        private async Task<PurchasingDispositionBaseResponseViewModel> JoinReportAsync(int page, int size, string order, string filter, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, DateTimeOffset? dateFromPayment, DateTimeOffset? dateToPayment, string bankExpenditureNoteNo, string SPBStatus, string PaymentStatus, int offSet)
        {
			DateTimeOffset dateFromPaymentFilter = (dateFromPayment == null ? Convert.ToDateTime("0001-01-01 00:00:00.0000000 +00:00") : dateFromPayment.Value.Date);
			DateTimeOffset dateToPaymentFilter = (dateToPayment == null ? DateTimeOffset.UtcNow.Date : dateToPayment.Value.Date);
			DateTimeOffset dateFromFilter = (dateFrom == null ? new DateTime(1970, 1, 1) : dateFrom.Value.Date);
			DateTimeOffset dateToFilter = (dateTo == null ? DateTimeOffset.UtcNow.Date : dateTo.Value.Date);
			var expeditionData = DbSet.Include(entity => entity.Items).ToList();
			var purchasingDispositionResponse = await GetPurchasingDispositionAsync(1, int.MaxValue, order, filter);
             
            List<PurchasingDispositionViewModel> data = purchasingDispositionResponse.data;

            List<PurchasingDispositionReportViewModel> result = new List<PurchasingDispositionReportViewModel>();
			
	
			data = data.Where(x => x.CreatedUtc.AddHours(offSet).Date >= dateFromFilter
						 && x.CreatedUtc.AddHours(offSet).Date <= dateToFilter).ToList();
			//if (dateFrom == null && dateTo == null)
			//{
			//    data = data
			//        .Where(x => DateTimeOffset.MinValue.Date <= x.CreatedUtc.AddHours(offSet).Date
			//            && x.CreatedUtc.AddHours(offSet).Date <= DateTime.UtcNow.Date).ToList();
			//}
			//else if (dateFrom == null && dateTo != null)
			//{
			//    data = data
			//        .Where(x => DateTimeOffset.MinValue.Date <= x.CreatedUtc.AddHours(offSet).Date
			//            && x.CreatedUtc.AddHours(offSet).Date <= dateTo.Value.Date).ToList();
			//}
			//else if (dateTo == null && dateFrom != null)
			//{
			//    data = data
			//        .Where(x => dateFrom.Value.Date <= x.CreatedUtc.AddHours(offSet).Date
			//            && x.CreatedUtc.AddHours(offSet).Date <= DateTime.UtcNow.Date).ToList();
			//}
			//else
			//{
			//    data = data
			//        .Where(x => dateFrom.Value.Date <= x.CreatedUtc.AddHours(offSet).Date
			//            && x.CreatedUtc.AddHours(offSet).Date <= dateTo.Value.Date).ToList();
			//}

			foreach (var item in data)
            {
                List<UnitPaymentOrderViewModel> dataupo = new List<UnitPaymentOrderViewModel>();
                List<string> dono = new List<string>();
                List<string> urnno = new List<string>();
				//var expedition = DbContext.PurchasingDispositionExpeditions.Where(x => x.DispositionNo == item.DispositionNo).Include(x => x.Items).FirstOrDefault();
			    
				var expeditions = expeditionData.OrderByDescending(a => a.LastModifiedUtc).Where(x => x.DispositionNo == item.DispositionNo).ToList();
				
				foreach (var expedition in expeditions)
                {
                    if (expedition != null)
                    {
                        foreach (var item2 in expedition.Items)
                        {
                            var epo = item2.EPOId != null ? GetExternalPurchaseOrderNo(item2.EPOId) : null;
                            item2.EPONo = epo != null ? epo.no : "-";
                            //var DO = epo != null ? GetDeliveryOrderNo(epo.no) : null;
                            var upo = GetUnitPaymentOrder(item2.EPONo);
                            foreach (var i in upo)
                            {
                                dataupo.Add(i);
                                foreach (var t in i.items)
                                {
                                    dono.Add(t.unitReceiptNote.deliveryOrder.no);
                                    urnno.Add(t.unitReceiptNote.no);

                                }
                            }
                        }
                    }



                    PurchasingDispositionReportViewModel vm = new PurchasingDispositionReportViewModel()
                    {
                        BankExpenditureNoteDate = expedition == null || expedition.BankExpenditureNoteDate == DateTimeOffset.MinValue ? DateTimeOffset.MinValue : expedition.BankExpenditureNoteDate,
                        DispositionNo = item.DispositionNo,
                        BankExpenditureNoteNo = expedition?.BankExpenditureNoteNo,
                        BankExpenditureNotePPHDate = expedition == null || expedition.BankExpenditureNotePPHDate == DateTimeOffset.MinValue ? null : expedition.BankExpenditureNotePPHDate,
                        BankExpenditureNotePPHNo = expedition?.BankExpenditureNotePPHNo,
                        CashierDivisionDate = expedition == null || expedition.CashierDivisionDate == DateTimeOffset.MinValue ? null : expedition.CashierDivisionDate,
                        CreatedUtc = item.CreatedUtc,
                        InvoiceNo = item.ProformaNo,
                        PaymentDueDate = item.PaymentDueDate,
                        //Position = expedition.Position == ExpeditionPosition.CASHIER_DIVISION ? 7 : 6,
                        Position = (int)expedition.Position,
                        SentToVerificationDivisionDate = expedition == null ? null : new DateTimeOffset?(expedition.CreatedUtc),
                        SendDate = expedition == null ? null : ((expedition.Position == ExpeditionPosition.CASHIER_DIVISION || expedition.Position == ExpeditionPosition.SEND_TO_CASHIER_DIVISION) && expedition.SendToCashierDivisionDate != DateTimeOffset.MinValue) ? expedition.SendToCashierDivisionDate :
                         ((expedition.Position == ExpeditionPosition.SEND_TO_PURCHASING_DIVISION) && expedition.SendToPurchasingDivisionDate != DateTimeOffset.MinValue) ? expedition.SendToPurchasingDivisionDate : null,
                        SupplierName = item.Supplier.name,
                        VerificationDivisionDate = expedition == null || expedition.VerificationDivisionDate == DateTimeOffset.MinValue ? null : expedition.VerificationDivisionDate,
                        VerifyDate = expedition == null || expedition.VerifyDate == DateTimeOffset.MinValue ? null : expedition.VerifyDate,
                        Staff = item == null ? "" : item.CreatedBy,
                        PayToSupplier = expedition != null ? expedition.PayToSupplier : 0,
                        Currency = expedition != null ? expedition.CurrencyCode : "",
                        CurrencyRate = item.Currency.rate,
                        Category = expedition != null ? expedition.CategoryName : "",
                        Division = expedition != null ? expedition.DivisionName : "",
                        Unit = expedition != null ? string.Join("\n", expedition.Items.Select(expeditionItem => $"- {expeditionItem.UnitName}")) : "",
                        DPP = expedition != null ? (decimal)expedition.DPP : 0,
                        DueDateDays = 0,
                        ExternalPurchaseOrderNo = expedition != null ? string.Join(" & ", expedition.Items.Select(expeditionItem => $" {expeditionItem.EPONo}")) : "",
                        IncomeTax = expedition != null ? (decimal)expedition.IncomeTaxValue : 0,
                        VAT = expedition != null ? (decimal)expedition.VatValue : 0,
                        Total = expedition != null ? (decimal)(expedition.DPP + expedition.VatValue - expedition.IncomeTaxValue) : 0,
                        VerifiedBy = expedition != null ? expedition.VerificationDivisionBy : "",
                        UnitPaymentOrderNo = dataupo != null ? string.Join(" & ", dataupo.Select(upono => $" {upono.no}").Distinct()) : "",
                        UnitPaymentOrderDate = dataupo != null ? string.Join(" & ", dataupo.Select(upodate => $" {upodate.date.Value.Date.ToString("dd MMM yyyy")}").Distinct()) : "",
                        DONo = dataupo != null ? string.Join(" & ", dono.Distinct()) : "",
                        UrnNo = dataupo != null ? string.Join(" & ", urnno.Distinct()) : "",
                        DifferenceNominal = expedition != null ? expedition.PayToSupplier - (expedition.AmountPaid + expedition.SupplierPayment) : 0,
                        PaymentCorrection = expedition != null ? expedition.PaymentCorrection : 0,
                        SupplierPayment = expedition != null ? expedition.SupplierPayment : 0
                    };
                    result.Add(vm);
                }

            }

			result = result.Where(a => a.BankExpenditureNoteDate.Value.AddHours(offSet).Date >= dateFromPaymentFilter.Date && a.BankExpenditureNoteDate.Value.AddHours(offSet).Date <= dateToPaymentFilter.Date).ToList();
			if (PaymentStatus == "SUDAH DIBAYAR")
			{
				result = result.Where(s => s.BankExpenditureNoteNo != null).ToList();
			}
			else if (PaymentStatus == "BELUM DIBAYAR")
			{
				result = result.Where(s => s.BankExpenditureNoteNo == null).ToList();
			}
			if (SPBStatus == "SUDAH ADA")
			{
				result = result.Where(s => s.UnitPaymentOrderNo != "").ToList();
			}
			else if (SPBStatus == "BELUM ADA")
			{
				result = result.Where(s => s.UnitPaymentOrderNo == "").ToList();
			}
			
			if (!String.IsNullOrEmpty( bankExpenditureNoteNo))
			{
				result = result.Where(s => s.BankExpenditureNoteNo == bankExpenditureNoteNo).ToList();
				 
			}
			 
			foreach (var i in result)
			{
				i.ExternalPurchaseOrderNo = i.ExternalPurchaseOrderNo == "" ? "-" : i.ExternalPurchaseOrderNo;
				i.BankExpenditureNoteDate = i.BankExpenditureNoteDate == Convert.ToDateTime("0001-01-01 00:00:00.0000000 +00:00") ? null : i.BankExpenditureNoteDate;
			}
			return new PurchasingDispositionBaseResponseViewModel
			{
				info = purchasingDispositionResponse.info,
				data = result
			};

		}

        public async Task<MemoryStream> GenerateExcelAsync(int page, int size, string order, string filter, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, DateTimeOffset? dateFromPayment, DateTimeOffset? dateToPayment, string bankExpenditureNoteNo, string SPBStatus, string PaymentStatus, int offSet)
        {
            var data = await JoinReportAsync(page, size, order, filter, dateFrom, dateTo,dateFromPayment,dateToPayment, bankExpenditureNoteNo,SPBStatus,PaymentStatus,offSet);
            string title = "Laporan Ekspedisi Disposisi Pembayaran",
                _dateFrom = dateFrom == null ? "-" : dateFrom.Value.ToString("dd MMMM yyyy"),
                _dateTo = dateTo == null ? "-" : dateTo.Value.ToString("dd MMMM yyyy");

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Disposisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Disposisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Jatuh Tempo", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Proforma", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Supplier", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kurs", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "DPP", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "PPN", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "PPh", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Koreksi Pembayaran", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Total", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tempo", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kategori", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Unit", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Divisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Posisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Pembelian Kirim", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Terima Verifikasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Cek Verifikasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Kirim", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Verifikator", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Terima Kasir", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Bayar Kasir", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No Bukti Pengeluaran Bank", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nominal yang dibayar", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Sisa yang Belum Dibayar", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mata Uang", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "PO Eksternal", DataType = typeof(string) });
			dt.Columns.Add(new DataColumn() { ColumnName = "No Surat Jalan", DataType = typeof(string) });
			dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Bon Terima", DataType = typeof(string) });
			dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal SPB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor SPB", DataType = typeof(string) });
            //dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Bayar PPH Kasir", DataType = typeof(string) });
            //dt.Columns.Add(new DataColumn() { ColumnName = "No Kuitansi PPHKasir", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Staf", DataType = typeof(string) });

            int index = 0;
            if (data.data.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", 0.ToString("#,##0.#0"), 0.ToString("#,##0.#0"), 0.ToString("#,##0.#0"), 0.ToString("#,##0.#0"), 0.ToString("#,##0.#0"), "", "", "", "", "", "", "", "", "", "", "","","", 0.ToString("#,##0.#0"), "", "", "", "", "", "", "");
                index++;
            }
            else
            {
                foreach (var item in data.data)
                {
                    dt.Rows.Add(
                        item.DispositionNo,
                        item.CreatedUtc == null ? "-" : item.CreatedUtc.Value.AddHours(offSet).ToString("dd MMM yyyy"),
                        item.PaymentDueDate == null ? "-" : item.PaymentDueDate.Value.AddHours(offSet).ToString("dd MMM yyyy"),
                        item.InvoiceNo,
                        item.SupplierName,
                        item.CurrencyRate.ToString("#,##0.#0"),
                        item.DPP.ToString("#,##0.#0"),
                        item.VAT.ToString("#,##0.#0"),
                        item.IncomeTax.ToString("#,##0.#0"),
                        item.PaymentCorrection.ToString("#,##0.#0"),
                        item.PayToSupplier.ToString("#,##0.#0"),
                        item.DueDateDays,
                        item.Category,
                        item.Unit,
                        item.Division,
                        item.Position == 0 ? "-" : item.Position == 4 ? "Dikirim ke Bag. Keuangan" : item.Position == 7 ? "Bag. Keuangan" : ((ExpeditionPosition)item.Position).ToDescriptionString(),
                        item.SentToVerificationDivisionDate == null ? "-" : item.SentToVerificationDivisionDate.Value.AddHours(offSet).ToString("dd MMM yyyy"),
                        item.VerificationDivisionDate == null ? "-" : item.VerificationDivisionDate.Value.AddHours(offSet).ToString("dd MMM yyyy"),
                        item.VerifyDate == null ? "-" : item.VerifyDate.Value.AddHours(offSet).ToString("dd MMM yyyy"),
                        item.SendDate == null ? "-" : item.SendDate.Value.AddHours(offSet).ToString("dd MMM yyyy"),
                        item.VerifiedBy,
                        item.CashierDivisionDate == null ? "-" : item.CashierDivisionDate.Value.AddHours(offSet).ToString("dd MMM yyyy"),
                        item.BankExpenditureNoteDate == null ? "-" : item.BankExpenditureNoteDate.Value.AddHours(offSet).ToString("dd MMM yyyy"),
                        string.IsNullOrEmpty(item.BankExpenditureNoteNo) ? "-" : item.BankExpenditureNoteNo,
                        //item.BankExpenditureNotePPHDate == null ? "-" : item.BankExpenditureNotePPHDate.Value.AddHours(offSet).ToString("dd MMM yyyy"), string.IsNullOrEmpty(item.BankExpenditureNotePPHNo) ? "-" : item.BankExpenditureNotePPHNo,
                        item.SupplierPayment.ToString("#,##0.#0"),
                        item.DifferenceNominal.ToString("#,##0.#0"),
                        item.Currency,
                        item.ExternalPurchaseOrderNo,
						item.DONo,
						item.UrnNo,
                        item.UnitPaymentOrderDate,
                        item.UnitPaymentOrderNo,
                        item.Staff);
                    index++;
                }
            }
            return Excel.CreateExcelWithTitle(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Disposisi Pembelian") },
                new List<KeyValuePair<string, int>>() { new KeyValuePair<string, int>("Disposisi Pembelian", index) }, title, _dateFrom, _dateTo, true);
        }

        public async Task<ReadResponse<PurchasingDispositionReportViewModel>> GetReportAsync(int page, int size, string order, string filter, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, DateTimeOffset? dateFromPayment, DateTimeOffset? dateToPayment,string bankExpenditureNoteNo, string SPBStatus, string PaymentStatus, int offSet)
        {
            var queries = await JoinReportAsync(page, size, order, filter, dateFrom, dateTo, dateFromPayment,dateToPayment,bankExpenditureNoteNo,SPBStatus,PaymentStatus, offSet);
            Pageable<PurchasingDispositionReportViewModel> pageable = new Pageable<PurchasingDispositionReportViewModel>(queries.data, page - 1, size);
            List<PurchasingDispositionReportViewModel> data = pageable.Data.ToList();
            //List<PurchasingDispositionReportViewModel> data = queries.data;
            return new ReadResponse<PurchasingDispositionReportViewModel>(data, pageable.TotalCount, new Dictionary<string, string>(), new List<string>());
        }

        private async Task<PurchasingDispositionResponseViewModel> GetPurchasingDispositionAsync(int page, int size, string order, string filter)
        {
            string dispositionUri = "purchasing-dispositions";
            string queryUri = "?page=" + page + "&size=" + size + "&order=" + order + "&filter=" + filter;
            string uri = dispositionUri + queryUri;
            IHttpClientService httpClient = (IHttpClientService)this.ServiceProvider.GetService(typeof(IHttpClientService));
            var response = await httpClient.GetAsync($"{APIEndpoint.Purchasing}{uri}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("{0}, {1}, {2}", response.StatusCode, response.Content, APIEndpoint.Purchasing));
            }
            else
            {
                PurchasingDispositionResponseViewModel result = JsonConvert.DeserializeObject<PurchasingDispositionResponseViewModel>(response.Content.ReadAsStringAsync().Result);
                return result;
            }
        }
        private ExternalPurchaseOrderViewModel GetExternalPurchaseOrderNo(string id)
        {
            string epoUri = "external-purchase-orders";
            IHttpClientService httpClient = (IHttpClientService)this.ServiceProvider.GetService(typeof(IHttpClientService));
            var response = httpClient.GetAsync($"{APIEndpoint.Purchasing}{epoUri}/{id}").Result;
            if(response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                ExternalPurchaseOrderViewModel epo = JsonConvert.DeserializeObject<ExternalPurchaseOrderViewModel>(result.GetValueOrDefault("data").ToString());
                return epo;
            }
            else
            {
                return null;
            }

        }
        private List<UnitPaymentOrderViewModel> GetUnitPaymentOrder(/*int page, int size, string order, */string epo)
        {
            string dispositionUri = "unit-payment-orders/EPO";
            IHttpClientService httpClient = (IHttpClientService)this.ServiceProvider.GetService(typeof(IHttpClientService));
            var response = httpClient.GetAsync($"{APIEndpoint.Purchasing}{dispositionUri}/{epo}").Result;
            if(response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                List<UnitPaymentOrderViewModel> upo = JsonConvert.DeserializeObject<List<UnitPaymentOrderViewModel>>(result.GetValueOrDefault("data").ToString()); ;
                return upo;
            }
            else
            {
                return null;
            }
        }


		public ReadResponse<PurchasingDispositionExpeditionModel> ReadBankExpenditureNoteNo(int page, int size, string order, List<string> select, string keyword, string filter)
		{

			IQueryable<PurchasingDispositionExpeditionModel> Query = this.DbSet.Where(m=>m.BankExpenditureNoteNo !=null).Include(m => m.Items);
			List<string> searchAttributes = new List<string>()
			{
				"BankExpenditureNoteNo"
			};

			Query = QueryHelper<PurchasingDispositionExpeditionModel>.Search(Query, searchAttributes, keyword);

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			Query = QueryHelper<PurchasingDispositionExpeditionModel>.Filter(Query, FilterDictionary);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			Query = QueryHelper<PurchasingDispositionExpeditionModel>.Order(Query, OrderDictionary);

			Pageable<PurchasingDispositionExpeditionModel> pageable = new Pageable<PurchasingDispositionExpeditionModel>(Query, page - 1, size);
			List<PurchasingDispositionExpeditionModel> Data = pageable.Data.ToList();
			int TotalData = pageable.TotalCount;

			return new ReadResponse<PurchasingDispositionExpeditionModel>(Data, TotalData, OrderDictionary, new List<string>());
		}
	}
}

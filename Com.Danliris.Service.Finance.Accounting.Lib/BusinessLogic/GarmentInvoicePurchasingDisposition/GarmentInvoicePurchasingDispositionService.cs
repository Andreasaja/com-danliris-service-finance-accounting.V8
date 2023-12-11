using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDispositionExpedition;
using Newtonsoft.Json;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Moonlay.NetCore.Lib;
using System.Net.Http;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePurchasingDisposition.Report;
using System.IO;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentInvoicePurchasingDisposition
{
	public class GarmentInvoicePurchasingDispositionService : IGarmentInvoicePurchasingDispositionService
	{
		private const string UserAgent = "finance-service";
		protected DbSet<GarmentInvoicePurchasingDispositionModel> DbSet;
		public IIdentityService IdentityService;
		private readonly IAutoDailyBankTransactionService _autoDailyBankTransactionService;
		public readonly IServiceProvider ServiceProvider;
		public FinanceDbContext DbContext;

		public GarmentInvoicePurchasingDispositionService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
		{
			DbContext = dbContext;
			ServiceProvider = serviceProvider;
			DbSet = dbContext.Set<GarmentInvoicePurchasingDispositionModel>();
			IdentityService = serviceProvider.GetService<IIdentityService>();
			_autoDailyBankTransactionService = serviceProvider.GetService<IAutoDailyBankTransactionService>();
		}

		public void CreateModel(GarmentInvoicePurchasingDispositionModel model)
		{
			EntityExtension.FlagForCreate(model, IdentityService.Username, UserAgent);
			//get last Paid
			var dispositionIds = model.Items.Select(s => s.DispositionId).ToList();
			//var getLastDiposition = DbContext.GarmentInvoicePurchasingDispositions.Where(s => dispositionIds.Contains(s.DispositionNoteId));

			foreach (var item in model.Items)
			{
				GarmentDispositionExpeditionModel expedition = DbContext.GarmentDispositionExpeditions.FirstOrDefault(ex => ex.Id.Equals(item.PurchasingDispositionExpeditionId));
				//var getLastDipositionPaidAmount = getLastDiposition.Where(s=> s.DispositionNoteId == expedition.DispositionNoteId)
				EntityExtension.FlagForCreate(item, IdentityService.Username, UserAgent);
				if (item.TotalPaidBefore + item.TotalPaid >= item.TotalAmount)
				{
					expedition.IsPaid = true;
				}
				else
				{
					expedition.IsPaid = false;
				}
				expedition.BankExpenditureNoteNo = model.InvoiceNo;
				expedition.BankExpenditureNoteDate = model.InvoiceDate;
			}
			DbSet.Add(model);
		}

		public async Task<int> CreateAsync(GarmentInvoicePurchasingDispositionModel model)
		{
			model.InvoiceNo = await GetDocumentNo("K", model.BankCode, IdentityService.Username, model.InvoiceDate);

			//if (model.CurrencyCode != "IDR")
			//{
			//var garmentCurrency = await GetGarmentCurrency(model.CurrencyCode);
			//model.CurrencyRate = garmentCurrency.Rate.GetValueOrDefault();
			model.CurrencyRate = model.CurrencyRate;
			//}

			CreateModel(model);
			//await _autoDailyBankTransactionService.AutoCreateFromPaymentDisposition(model);
			return await DbContext.SaveChangesAsync();
		}

		private async Task<string> GetDocumentNo(string type, string bankCode, string username, DateTimeOffset dispositionDate)
		{
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				MissingMemberHandling = MissingMemberHandling.Ignore
			};

			var http = ServiceProvider.GetService<IHttpClientService>();
			var uri = APIEndpoint.Purchasing + $"bank-expenditure-notes/bank-document-no-date?type={type}&bankCode={bankCode}&username={username}&date={dispositionDate.ToString("yyyy-MM-dd")}";
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

		public async Task DeleteModel(int id)
		{
			GarmentInvoicePurchasingDispositionModel model = await ReadByIdAsync(id);
			foreach (var item in model.Items)
			{
				GarmentDispositionExpeditionModel expedition = DbContext.GarmentDispositionExpeditions.FirstOrDefault(ex => ex.Id.Equals(item.PurchasingDispositionExpeditionId));
				EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
				expedition.IsPaid = false;
				expedition.BankExpenditureNoteNo = null;
				expedition.BankExpenditureNoteDate = DateTimeOffset.MinValue;
			}
			EntityExtension.FlagForDelete(model, IdentityService.Username, UserAgent, true);
			DbSet.Update(model);
		}

		public async Task<int> DeleteAsync(int id)
		{
			var existingModel = DbSet
						.Include(d => d.Items)
						.Single(dispo => dispo.Id == id && !dispo.IsDeleted);

			//await _autoDailyBankTransactionService.AutoCreateFromPaymentDisposition(existingModel);
			await DeleteModel(id);
			return await DbContext.SaveChangesAsync();
		}

		public async Task<GarmentInvoicePurchasingDispositionModel> ReadByIdAsync(int id)
		{
			return await DbSet.Include(m => m.Items)
				.FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
		}

		public async Task<int> UpdateAsync(int id, GarmentInvoicePurchasingDispositionModel model)
		{
			var existingModel = DbSet
						.Include(d => d.Items)
						.Single(dispo => dispo.Id == id && !dispo.IsDeleted);
			UpdateModel(id, model);
			return await DbContext.SaveChangesAsync();
		}

		public void UpdateModel(int id, GarmentInvoicePurchasingDispositionModel model)
		{
			GarmentInvoicePurchasingDispositionModel exist = DbSet
						.Include(d => d.Items)
						.Single(dispo => dispo.Id == id && !dispo.IsDeleted);


			exist.ChequeNo = model.ChequeNo;
			exist.InvoiceDate = model.InvoiceDate;

			foreach (var item in exist.Items)
			{
				GarmentInvoicePurchasingDispositionItemModel itemModel = model.Items.FirstOrDefault(prop => prop.Id.Equals(item.Id));

				if (itemModel == null)
				{
					GarmentDispositionExpeditionModel expedition = DbContext.GarmentDispositionExpeditions.FirstOrDefault(ex => ex.Id.Equals(item.PurchasingDispositionExpeditionId));
					expedition.IsPaid = false;
					expedition.BankExpenditureNoteNo = null;
					expedition.BankExpenditureNoteDate = DateTimeOffset.MinValue;

					EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
				}
				else
				{
					item.SetTotalPaid(itemModel.TotalPaid);
					GarmentDispositionExpeditionModel expedition = DbContext.GarmentDispositionExpeditions.FirstOrDefault(ex => ex.Id.Equals(item.PurchasingDispositionExpeditionId));
					if (itemModel.TotalPaidBefore + itemModel.TotalPaid >= itemModel.TotalAmount)
					{
						expedition.IsPaid = true;
					}
					else
					{
						expedition.IsPaid = false;
					}

					EntityExtension.FlagForUpdate(item, IdentityService.Username, UserAgent);

				}
			}

			EntityExtension.FlagForUpdate(exist, IdentityService.Username, UserAgent);
			//DbSet.Update(exist);
		}

		public ReadResponse<GarmentInvoicePurchasingDispositionModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
		{
			IQueryable<GarmentInvoicePurchasingDispositionModel> Query = this.DbSet.Include(m => m.Items);
			List<string> searchAttributes = new List<string>()
			{
				"InvoiceNo", "SupplierName", "CurrencyCode", "BankName", "Items.DispositionNo"
			};

			Query = QueryHelper<GarmentInvoicePurchasingDispositionModel>.Search(Query, searchAttributes, keyword);

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			Query = QueryHelper<GarmentInvoicePurchasingDispositionModel>.Filter(Query, FilterDictionary);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			Query = QueryHelper<GarmentInvoicePurchasingDispositionModel>.Order(Query, OrderDictionary);

			//Pageable<GarmentInvoicePurchasingDispositionModel> pageable = new Pageable<GarmentInvoicePurchasingDispositionModel>(Query, page - 1, size);


			var Data = Query.Skip((page - 1) * size).Take(size).ToList();
			int TotalData = Query.Count();

			return new ReadResponse<GarmentInvoicePurchasingDispositionModel>(Data, TotalData, OrderDictionary, new List<string>());
		}



		public ReadResponse<GarmentInvoicePurchasingDispositionItemModel> ReadDisposition()
		{
			List<GarmentInvoicePurchasingDispositionItemModel> paymentDispositionNoteDetails = DbContext.GarmentInvoicePurchasingDispositionItems.Distinct().ToList();

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>("{ }");

			int TotalData = paymentDispositionNoteDetails.Count;

			return new ReadResponse<GarmentInvoicePurchasingDispositionItemModel>(paymentDispositionNoteDetails, TotalData, OrderDictionary, new List<string>());
		}
		private async Task SetTrueDisposition(string dispositionNo)
		{
			var http = ServiceProvider.GetService<IHttpClientService>();
			await http.PutAsync(APIEndpoint.Purchasing + $"garment-disposition-purchase/update/is-paid-true/{dispositionNo}", new StringContent("{}", Encoding.UTF8, General.JsonMediaType));
		}

		public async Task<int> Post(GarmentInvoicePurchasingDispositionPostingViewModel form)
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

					await _autoDailyBankTransactionService.AutoCreateFromGarmentInvoicePurchasingDisposition(model);
				}
			}

			var result = await DbContext.SaveChangesAsync();

			return result;
		}

		public async Task<List<MonitoringDispositionPayment>> GetMonitoring(string invoiceNo, string dispositionNo, DateTimeOffset startDate, DateTimeOffset endDate, int offset)
		{
			var data = await GetReportQuery(invoiceNo, dispositionNo, startDate, endDate);
			return data ;
		}

		public async Task<List<MonitoringDispositionPayment>> GetReportQuery(string invoiceNo, string dispositionNo, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			DateTimeOffset dateFrom = startDate;
			dateFrom.AddHours(7);
			DateTimeOffset dateTo = endDate;
			dateTo.AddHours(7);
			if (endDate == Convert.ToDateTime("0001-01-01 00:00:00.0000000 +00:00") && startDate == Convert.ToDateTime("0001-01-01 00:00:00.0000000 +00:00"))
			{
				endDate = DateTimeOffset.Now;
			}


			var queryHeader = from aa in DbContext.GarmentInvoicePurchasingDispositions
							  where (aa.InvoiceDate >= dateFrom && aa.InvoiceDate <= dateTo)
							  select new { aa.PaymentType, aa.CurrencyRate, aa.Id, aa.InvoiceNo, aa.InvoiceDate, aa.CurrencySymbol, aa.BankAccountName, aa.BankAccountNo, aa.BankCurrencyCode, aa.SupplierName };
			var query = from a in queryHeader
						join b in DbContext.GarmentInvoicePurchasingDispositionItems on a.Id equals b.GarmentInvoicePurchasingDispositionId
						select new MonitoringDispositionPayment
						{
							InvoiceNo = a.InvoiceNo,
							InvoiceDate = a.InvoiceDate,
							DispositionNo = b.DispositionNo,
							DispositionDate = b.DispositionDate,
							DispositionDueDate = b.DipositionDueDate,
							BankName = a.BankAccountName + "-" + a.BankCurrencyCode + "-" + a.BankAccountNo,
							CurrencySymbol = a.CurrencySymbol,
							SupplierName = a.SupplierName,
							ProformaNo = b.ProformaNo,
							VatAmount = b.VATAmount,
							TotalAmount = b.TotalAmount,
							TotalPaidToSupplier = b.TotalPaid * a.CurrencyRate,
							TotalDifference = (b.TotalAmount - b.TotalPaid),
							Paid = b.TotalPaid,
							PaymentType = a.PaymentType,
							Category = b.Category
						};

			if(invoiceNo!=null && dispositionNo == null)
			{
				query = query.Where(s => s.InvoiceNo == invoiceNo);
			}
			if(dispositionNo!=null && invoiceNo == null)
			{
				query = query.Where(s => s.DispositionNo == dispositionNo);
			}
			if (invoiceNo !=null && dispositionNo !=null)
			{
				query = query.Where(s => s.DispositionNo == dispositionNo && s.InvoiceNo == invoiceNo);
			}

			List<MonitoringDispositionPayment> data = new List<MonitoringDispositionPayment>();
			double currentTotal = 0;
			double currentTotalDiff = 0;
			List<string> tempDispo = new List<string>();
		 
			int count = 0;
			foreach (var item in query.OrderBy(s => s.DispositionNo))
			{
				tempDispo.Add(item.DispositionNo);
				if (count < query.Count() && count > 0)
				{
					if (tempDispo[count] == tempDispo[count - 1])
					{
						currentTotal += item.Paid;
						currentTotalDiff -= (item.Paid);
					}
					else
					{
						currentTotal = item.Paid;
						currentTotalDiff = item.TotalAmount - item.Paid;
					}
				}
				else if (count == 0)
				{
					currentTotal = item.Paid;
					currentTotalDiff = item.TotalAmount - item.Paid;
				}
				count++;
				MonitoringDispositionPayment payment = new MonitoringDispositionPayment
				{
					InvoiceNo = item.InvoiceNo,
					InvoiceDate = item.InvoiceDate,
					DispositionNo = item.DispositionNo,
					DispositionDate = item.DispositionDate,
					DispositionDueDate = item.DispositionDueDate,
					BankName = item.BankName,
					CurrencySymbol = item.CurrencySymbol,
					SupplierName = item.SupplierName,
					ProformaNo = item.ProformaNo,
					VatAmount = item.VatAmount,
					TotalAmount = item.TotalAmount,
					TotalPaidToSupplier = item.TotalPaidToSupplier,
					TotalDifference = Math.Round(currentTotalDiff,2),
					Paid = item.TotalPaid,
					PaymentType = item.PaymentType,
					Category = item.Category,
					TotalPaid = currentTotal

				};
				data.Add(payment);
			}


			return data;
		}

		public ReadResponse<GarmentInvoicePurchasingDispositionItemModel> ReadDetailsByEPOId(string epoId)
		{
			throw new NotImplementedException();
		}

		public ReadResponse<GarmentInvoicePurchasingDispositionNoVM> GetLoader(string keyword, string filter)
		{
			IQueryable<GarmentInvoicePurchasingDispositionNoVM> Query = (from a in DbContext.GarmentInvoicePurchasingDispositionItems
																		 select new
																		 GarmentInvoicePurchasingDispositionNoVM
																		 {
																			 DispositionNo = a.DispositionNo
																		 }).Distinct();

			Query = Query
				.Select(s => new GarmentInvoicePurchasingDispositionNoVM
				{
					DispositionNo = s.DispositionNo
				});

			List<string> searchAttributes = new List<string>()
			{
				"DispositionNo"
			};

			Query = QueryHelper<GarmentInvoicePurchasingDispositionNoVM>.Search(Query, searchAttributes, keyword);

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			Query = QueryHelper<GarmentInvoicePurchasingDispositionNoVM>.Filter(Query, FilterDictionary);

			return new ReadResponse<GarmentInvoicePurchasingDispositionNoVM>(Query.Distinct().ToList(), Query.Count(), new Dictionary<string, string>(), new List<string>());

		}

		public async Task<MemoryStream> DownloadReportXls(string invoiceNo, string dispositionNo, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			var data = await GetMonitoring(invoiceNo, dispositionNo, startDate, endDate,7);
			DataTable result = new DataTable();
			result.Columns.Add(new DataColumn() { ColumnName = "No Bukti Pembayaran Disposisi", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Bayar Disposisi", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "No Disposisi", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Disposisi", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Jatuh Tempo", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Bank Bayar", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Mata Uang", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Supplier", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Nomor Proforma Invoice", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Kategori", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "PPN", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Total Pembayaran", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Jumlah dibayar ke Supplier", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Selisih Total yang dibayar", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Total yang sudah dibayar", DataType = typeof(String) });
			result.Columns.Add(new DataColumn() { ColumnName = "Jenis Transaksi", DataType = typeof(String) });


			int counter = 0;


			foreach (var item in data)
			{
				counter++;
				string invoicedate = "";
				string dispositionDate = "";
				string dispositionDueDate = "";
				if (item.InvoiceDate == Convert.ToDateTime("0001-01-01 00:00:00.0000000 +00:00"))
				{
					invoicedate = "-";
				}
				else
				{
					invoicedate =   (item.InvoiceDate.AddHours(7)).ToString("dd-MMMM-yyyy");
				}
				if (item.DispositionDate == Convert.ToDateTime("0001-01-01 00:00:00.0000000 +00:00"))
				{
					dispositionDate = "-";
				}
				else
				{
					dispositionDate =  (item.DispositionDate.AddHours(7)).ToString("dd-MMMM-yyyy");
				}
				if (item.DispositionDueDate == Convert.ToDateTime("0001-01-01 00:00:00.0000000 +00:00"))
				{
					dispositionDueDate = "-";
				}
				else
				{
					dispositionDueDate = (item.DispositionDueDate.AddHours(7)).ToString("dd-MMMM-yyyy");
				}
				result.Rows.Add(item.InvoiceNo,invoicedate, item.DispositionNo, dispositionDate, dispositionDueDate,
				item.BankName, item.CurrencySymbol, item.SupplierName, item.ProformaNo, item.Category, item.VatAmount, item.TotalAmount, item.TotalPaidToSupplier, item.TotalDifference, item.TotalPaid, item.PaymentType);
			}


			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
				worksheet.Cells["A1"].Value = "LAPORAN BUKTI PEMBAYARAN DISPOSISI GARMENT ";
				worksheet.Cells["A1"].Style.Font.Size = 14;
				worksheet.Cells["A1"].Style.Font.Bold = true;
				worksheet.Cells["A2"].LoadFromDataTable(result, true);

				worksheet.Cells["A" + 2 + ":P" + (counter + 2) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 2 + ":P" + (counter + 2) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 2 + ":P" + (counter + 2) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				worksheet.Cells["A" + 2 + ":P" + (counter + 2) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
			 
				worksheet.Cells["A" + 2 + ":P" + 2 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				if (data.Count > 0)
				{
					foreach (var cell in worksheet.Cells["K" + 3 + ":O" + (counter + 2) + ""])
					{
						cell.Value = Convert.ToDecimal(cell.Value);
						cell.Style.Numberformat.Format = "#,##0.00";
						cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
					}
				}
				worksheet.Cells["A" + 2 + ":P" + (counter + 2) + ""].AutoFitColumns();
				worksheet.Cells["A" + (2) + ":P" + (2) + ""].Style.Font.Bold = true;


				var stream = new MemoryStream();

				package.SaveAs(stream);
				return stream;
			}
		}
	}
}